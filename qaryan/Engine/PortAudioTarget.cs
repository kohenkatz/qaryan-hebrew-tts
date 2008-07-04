//    This file is part of Qaryan.
//
//    Qaryan is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Qaryan is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Qaryan.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MotiZilberman;
using PortAudioSharp;

namespace Qaryan.Audio
{
    /// <summary>
    /// An <see cref="AudioTarget">AudioTarget</see> encapsulating a DirectSound output device.
    /// </summary>
    public class PortAudioTarget : AudioTarget
    {
        public override string Name
        {
            get
            {
                return base.Name + " (PortAudio)";
            }
        }

        PortAudioPlayer Player;
        
        Queue<byte[]> Buffers;
        int bufPos = 0;

        PortAudio.PaStreamCallbackResult PullAudio(System.IntPtr input, System.IntPtr output, uint frameCount, ref PortAudioSharp.PortAudio.PaStreamCallbackTimeInfo timeInfo, PortAudioSharp.PortAudio.PaStreamCallbackFlags statusFlags, System.IntPtr userData)
        {
            uint length = frameCount;
            //System.Runtime.InteropServices.Marshal.WriteInt16
            if ((!IsRunning) && (Buffers.Count < 1))
            {
                Log(LogLevel.Debug, "Not running and no buffers left");
                Player.Stop();
                if (Eof)
                    _AudioFinished();
                return PortAudio.PaStreamCallbackResult.paAbort;
            }
            else
            {
                int pos = 0;
                if (Buffers.Count < 1)
                {
                    //Log(LogLevel.Warning, "Buffer underrun, {0} bytes requested",length);
                    return PortAudio.PaStreamCallbackResult.paContinue;
                }
                else
                {
                    int copied = 0;
                    while ((copied < length) && (Buffers.Count > 0))
                    {
                        byte[] buf = Buffers.Peek();
                        int toCopy = (int)length - copied;
                        int availCopy = Math.Min(buf.Length - bufPos, toCopy);
                        System.Runtime.InteropServices.Marshal.Copy(buf, bufPos, output, availCopy);
                        copied += availCopy;
                        bufPos += availCopy;
//                        pos += availCopy;
                        output = (IntPtr)((int)output + availCopy);
                        if (bufPos >= buf.Length)
                        {
                            //Log(LogLevel.Debug, "End of queued buffer ({0} bytes)", buf.Length);
                            Buffers.Dequeue();
                            bufPos = 0;
                        }
                    }
                    //Log(LogLevel.Debug, "Copied {0} bytes into {1} byte device buffer", copied, length);
                    /*if ((length>copied) && (!Eof) && (IsRunning))
                        Log(LogLevel.Warning, "Buffer underrun, {0} bytes short", length-copied);*/
                }
                return PortAudio.PaStreamCallbackResult.paContinue;
            }
        }

        protected override void Open(WaveFormat format)
        {
            Log(LogLevel.Info, "Open {0}ch, {1} bits/sample, {2} Hz PCM output", format.Channels, format.BitsPerSample, format.SamplesPerSecond);
            bufPos = 0;
            Buffers = new Queue<byte[]>();
            Player = new PortAudioPlayer(format.Channels,(int)format.SamplesPerSecond,format.SamplesPerSecond*5,PullAudio);
            Player.Start();
        }

        protected override void PlayBuffer(AudioBufferInfo buffer)
        {
            Buffers.Enqueue(buffer.Data);
        }

        protected override void Close()
        {
        }

        public override void Stop()
        {
            if (Player != null)
                Player.Dispose();
            Buffers.Clear();
            base.Stop();
        }

        public override bool IsRunning
        {
            get
            {
                return base.IsRunning || (Buffers.Count > 0);
            }
        }

        public void PlaySync()
        {
            this.Join();
        }

        public override void Join()
        {
            base.Join();
/*            if (Player != null)
                Player.Join();*/
        }
        Control owner;

        public PortAudioTarget(Control owner)
        {
            this.owner = owner;
        }
    }
}

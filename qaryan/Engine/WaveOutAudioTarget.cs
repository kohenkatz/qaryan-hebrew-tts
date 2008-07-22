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
using WaveLib;

namespace Qaryan.Audio
{
    /// <summary>
    /// An <see cref="AudioTarget">AudioTarget</see> encapsulating a DirectSound output device.
    /// </summary>
    public class WaveOutAudioTarget : AudioTarget
    {
        public override string Name
        {
            get
            {
                return base.Name + " (waveOut)";
            }
        }

        public override bool PlatformSupported
        {
            get
            {
                try
                {
                    WaveNative.waveOutGetNumDevs();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        WaveOutPlayer Player;

        Queue<byte[]> Buffers;
        int bufPos = 0;

        void PullAudio(IntPtr buffer, int size)
        {
            if ((!IsRunning) && (Buffers.Count < 1))
            {
                Log(LogLevel.Debug, "Not running and no buffers left");
                Stop();
                if (Eof)
                    _AudioPlaybackFinished();
            }
            else
            {
                if (Buffers.Count < 1)
                {
                    Log(LogLevel.Debug, "Buffer underrun, {0} bytes requested",size);
                    return;
                }
                else
                {
                    int copied = 0;
                    while ((copied < size) && (Buffers.Count > 0))
                    {
                        byte[] buf = Buffers.Peek();
                        int toCopy = size - copied;
                        int availCopy = Math.Min(buf.Length - bufPos, toCopy);
                        System.Runtime.InteropServices.Marshal.Copy(buf, bufPos, buffer, availCopy);
                        copied += availCopy;
                        bufPos += availCopy;
                        buffer = (IntPtr)((int)buffer + availCopy);
                        if (bufPos >= buf.Length)
                        {
                            Log(LogLevel.Debug, "End of queued buffer ({0} bytes)", buf.Length);
                            Buffers.Dequeue();
                            bufPos = 0;
                        }
                    }
                    Log(LogLevel.Debug, "Copied {0} bytes into {1} byte device buffer", copied, size);
                    /*if ((length>copied) && (!Eof) && (IsRunning))
                        Log(LogLevel.Warning, "Buffer underrun, {0} bytes short", length-copied);*/
                }
            }
        }

        protected override void Open(WaveFormat format)
        {
            Log(LogLevel.Info, "Open {0}ch, {1} bits/sample, {2} Hz PCM output", format.Channels, format.BitsPerSample, format.SamplesPerSecond);
            bufPos = 0;
            Buffers = new Queue<byte[]>();
            Player = new WaveOutPlayer(0, new WinWaveFormat(format),4096,3,PullAudio);
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
            Buffers.Clear();
            base.Stop();
            try
            {
                if (Player != null)
                    Player.Dispose();
            }
            finally
            {
                Player = null;
            }
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
        }

        public WaveOutAudioTarget()
        {
        }
    }
}
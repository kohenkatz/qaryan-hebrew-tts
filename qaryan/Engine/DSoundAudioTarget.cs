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

namespace Qaryan.Audio
{
    public class DSoundAudioTarget : AudioTarget
    {
        public override string Name
        {
            get
            {
                return base.Name+" (DirectX)";
            }
        }

        SoundPlayer Player;

        Queue<byte[]> Buffers;
        int bufPos = 0;

        void PullAudio(byte[] buffer, int length)
        {
            if ((!IsRunning)&&(Buffers.Count < 1))
            {
                Player.Stop();
                Log(LogLevel.Debug, "Not running and no buffers left");
                if (Eof)
                    _AudioFinished();
            }
            else
            {
                int pos = 0;
                if (Buffers.Count < 1)
                {
                    Log(LogLevel.Warning, "Buffer underrun, {0} bytes requested",length);
                    return;
                }
                else
                {
                    int copied = 0;
                    while ((copied < length) && (Buffers.Count > 0))
                    {
                        byte[] buf = Buffers.Peek();
                        int toCopy = length - copied;
                        int availCopy = Math.Min(buf.Length - bufPos, toCopy);
                        System.Buffer.BlockCopy(buf, bufPos, buffer, pos, availCopy);
                        
                        copied += availCopy;
                        bufPos += availCopy;
                        pos += availCopy;
                        if (bufPos >= buf.Length)
                        {
                            Log(LogLevel.Debug, "End of queued buffer ({0} bytes)", buf.Length);
                            Buffers.Dequeue();
                            bufPos = 0;
                        }
                    }
                    Log(LogLevel.Debug, "Copied {0} bytes into {1} byte device buffer", copied, length);
                    if ((length>copied) && (!Eof) && (IsRunning))
                        Log(LogLevel.Warning, "Buffer underrun, {0} bytes short", length-copied);
                }
            }
        }

        protected override void Open(WaveFormat format)
        {
            bufPos = 0;
            Buffers = new Queue<byte[]>();
            Player = new SoundPlayer(owner, PullAudio, format);
            Player.Resume();
            Log(LogLevel.Info, "Open {0}ch, {1} bits/sample, {2} Hz PCM output", format.Channels, format.BitsPerSample, format.SamplesPerSecond);
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
                return base.IsRunning || (Buffers.Count>0);
            }
        }

        public void PlaySync()
        {
            this.Join();
        }

        public override void Join()
        {
            base.Join();
            if (Player!=null)
                Player.Join();
        }
        Control owner;

        public DSoundAudioTarget(Control owner)
        {
            this.owner = owner;
        }
    }
}

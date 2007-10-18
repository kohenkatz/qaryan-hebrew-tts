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

namespace Qaryan.Audio
{
    public class DSoundAudioTarget : AudioTarget
    {
        SoundPlayer Player;

        Queue<byte[]> Buffers;
        int bufPos = 0;

        int silence = 0;
        bool dirty = false;

        void PullAudio(byte[] buffer, int length)
        {
            if (!IsRunning)
            {
                Player.Stop();
            }
            int pos = 0;
            if (Buffers.Count < 1)
                return;
            else
            {
                int copied = 0;
                while ((copied < length)&&(Buffers.Count>0))
                {
                    byte[] buf = Buffers.Peek();
                    int toCopy = length - copied;
                    int availCopy = Math.Min(buf.Length - bufPos, toCopy);
                    System.Buffer.BlockCopy(buf, bufPos, buffer, pos,availCopy);
                    copied += availCopy;
                    bufPos += availCopy;
                    pos += availCopy;
                    if (bufPos >= buf.Length)
                    {
                        Buffers.Dequeue();
                        bufPos = 0;
                    }
                }
            }

        }

        protected override void Open(WaveFormat format)
        {
            dirty = false;
            bufPos = 0;
            silence = 0;
            Buffers = new Queue<byte[]>();
            Player = new SoundPlayer(owner, PullAudio, format);
            Player.Resume();


        }

        protected override void PlayBuffer(AudioBufferInfo buffer)
        {
            Buffers.Enqueue(buffer.Data);

        }

        protected override void Close()
        {
            dirty = true;
        }

        public override void Stop()
        {
            dirty = true;
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
        
        Control owner;

        public DSoundAudioTarget(Control owner)
        {
            this.owner = owner;
        }
    }
}

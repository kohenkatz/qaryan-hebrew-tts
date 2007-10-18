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
using System.Runtime.InteropServices;
using DirectTextToSpeech;
using Qaryan.Audio;

namespace Qaryan.Interfaces.SAPI4
{
    public class Sapi4AudioTarget : AudioTarget
    {

        Queue<byte[]> Buffers;
        int bufPos = 0;

        protected override void Open(WaveFormat format)
        {
            Buffers = new Queue<byte[]>();
            bufPos = 0;
            SDATA s = new SDATA();
            WAVEFORMATEX wfx = new WAVEFORMATEX();
            wfx.cbSize = 0;
            wfx.nAvgBytesPerSec = (int)format.AverageBytesPerSecond;
            wfx.nBlockAlign = (short)format.BlockAlign;
            wfx.nChannels = (short)format.Channels;
            wfx.nSamplesPerSec = (int)format.SamplesPerSecond;
            wfx.wBitsPerSample = (short)format.BitsPerSample;
            wfx.wFormatTag = (short)format.FormatTag;
            s.AsWaveFormatEx = wfx;
            Sapi4Engine.miniLog("audio.WaveFormatSet");
            audio.WaveFormatSet(s);
            Sapi4Engine.miniLog("audio.LevelGet");
            int level;
            audio.LevelGet(out level);
            Sapi4Engine.miniLog(String.Format("level={0}", level));
//            wfx = s.AsWaveFormatEx;
            Sapi4Engine.miniLog(String.Format("{0} Hz, {1} bit, {2} channels", wfx.nSamplesPerSec, wfx.wBitsPerSample, wfx.nChannels));
            Sapi4Engine.miniLog("audio.Claim");
            audio.Claim();
            Sapi4Engine.miniLog("audio.Start");
            audio.Start();
            Sapi4Engine.miniLog(" opened successfully");
        }

        protected override void PlayBuffer(AudioBufferInfo buffer)
        {
            Sapi4Engine.miniLog("entering PlayBuffer");
            Buffers.Enqueue(buffer.Data);
            int pos = 0;
            if (Buffers.Count < 1)
                return;
            else
            {
                Sapi4Engine.miniLog("at PlayBuffer");
                int length = 0; bool destEof = false;
                do
                {
                    audioDest.FreeSpace(ref length, ref destEof);
                    Sapi4Engine.miniLog(String.Format("{0} bytes. eof: {1}", length, destEof));
                    if (!destEof)
                    {
                        int copied = 0;
                        while ((copied < length) && (Buffers.Count > 0))
                        {
                            byte[] buf = Buffers.Peek();
                            int toCopy = length - copied;
                            int availCopy = Math.Min(buf.Length - bufPos, toCopy);
                            IntPtr pmem = Marshal.AllocCoTaskMem(availCopy);
                            Marshal.Copy(buf, bufPos, pmem, availCopy);
                            Sapi4Engine.miniLog("before DataSet");
                            audioDest.DataSet(pmem, availCopy);
                            Marshal.FreeCoTaskMem(pmem);
                            Sapi4Engine.miniLog("after DataSet");
                            //System.Buffer.BlockCopy(buf, bufPos, buffer, pos, availCopy);
                            copied += availCopy;
                            bufPos += availCopy;
                            pos += availCopy;
                            if (bufPos >= buf.Length)
                            {
                                Buffers.Dequeue();
                                bufPos = 0;
                            }
                        }
                        audioDest.FreeSpace(ref length, ref destEof);
                        Sapi4Engine.miniLog(String.Format("second round {0} bytes. eof: {1}", length, destEof));
                    }
                    System.Threading.Thread.Sleep(0);
                }
                while ((Buffers.Count > 0) && (destEof || (length == 0)));
            }
            Sapi4Engine.miniLog("leaving PlayBuffer");
        }

        protected override void Close()
        {
            audio.UnClaim();
        }

        public override void Stop()
        {
            Buffers.Clear();
            audio.Flush();
            audio.Stop();
        }

        public void PlaySync()
        {
            this.Join();
        }

        IAudioDest audioDest;
        IAudio audio;

        public Sapi4AudioTarget(IAudioDest audioDest)
        {
            this.audioDest = audioDest;
            this.audio = audioDest as IAudio;
            Sapi4Engine.miniLog(
                String.Format(
                "audio target created, AudioDest is {0}, Audio is {1}", audioDest, audio));
        }
    }
}

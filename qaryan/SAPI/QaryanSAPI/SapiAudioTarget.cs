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
using Qaryan.Audio;
using System.Speech.Synthesis.TtsEngine;
using System.Runtime.InteropServices;

namespace Qaryan.SAPI
{
    internal class SapiAudioTarget : AudioTarget
    {
        ITtsEngineSite site;

        public SapiAudioTarget(ITtsEngineSite site)
        {
            this.site = site;
        }

        protected override void Open(WaveFormat format)
        {
//            writer = new WaveFileWriter(File.Create(Filename), format.Channels, format.SamplesPerSecond, format.AverageBytesPerSecond, format.BlockAlign, format.BitsPerSample,WriteHeader);
        }

        protected override void PlayBuffer(AudioBufferInfo buffer)
        {
            int index = 0;
            while (index < buffer.Data.Length)
            {
                index = site.Write(Marshal.UnsafeAddrOfPinnedArrayElement(buffer.Data, index), buffer.Data.Length - index);
            }
        }

        protected override void Close()
        {
        }

        public void PlaySync()
        {
            this.Join();
        }

        public override void Stop()
        {
            //            throw new Exception("The method or operation is not implemented.");
        }
    }
}
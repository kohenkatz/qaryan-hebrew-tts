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
using Xiph.Audio.Output;
using Qaryan.Core;

namespace Qaryan.Audio
{
    public class LibaoAudioTarget : AudioTarget
    {
        public override bool PlatformSupported
        {
            get
            {
                try
                {
                    Xiph.Audio.Output.LowLevel.libao.ao_is_big_endian();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

		AoDevice device;
        protected override void Open(WaveFormat format)
        {
            AoDriver driver=AoDriver.Default;
            if (driver==null)
                driver=AoDriver.FromName("alsa");
            if (driver==null)
                driver=AoDriver.FromName("win32");
			device=driver.OpenLive(format.BitsPerSample,
			                                 (int)format.SamplesPerSecond,
			                                 format.Channels);
        }

        protected override void PlayBuffer(AudioBufferInfo buffer)
        {
            device.Play(buffer.Data);
        }

        protected override void Close()
        {
           
        }

        public void PlaySync()
        {
			this.Join();
     	}

		public override void Join ()
		{
            while (IsRunning)
                System.Threading.Thread.Sleep(0);
		}

        public override void Stop()
        {
            //            throw new Exception("The method or operation is not implemented.");
        }

        protected override void AudioProviderFinished()
        {
            base.AudioProviderFinished();
            _AudioPlaybackFinished();
        }
    }
}
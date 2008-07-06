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
using MotiZilberman;
using Qaryan.Core;
using Qaryan.Audio;

namespace Qaryan.Core
{
    /// <summary>
    /// A specialized audio provider capable of synthesizing speech from objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to consume.</typeparam>
    public abstract class Synthesizer<T> : LookaheadConsumerProducer<T,AudioBufferInfo>,AudioProvider,IPlatformSupported
    {
        WaveFormat fmt=new WaveFormat();
        public virtual WaveFormat AudioFormat
        {
            get
            {
                return fmt;
            }
            set
            {
                fmt = value;
            }
        }

        protected void BufferReady(byte[] data) {
            AudioBufferInfo abuf = new AudioBufferInfo();
            abuf.Data = data;
            abuf.Format = fmt;
            Emit(abuf);
        }

        public event SimpleNotify AudioFinished;
        protected internal void InvokeAudioFinished()
        {
            if (AudioFinished != null)
                AudioFinished();
            _DoneProducing();
        }

        #region IPlatformSupported Members

        public virtual bool PlatformSupported
        {
            get { return true; }
        }

        #endregion
    }
}

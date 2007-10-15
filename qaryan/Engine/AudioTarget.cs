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

namespace Qaryan.Audio
{
    public abstract class AudioTarget: LookaheadConsumer<AudioBufferInfo>
    {
        public event SimpleNotify AudioFinished;

        AudioProvider Provider;

        protected abstract void Open(WaveFormat format);
        protected abstract void PlayBuffer(AudioBufferInfo buffer);
        protected abstract void Close();

        public virtual void Stop()
        {
            AudioFinished();
        }

        private bool Eof = false;
        private bool isConsuming = false;

        protected override void BeforeConsumption()
        {
            base.BeforeConsumption();
            isConsuming = true;
            Eof = false;
            Open(Provider.AudioFormat);
        }

        protected override void Consume(Queue<AudioBufferInfo> InQueue)
        {
            while (InQueue.Count > 0)
            {
                AudioBufferInfo abi=InQueue.Dequeue();
                PlayBuffer(abi);
                _ItemConsumed(abi);
            }
        }

        protected override void AfterConsumption()
        {
            base.AfterConsumption();
            isConsuming = false;
        }

        protected void OnAudioFinished()
        {
            Eof = true;
            Close();
            if (AudioFinished != null)
                AudioFinished();
            _DoneConsuming();
        }

        public virtual bool IsRunning
        {
            get
            {
                return isConsuming || !Eof;
            }
        }

        public override void Run(Producer<AudioBufferInfo> producer)
        {
            if (!(producer is AudioProvider))
                throw new ArgumentException("AudioTarget can only be run on an AudioProvider");
            Provider = producer as AudioProvider;
            Provider.AudioFinished += OnAudioFinished;
            base.Run(producer);
        }
    }
}

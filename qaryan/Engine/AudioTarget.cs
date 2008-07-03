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
    /// <summary>
    /// A specialized <see cref="Producer">Producer</see> designed to convey out-of-stream information regarding produced audio buffers.
    /// </summary>
    /// <seealso cref="AudioBufferInfo"/>
    /// <seealso cref="AudioTarget"/>
    public interface AudioProvider : Producer<AudioBufferInfo>
    {
        /// <summary>
        /// Gets or sets the audio provider's audio format.
        /// </summary>
        WaveFormat AudioFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Notifies interested clients (notably <see cref="AudioTarget"/>s) when audio output is over.
        /// </summary>
        event SimpleNotify AudioFinished;
    }

    /// <summary>
    /// Represents an abstract audio device capable of playing <see cref="AudioBufferInfo">AudioBufferInfo</see> objects.
    /// </summary>
    public abstract class AudioTarget: LookaheadConsumer<AudioBufferInfo>
    {
        public override string Name
        {
            get
            {
                return "Audio";
            }
        }

        /// <summary>
        /// Notifies interested clients when audio playback is over.
        /// </summary>
        public event SimpleNotify AudioFinished;

        AudioProvider Provider;

        protected abstract void Open(WaveFormat format);
        protected abstract void PlayBuffer(AudioBufferInfo buffer);
        protected abstract void Close();

        /// <summary>
        /// Stops playback immediately.
        /// </summary>
        public virtual void Stop()
        {
            AudioFinished();
        }

        private bool eof = false;

        /// <summary>
        /// A flag signifying whether the attached AudioProvider is still generating buffers (<see langword="false"/>) or has finished doing so (<see langword="true"/>).
        /// </summary>
        protected bool Eof
        {
            get
            {
                return eof;
            }
        }

        private bool isConsuming = false;

        protected override void BeforeConsumption()
        {
            Log(LogLevel.MajorInfo, "Start buffering");
            base.BeforeConsumption();
            isConsuming = true;
            eof = false;
            Open(Provider.AudioFormat);
        }

        protected override void Consume(Queue<AudioBufferInfo> InQueue)
        {
            while (InQueue.Count > 0)
            {
                AudioBufferInfo abi=InQueue.Dequeue();
                /*if (eof)
                {
                    Log(LogLevel.Debug, "Skip buffer, {0} bytes (eof)", abi.Data.Length);
                    continue;
                }
                else
                {*/
                    Log(LogLevel.Debug, "Play buffer, {0} bytes", abi.Data.Length);
                    PlayBuffer(abi);
                    _ItemConsumed(abi);
            //    }
            }
        }

        protected override void AfterConsumption()
        {
            Log(LogLevel.MajorInfo, "Finished buffering");
            base.AfterConsumption();
            isConsuming = false;
        }

        protected void OnAudioFinished()
        {
            Log(LogLevel.Debug, "End of audio at source");

            _DoneConsuming();
            eof = true;
        }

        /// <summary>
        /// Returns whether the object is currently consuming buffers.
        /// </summary>
        public virtual bool IsRunning
        {
            get
            {
                return isConsuming || !eof;
            }
        }

        public override void Run(Producer<AudioBufferInfo> producer)
        {
            if (!(producer is AudioProvider))
            {
                Log(LogLevel.Error, "Attempted run on non-AudioTarget ({0})", producer.GetType().FullName);
                throw new ArgumentException("AudioTarget can only be run on an AudioProvider");
            }
            Provider = producer as AudioProvider;
            Provider.AudioFinished += OnAudioFinished;
            base.Run(producer);
        }

        protected void _AudioFinished()
        {
            Log(LogLevel.Info, "Audio playback finished");
            Close();
            if (AudioFinished != null)
                AudioFinished();
        }
    }
}

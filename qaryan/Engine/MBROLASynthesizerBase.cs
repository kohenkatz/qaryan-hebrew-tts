using System;
using System.Collections.Generic;
using System.Text;
using Qaryan.Audio;
using Qaryan.Core;
using MotiZilberman;

namespace Qaryan.Synths.MBROLA
{
    public class MBROLASynthesizerBase : Synthesizer<MBROLAElement>
    {
        MBROLAVoice voice;

        public virtual MBROLAVoice Voice
        {
            get { return voice; }
            set
            {
                voice = value;
                voice.Activate();
            }
        }

        public override WaveFormat AudioFormat
        {
            get
            {
                WaveFormat format = base.AudioFormat;
                format.Channels = 1;
                if (voice != null)
                    format.SamplesPerSecond = voice.SampleRate;
                else
                    format.SamplesPerSecond = 22050;
                format.BitsPerSample = 16;
                format.AverageBytesPerSecond = format.SamplesPerSecond * format.BitsPerSample / 8;
                format.BlockAlign = (ushort)(format.Channels * (format.BitsPerSample / 8));
                format.FormatTag = WaveFormatTag.Pcm;
                return format;
            }
            set
            {
                base.AudioFormat = value;
            }
        }

        public override void Run(Producer<MBROLAElement> producer)
        {

            base.Run(producer, 1);
        }
    }
}

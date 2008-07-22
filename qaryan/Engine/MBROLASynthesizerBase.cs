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
        MBROLAVoice voiceOld;

        public virtual MBROLAVoice VoiceOld
        {
            get { return voiceOld; }
            set
            {
                voiceOld = value;
                voiceOld.Activate();
            }
        }

        public override WaveFormat AudioFormat
        {
            get
            {
                return Voice.AudioFormat;
            }
        }

        public override void Run(Producer<MBROLAElement> producer)
        {

            base.Run(producer, 1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Speech.Synthesis.TtsEngine;
using Qaryan.Core;
using Qaryan.Synths.MBROLA;
using Qaryan.Audio;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Qaryan.SAPI
{
    [Guid("02D74A76-BEEF-48f9-AE86-86FC6E1FFAD0")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class QaryanSsml : TtsEngineSsml
    {
        public override void AddLexicon(Uri uri, string mediaType, ITtsEngineSite site)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        MBROLAVoice Voice;
        MBROLASynthesizerBase Synth;

        protected QaryanSsml(string registryKey)
            : base(registryKey)
        {
            string voicePath = (string)Registry.GetValue(registryKey + "\\Qaryan", "MBROLAVoice", "");
            Voice = new MBROLAVoice();
            Voice.LoadFromXml(voicePath);

            Synth = new MBROLASynthesizer();
            Synth.VoiceOld = Voice;
        }

        public override IntPtr GetOutputFormat(SpeakOutputFormat format, IntPtr targetWaveFormat)
        {
            WaveFormat waveFormat = Synth.AudioFormat;
            IntPtr wfx = Marshal.AllocCoTaskMem(Marshal.SizeOf(waveFormat));
            Marshal.StructureToPtr(waveFormat, wfx, false);

            return wfx;
        }

        public override void RemoveLexicon(Uri uri, ITtsEngineSite site)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void Speak(TextFragment[] fragment, IntPtr waveHeader, ITtsEngineSite site)
        {
            //throw new Exception("The method or operation is not implemented.");
            StringBuilder sb=new StringBuilder();
            foreach (TextFragment frag in fragment)
            {
                sb.AppendLine(frag.TextToSpeak);
            }
            StringCharProducer prod = new StringCharProducer(sb.ToString());
            Tokenizer tok = new Tokenizer();
            Parser par = new Parser();
            Segmenter seg = new Segmenter();
            Phonetizer pho = new Phonetizer();
            FujisakiProcessor fuji = new FujisakiProcessor();
            MBROLATranslator tra = new MBROLATranslator();
            AudioTarget tar = new SapiAudioTarget(site);
            tra.Voice = Voice;
            tok.Run(prod);
            par.Run(tok);
            seg.Run(par);
            pho.Run(seg);
            fuji.Run(pho);
            tra.Run(fuji);
            Synth.Run(tra);
            tar.Run(Synth);
        }
    }
}

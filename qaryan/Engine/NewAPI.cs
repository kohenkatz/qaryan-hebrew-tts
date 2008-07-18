using System;
using System.Collections.Generic;
using System.Text;
using MotiZilberman;
using Qaryan.Audio;
using Qaryan.Synths.MBROLA;
using System.IO;

namespace Qaryan.Core
{
    public class Voice
    {
    }

    interface Frontend : IConsumerProducer<char, Phone>
    {
    }

    interface Backend : IConsumerProducer<Phone, AudioBufferInfo>, AudioProvider
    {
    }

    class StandardFrontend : Chain<char, Phone>, Frontend
    {
        Tokenizer tok; Parser par; Segmenter seg; Phonetizer pho; FujisakiProcessor fuji;
        public Tokenizer Tokenizer
        {
            get
            {
                return tok;
            }
        }
        public Parser Parser
        {
            get
            {
                return par;
            }
        }
        public Segmenter Segmenter
        {
            get
            {
                return seg;
            }
        }
        public Phonetizer Phonetizer
        {
            get
            {
                return pho;
            }
        }
        public FujisakiProcessor FujisakiProcessor
        {
            get
            {
                return fuji;
            }
        }
        protected override void CreateChain(out Consumer<char> First, out Producer<Phone> Last)
        {
            First = tok = new Tokenizer();
            par = new Parser();
            seg = new Segmenter();
            pho = new Phonetizer();
            Last = fuji = new FujisakiProcessor();
        }

        public override void Run(Producer<char> producer)
        {
            tok.Run(producer);
            par.Run(tok);
            seg.Run(par);
            pho.Run(seg);
            fuji.Run(pho);
        }
    }

    abstract class AudioChain<Tin> : Chain<Tin, AudioBufferInfo>, AudioProvider
    {
        protected AudioChain()
            : base()
        {
            ((AudioProvider)ChainLast).AudioFinished += new SimpleNotify(AudioChain_AudioFinished);
        }

        void AudioChain_AudioFinished()
        {
            if (AudioFinished != null)
                AudioFinished();
        }

        #region AudioProvider Members

        public WaveFormat AudioFormat
        {
            get
            {
                return ((AudioProvider)ChainLast).AudioFormat;
            }
            set
            {
                ((AudioProvider)ChainLast).AudioFormat = value;
            }
        }

        public event SimpleNotify AudioFinished;

        #endregion
    }

    class MbrolaBackend : AudioChain<Phone>, Backend
    {
        MBROLATranslator tra; MBROLASynthesizerBase synth;

        protected override void CreateChain(out Consumer<Phone> First, out Producer<AudioBufferInfo> Last)
        {
            First = tra = new MBROLATranslator();
            Last = synth = PlatformInstantiator<MBROLASynthesizerBase>.Create(
            typeof(MBROLASynthesizer), typeof(MBROLAProcessSynthesizer));
        }

        public override void Run(Producer<Phone> producer)
        {
            tra.Run(producer);
            synth.Run(tra);
        }
    }

    public class QaryanEngine
    {
        Voice _Voice;
        public Voice Voice
        {
            get
            {
                return _Voice;
            }
            set
            {
                _Voice = value;
            }
        }
        Frontend Frontend;
        Backend Backend;
        AudioTarget _audioTarget;

        AudioTarget audioTarget
        {
            set
            {
                _audioTarget = value;
                _audioTarget.AudioFinished += new SimpleNotify(_audioTarget_AudioFinished);
            }
            get
            {
                return audioTarget;
            }
        }

        void _audioTarget_AudioFinished()
        {
            _audioTarget = null;
        }

        public QaryanEngine()
        {
            Frontend = new StandardFrontend();
            Backend = new MbrolaBackend();
        }

        public void Speak(string text, AudioTarget audioTarget)
        {
            if (audioTarget == null)
                return;
            StringCharProducer sp = new StringCharProducer(text);
            Frontend.Run(sp);
            Backend.Run(Frontend);
            this.audioTarget = audioTarget;
            audioTarget.Run(Backend);
        }

        public void Speak(string text)
        {
            Speak(text, PlatformInstantiator<AudioTarget>.Create(
                typeof(LibaoAudioTarget),
                typeof(WaveOutAudioTarget)));
        }

        public void SpeakToWavFile(string text, string fileName)
        {
            WaveFileAudioTarget audio = new WaveFileAudioTarget();
            audio.Filename = fileName;
            Speak(text, audio);
        }

        public void SpeakToRawFile(string text, string fileName)
        {
            WaveFileAudioTarget audio = new WaveFileAudioTarget();
            audio.WriteHeader = false;
            audio.Filename = fileName;
            Speak(text, audio);
        }

        public void SpeakToWavStream(string text, Stream stream)
        {
            StreamAudioTarget audio = new StreamAudioTarget();
            audio.Stream = stream;
            audio.WriteHeader = true;
            Speak(text, audio);
        }

        public void SpeakToRawStream(string text, Stream stream)
        {
            StreamAudioTarget audio = new StreamAudioTarget();
            audio.Stream = stream;
            audio.WriteHeader = false;
            Speak(text, audio);
        }

        public void Join()
        {
            try
            {
                if (_audioTarget != null)
                    _audioTarget.Join();
            }
            catch { }
        }
    }
}

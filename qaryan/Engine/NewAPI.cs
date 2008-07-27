using System;
using System.Collections.Generic;
using System.Text;
using MotiZilberman;
using Qaryan.Audio;
using Qaryan.Synths.MBROLA;
using System.IO;
using System.Xml;

namespace Qaryan.Core
{
    public class Voice
    {
        string _DisplayName, _BackendName;
        BackendVoice _BackendVoice;

        public string BackendName
        {
            get
            {
                return _BackendName;
            }
        }

        internal BackendVoice BackendVoice
        {
            get
            {
                return _BackendVoice;
            }
        }

        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
        }

        void ReadVoiceInfo(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    reader.MoveToContent();
                    switch (reader.Name)
                    {
                        case "DisplayName": _DisplayName = reader.ReadString();
                            break;
                        case "Backend": _BackendName = reader.ReadString();
                            break;
                    }
                }
            }
            reader.Close();
        }

        void Read(XmlReader reader)
        {
            if (!reader.ReadToFollowing("Voice"))
                throw new OperationCanceledException();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "VoiceInfo":
                            ReadVoiceInfo(reader.ReadSubtree());
                            break;
                        case "BackendData":
                            string backend = reader.GetAttribute("For");
                            _BackendVoice = QaryanEngine.CreateBackendVoice(backend);
                            _BackendVoice.ReadBackendData(reader.ReadSubtree());
                            break;
                    }
                }
            }
            reader.Close();
        }

        public void Load(string fileName)
        {
            Read(XmlReader.Create(File.OpenRead(fileName)));
        }

        public bool BackendSupported
        {
            get
            {
                if (BackendVoice == null)
                    return false;
                return BackendVoice.IsSupported;
            }
        }

        public WaveFormat AudioFormat
        {
            get
            {
                if (BackendVoice == null)
                    return new WaveFormat();
                return BackendVoice.AudioFormat;
            }
        }
    }

    public abstract class BackendVoice
    {
        internal abstract void ReadBackendData(XmlReader reader);
        internal virtual bool IsSupported {
            get
            {
                return true;
            }
        }
        internal abstract WaveFormat AudioFormat
        {
            get;
        }
    }


    public interface IHasVoice
    {
        Voice Voice
        {
            get;
            set;
        }
    }

    public interface Frontend : IConsumerProducer<char, Phone>, IHasVoice, ILogSource
    {
    }

    public interface Backend : IConsumerProducer<Phone, AudioBufferInfo>, AudioProvider, IHasVoice, ILogSource
    {
    }

    public class StandardFrontend : Chain<char, Phone>, Frontend
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
            tok.LogLine += new LogLineHandler(OnLogLine);
            par.LogLine+=new LogLineHandler(OnLogLine);
            seg.LogLine+=new LogLineHandler(OnLogLine);
            pho.LogLine+=new LogLineHandler(OnLogLine);
            fuji.LogLine += new LogLineHandler(OnLogLine);
        }

        void OnLogLine(ILogSource sender, string message, LogLevel visibility)
        {
            if (LogLine != null)
                LogLine(sender, message, visibility);
        }

        public override void Run(Producer<char> producer)
        {
            tok.Run(producer);
            par.Run(tok);
            seg.Run(par);
            pho.Run(seg);
            fuji.Run(pho);
        }

        private Voice _Voice;

        #region IHasVoice Members

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

        #endregion

        #region ILogSource Members

        public string Name
        {
            get {
                return "Frontend (Standard)";
            }
        }

        public event LogLineHandler LogLine;

        #endregion
    }

    public abstract class AudioChain<Tin> : Chain<Tin, AudioBufferInfo>, AudioProvider
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
        }

        public event SimpleNotify AudioFinished;

        #endregion
    }



    public class QaryanEngine : ILogSource
    {
        static Dictionary<string, Type> Backends = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        static Dictionary<string, Type> BackendVoices = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        static QaryanEngine()
        {
            QaryanEngine.RegisterBackend("MBROLA", typeof(MbrolaBackend), typeof(MbrolaVoiceNew));
        }

        public static void RegisterBackend(string name, Type backendType, Type backendVoiceType)
        {
            if (typeof(Backend).IsAssignableFrom(backendType) && !Backends.ContainsKey(name))
                Backends[name] = backendType;
            if (typeof(BackendVoice).IsAssignableFrom(backendVoiceType) && !BackendVoices.ContainsKey(name))
                BackendVoices[name] = backendVoiceType;
        }

        public static Backend CreateBackend(string name)
        {
            if (Backends.ContainsKey(name))
                return Backends[name].GetConstructor(new Type[0]).Invoke(new object[0]) as Backend;
            return null;
        }

        public static BackendVoice CreateBackendVoice(string name)
        {
            if (BackendVoices.ContainsKey(name))
                return BackendVoices[name].GetConstructor(new Type[0]).Invoke(new object[0]) as BackendVoice;
            return null;
        }

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
                if (Frontend != null)
                    Frontend.Voice = _Voice;
                if (_Voice != null)
                {
                    if ((Backend==null) || Backend.GetType()!=Backends[Voice.BackendName])
                        Backend = CreateBackend(Voice.BackendName);
                    if (Backend != null)
                        Backend.Voice = _Voice;
                }
                else
                    Backend = null;
            }
        }
            
        Frontend _Frontend;
        Backend _Backend;
        public Frontend Frontend
        {
            get
            {
                return _Frontend;
            }
            private set
            {
                _Frontend = value;
                if (_Frontend!=null)
                    _Frontend.LogLine += new LogLineHandler(OnLogLine);
            }
        }

        void OnLogLine(ILogSource sender, string message, LogLevel visibility)
        {
            if (LogLine != null)
                LogLine(sender, message, visibility);
        }

        public Backend Backend
        {
            get
            {
                return _Backend;
            }
            private set
            {
                _Backend = value;
                if (_Backend != null)
                    _Backend.LogLine += OnLogLine;
            }
        }
        AudioTarget _audioTarget;

        AudioTarget audioTarget
        {
            set
            {
                _audioTarget = value;
                _audioTarget.AudioFinished += new SimpleNotify(_audioTarget_AudioFinished);
                _audioTarget.LogLine+=new LogLineHandler(OnLogLine);
            }
            get
            {
                return _audioTarget;
            }
        }

        void _audioTarget_AudioFinished()
        {
            _IsSpeaking = false;
            _audioTarget = null;
        }

        public QaryanEngine()
        {
            Frontend = new StandardFrontend();
            //Backend = new MbrolaBackend();
        }

        void Speak(Producer<char> producer, AudioTarget audioTarget) {
            if (audioTarget == null)
                return;
            Frontend.Run(producer);
            Backend.Run(Frontend);
            this.audioTarget = audioTarget;
            audioTarget.Run(Backend);
        }

        public void Speak(string text, AudioTarget audioTarget)
        {
            if (audioTarget == null)
                return;
            _IsSpeaking = true;
            StringCharProducer sp = new StringCharProducer(text);
            Speak(sp, audioTarget);
        }

        void Speak(TextReader text, AudioTarget audioTarget)
        {
            TextReaderCharProducer trcp = new TextReaderCharProducer();
            trcp.Run(text);
            Speak(trcp, audioTarget);
        }

        public void Speak(TextReader text)
        {
            Speak(text, PlatformInstantiator<AudioTarget>.Create(
                typeof(LibaoAudioTarget),
                typeof(WaveOutAudioTarget)));
        }

        public void Speak(string text)
        {
            Speak(text, PlatformInstantiator<AudioTarget>.Create(
                typeof(LibaoAudioTarget),
                typeof(WaveOutAudioTarget)));
        }

        public void SpeakToWavFile(TextReader text, string fileName)
        {
            WaveFileAudioTarget audio = new WaveFileAudioTarget();
            audio.Filename = fileName;
            Speak(text, audio);
        }

        public void SpeakToWavFile(string text, string fileName)
        {
            WaveFileAudioTarget audio = new WaveFileAudioTarget();
            audio.Filename = fileName;
            Speak(text, audio);
        }

        public void SpeakToRawFile(TextReader text, string fileName)
        {
            WaveFileAudioTarget audio = new WaveFileAudioTarget();
            audio.WriteHeader = false;
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

        public void SpeakToWavStream(TextReader text, Stream stream)
        {
            StreamAudioTarget audio = new StreamAudioTarget();
            audio.Stream = stream;
            audio.WriteHeader = true;
            Speak(text, audio);
        }

        public void SpeakToWavStream(string text, Stream stream)
        {
            StreamAudioTarget audio = new StreamAudioTarget();
            audio.Stream = stream;
            audio.WriteHeader = true;
            Speak(text, audio);
        }

        public void SpeakToRawStream(TextReader text, Stream stream)
        {
            StreamAudioTarget audio = new StreamAudioTarget();
            audio.Stream = stream;
            audio.WriteHeader = false;
            Speak(text, audio);
        }

        public void SpeakToRawStream(string text, Stream stream)
        {
            StreamAudioTarget audio = new StreamAudioTarget();
            audio.Stream = stream;
            audio.WriteHeader = false;
            Speak(text, audio);
        }

        public void SpeakToNull(string text)
        {
            Speak(text, new NullAudioTarget());
        }

        public void SpeakToNull(TextReader text)
        {
            Speak(text, new NullAudioTarget());
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

        bool _IsSpeaking;

        public bool IsSpeaking
        {
            get
            {
                return _IsSpeaking;
            }
        }

        #region ILogSource Members

        public string Name
        {
            get {
                return "Qaryan Engine";
            }
        }

        public event LogLineHandler LogLine;

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Xml;
using MotiZilberman;
using Qaryan.Core;
using Qaryan.Audio;
using MBROLA;
using System.IO;

namespace Qaryan.Synths.MBROLA
{
    public class MbrolaBackend : AudioChain<Phone>, Backend
    {
        MbrolaTranslator tra; MBROLASynthesizerBase synth;

        public MbrolaTranslator Translator {
            get {
                return tra;
            }
        }

        public MBROLASynthesizerBase Synthesizer
        {
            get
            {
                return synth;
            }
        }

        protected override void CreateChain(out Consumer<Phone> First, out Producer<AudioBufferInfo> Last)
        {
            First = tra = new MbrolaTranslator();
            Last = synth = PlatformInstantiator<MBROLASynthesizerBase>.Create(
            typeof(MBROLASynthesizer), typeof(MBROLAProcessSynthesizer));
            tra.Voice = synth.Voice = _Voice;
            tra.LogLine += new LogLineHandler(OnLogLine);
            synth.LogLine += new LogLineHandler(OnLogLine);
        }

        void OnLogLine(ILogSource sender, string message, LogLevel visibility)
        {
            if (LogLine != null)
                LogLine(sender, message, visibility);
        }

        public override void Run(Producer<Phone> producer)
        {
            (Voice.BackendVoice as MbrolaVoiceNew).Activate();
            tra.Run(producer);
            synth.Run(tra);
        }


        #region IHasVoice Members

        Voice _Voice;
        public Voice Voice
        {
            get
            {
                return _Voice;
            }
            set
            {
                _Voice = tra.Voice = synth.Voice = value;
            }
        }

        #endregion

        #region ILogSource Members

        public string Name
        {
            get {
                return "Backend (MBROLA)";
            }
        }

        public event LogLineHandler LogLine;

        #endregion
    }

    enum DiphoneOp
    {
        Value, Symbol1, Symbol2
    }

    enum DiphoneAction
    {
        None, Merge, Delete, Insert
    }

    struct DiphoneRuleAction
    {
        public DiphoneAction Action;
        public DiphoneOp Op;
        public string Value;
        public DiphoneRuleAction(DiphoneAction action, DiphoneOp op, string value)
        {
            Action = action;
            Op = op;
            Value = value;
        }
        public DiphoneRuleAction(string action, string op, string value)
            :
            this((DiphoneAction)Enum.Parse(typeof(DiphoneAction), action),
                 (DiphoneOp)Enum.Parse(typeof(DiphoneOp), op), value)
        {
        }
    }

    class DiphoneRule
    {

        DiphoneRuleAction _RuleAction;

        public DiphoneRuleAction RuleAction
        {
            get
            {
                return _RuleAction;
            }
        }

        List<string> Symbol1 = new List<string>(), Symbol2 = new List<string>();
        bool NegSymbol1 = false, NegSymbol2 = false;

        public string Value
        {
            get
            {
                return RuleAction.Value;
            }
        }

        public DiphoneRule(string[] symbol1, string[] symbol2, DiphoneRuleAction action, char negator)
        {
            NegSymbol1 = Array.IndexOf<string>(symbol1, "ALL") > -1;
            NegSymbol2 = Array.IndexOf<string>(symbol2, "ALL") > -1;
            foreach (string s in symbol1)
                if (!(NegSymbol1 ^ (s[0] == negator)))
                {
                    if (s[0] == negator)
                        Symbol1.Add(s.Substring(1));
                    else
                        Symbol1.Add(s);
                }

            foreach (string s in symbol2)
                if (!(NegSymbol2 ^ (s[0] == negator)))
                {
                    if (s[0] == negator)
                        Symbol2.Add(s.Substring(1));
                    else
                        Symbol2.Add(s);
                }
            _RuleAction = action;
        }

        public bool Match(string symbol1, string symbol2)
        {
            return (NegSymbol1 ^ Symbol1.Contains(symbol1)) &&
             (NegSymbol2 ^ Symbol2.Contains(symbol2));
        }
    }

    class MbrolaVoiceNew : BackendVoice
    {
        bool Register()
        {
            if (Mbrola.Binding == MbrolaBinding.Library)
            {
                if (!MbrPlay.DatabaseExist(this.Database)
                    ||
                    !File.Exists(MbrPlay.RegGetDatabasePath(Database)))
                {
                    string localPath = Path.Combine(FileBindings.VoicePath, this.Database);
                    if (File.Exists(localPath))
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder(260);
                        return MbrPlay.RegisterDatabase(this.Database, localPath, this.Database, false, sb, 260);
                    }
                    else
                        return false;
                }
                return true;
            }
            else
                return File.Exists(DatabaseFile);

        }
        internal override bool IsSupported
        {
            get
            {
                return Activate();
            }
        }

        public bool Activate()
        {
            if (Register())
            {
                if (Mbrola.Binding == MbrolaBinding.Library)
                {
                    Mbrola.Init(MbrPlay.RegGetDatabasePath(Database));
                    MbrPlay.SetDatabase(Database);
                    MbrPlay.Play("_ 1\n_ 1\n", (int)MbrFlags.Wait | (int)MbrOut.Disabled, null, null);
                }
                return true;
            }
            else
                return false;
        }

        string _Database;
        float _VolumeRatio, _SpeedRatio;

        Dictionary<string, string[]> PhoneMap;
        List<string> Symbols;
        List<DiphoneRule> DiphoneRules;

        public string Database
        {
            get
            {
                return _Database;
            }
        }

        public float VolumeRatio
        {
            get
            {
                return _VolumeRatio;
            }
        }

        public float SpeedRatio
        {
            get
            {
                return _SpeedRatio;
            }
        }

        void ReadMbrolaInfo(XmlReader reader)
        {
            _VolumeRatio = 1.0f;
            _SpeedRatio = 1.0f;
            _Database = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    reader.MoveToContent();
                    switch (reader.Name)
                    {
                        case "Database": _Database = reader.ReadString();
                            break;
                        case "VolumeRatio": _VolumeRatio = float.Parse(reader.ReadString());
                            break;
                        case "SpeedRatio": _SpeedRatio = float.Parse(reader.ReadString());
                            break;
                    }
                }
            }
            reader.Close();
        }

        char PhoneSeparator = ';', PhoneNegator = '!';

        void ReadPhoneMap(XmlReader reader)
        {
            PhoneMap = new Dictionary<string, string[]>();
            Symbols = new List<string>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Phone":
                            string internalPhone, mbrolaPhones;
                            internalPhone = reader.GetAttribute("Internal").Trim();
                            reader.MoveToContent();
                            mbrolaPhones = reader.ReadString();
                            foreach (string s in PhoneMap[internalPhone] = mbrolaPhones.Split(PhoneSeparator))
                                if (!Symbols.Contains(s))
                                    Symbols.Add(s);
                            break;
                    }
                }
            }
            reader.Close();
        }

        void ReadRuleMap(XmlReader reader)
        {
            DiphoneRules = new List<DiphoneRule>();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "DiphoneRule":
                            string symbol1, symbol2, action, op, value;
                            symbol1 = reader.GetAttribute("Symbol1").Trim();
                            symbol2 = reader.GetAttribute("Symbol2").Trim();
                            action = reader.GetAttribute("Action");
                            op = reader.GetAttribute("Op");
                            if (action == null)
                                action = "Merge";
                            else
                                action = action.Trim();
                            if (op == null)
                                op = "Value";
                            else
                                op = op.Trim();
                            reader.MoveToContent();
                            value = reader.ReadString();
                            DiphoneRules.Add(new DiphoneRule(symbol1.Split(PhoneSeparator), symbol2.Split(PhoneSeparator), new DiphoneRuleAction(action, op, value), PhoneNegator));
                            break;
                    }
                }
            }
            reader.Close();
        }

        internal override void ReadBackendData(XmlReader reader)
        {
            _AudioFormat = null;
            string sep = reader.GetAttribute("Separator");
            if (sep != null)
                PhoneSeparator = sep[0];
            else
                PhoneSeparator = ';';
            string neg = reader.GetAttribute("Negator");
            if (neg != null)
                PhoneNegator = neg[0];
            else
                PhoneNegator = '!';
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "MbrolaInfo": ReadMbrolaInfo(reader.ReadSubtree());
                            break;
                        case "PhoneMap":
                            ReadPhoneMap(reader.ReadSubtree());
                            break;
                        case "RuleMap": ReadRuleMap(reader.ReadSubtree());
                            break;
                    }
                }
            }
            reader.Close();
        }

        internal DiphoneRuleAction MatchRules(MBROLAElement e1, MBROLAElement e2)
        {
            foreach (DiphoneRule rule in DiphoneRules)
                if (rule.Match(e1.Symbol, e2.Symbol))
                    return rule.RuleAction;
            return new DiphoneRuleAction(DiphoneAction.None, DiphoneOp.Value, null);
        }

        internal MBROLAElement[] CreateFragment(Phone p)
        {
            string[] symbols = PhoneMap[p.Symbol];
            MBROLAElement[] result = new MBROLAElement[symbols.Length];
            for (int i = 0; i < symbols.Length; i++)
            {
                string symbol = symbols[i];
                result[i] = new MBROLAElement(symbol, p.Duration / symbols.Length);
            }
            foreach (Qaryan.Core.PitchPoint pp in p.PitchCurve)
            {
                int j = (int)Math.Floor(pp.Time / (p.Duration / symbols.Length));
                result[j].Pitch.Add(new MBROLAPitchPoint(100 * (pp.Time - (j * p.Duration / symbols.Length)) / result[j].Duration, pp.Value));
            }
            return result;
        }
        uint _SampleRate = 0;

        uint SampleRate
        {
            get
            {
                if (_SampleRate == 0)
                {
                    Activate();
                    if (Mbrola.Binding == MbrolaBinding.Library)
                    {
                        _SampleRate = (uint)MbrPlay.GetDefaultFreq();
                    }
                    else
                        _SampleRate = DiphoneDB.WaveFormatFromFile(DatabaseFile).SamplesPerSecond;
                }
                return _SampleRate;
            }
        }

        string _MBROLAVoicePath = System.Environment.GetEnvironmentVariable("MBROLA_DATABASE_DIR") ?? FileBindings.VoicePath;

        public string DatabaseFile
        {
            get
            {
                if (Mbrola.Binding == MbrolaBinding.Library)
                    return MbrPlay.RegGetDatabasePath(Database);
                else
                {
                    string VoiceFilename = Path.Combine(_MBROLAVoicePath, Database);
                    if (Directory.Exists(VoiceFilename))
                        VoiceFilename = Path.Combine(VoiceFilename, Database);
                    return VoiceFilename;
                }
            }
        }

        WaveFormat? _AudioFormat;

        internal override WaveFormat AudioFormat
        {
            get
            {
                WaveFormat result;
                if (_AudioFormat == null)
                {
                    if (Mbrola.Binding == MbrolaBinding.Library)
                    {
                        result = new WaveFormat();
                        result.FormatTag = WaveFormatTag.Pcm;
                        result.BitsPerSample = 16;
                        result.Channels = 1;
                        result.SamplesPerSecond = (uint)MbrPlay.GetDefaultFreq();
                        //result.SamplesPerSecond = 22050;
                        result.AverageBytesPerSecond = result.SamplesPerSecond * result.BitsPerSample / 8;
                        result.BlockAlign = (ushort)(result.Channels * (result.BitsPerSample / 8));
                    }
                    else
                        result = DiphoneDB.WaveFormatFromFile(DatabaseFile);
                    _AudioFormat = result;
                }
                return (WaveFormat)_AudioFormat;
            }
        }
    }
}

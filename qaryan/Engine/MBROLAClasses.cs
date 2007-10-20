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
using Qaryan.Core;
using MBROLA;
using System.IO;
using System.Xml;

namespace Qaryan.Synths.MBROLA
{

    public struct SymbolPair
    {
        public string First, Second;
        public SymbolPair(string first, string second)
        {
            First = first;
            Second = second;
        }
        public override bool Equals(object obj)
        {
            return (obj is SymbolPair) && (((SymbolPair)obj).First == First) && (((SymbolPair)obj).Second == Second);
        }
        public override int GetHashCode()
        {
            return First.GetHashCode() ^ Second.GetHashCode();
        }
    }

    public class MBROLAVoice : IEqualityComparer<SymbolPair>
    {
        public bool Equals(SymbolPair x, SymbolPair y)
        {
            return (x.First == y.First) && (x.Second == y.Second);
        }

        public int GetHashCode(SymbolPair obj)
        {
            return (obj.First + "=>" + obj.Second).GetHashCode();
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            protected set
            {
                name = value;
            }
        }

        private string _MBROLAVoicePath = FileBindings.VoicePath;
        
        internal string MBROLAVoicePath
        {
            get
            {
                return _MBROLAVoicePath;
            }
            set
            {
                _MBROLAVoicePath = value;
            }
        }

        public string FileName
        {
            get
            {
                string VoiceFilename = Path.Combine(MBROLAVoicePath, Name);
                if (Directory.Exists(VoiceFilename))
                    VoiceFilename = Path.Combine(VoiceFilename, Name);
                return VoiceFilename;
            }
        }
        private string displayName;
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            protected set
            {
                displayName = value;
            }
        }

        public uint SampleRate
        {
            get
            {
                if (Mbrola.Binding == MbrolaBinding.Library)
                    return (uint)MbrPlay.GetDefaultFreq();
                else
                    return DiphoneDB.WaveFormatFromFile(FileName).SamplesPerSecond;
            }
        }

        private Dictionary<string, string[]> silprMap;


        private Dictionary<string, string[]> foreignMap;
        private Dictionary<SymbolPair, string> unifyRules;
        private Dictionary<SymbolPair, string> bufferRules;
        private List<string> symbols;
        private Dictionary<string, string> ipaMap;

        public Dictionary<string, string> IpaMap
        {
            get { return ipaMap; }
        }

        public Dictionary<string, string[]> SilprMap
        {
            get { return silprMap; }
        }

        public void LoadFromXml(string filename)
        {
            symbols = new List<string>();
            silprMap = new Dictionary<string, string[]>();
            foreignMap = new Dictionary<string, string[]>();
            ipaMap = new Dictionary<string, string>();
            unifyRules = new Dictionary<SymbolPair, string>(this);
            bufferRules = new Dictionary<SymbolPair, string>(this);
            FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            XmlTextReader r = new XmlTextReader(fs);
            //			r.ReadStartElement("MbrolaVoice");
            r.ReadToFollowing("MbrolaVoice");
            name = r.GetAttribute("Name");
            displayName = r.GetAttribute("DisplayName");
            r.ReadToDescendant("SilprMap");
            if (r.ReadToDescendant("Phone"))
                do
                {
                    string silpr = r.GetAttribute("Silpr");
                    string ipa = r.GetAttribute("Ipa");
                    //				r.MoveToContent();
                    string[] map;
                    if (r.GetAttribute("Origin") == "Foreign")
                        map = foreignMap[silpr] = r.ReadElementContentAsString().Split(':');
                    else
                        map = silprMap[silpr] = r.ReadElementContentAsString().Split(';');
                    if ((map.Length == 1) && (ipa != null))
                        ipaMap[map[0]] = ipa;
                    foreach (string s in map)
                        if (!symbols.Contains(s))
                            symbols.Add(s);
                } while (r.ReadToNextSibling("Phone"));
            r.ReadToNextSibling("UnifyMap");
            if (r.ReadToDescendant("UnifyRule"))
                do
                {
                    string s1 = r.GetAttribute("Symbol1"),
                    s2 = r.GetAttribute("Symbol2");
                    List<string> symbols1, symbols2;
                    symbols1 = new List<string>();
                    symbols2 = new List<string>();
                    foreach (string s in s1.Split(';'))
                    {
                        if (s == "ALL")
                            symbols1.AddRange(symbols);
                        else if (s[0] == '!')
                            symbols1.Remove(s.Substring(1));
                        else
                            symbols1.Add(s);
                    }
                    foreach (string s in s2.Split(';'))
                    {
                        if (s == "ALL")
                            symbols2.AddRange(symbols);
                        else if (s[0] == '!')
                            symbols2.Remove(s.Substring(1));
                        else
                            symbols2.Add(s);
                    }
                    //				r.MoveToContent();
                    string ruleResult = r.ReadElementContentAsString();
                    foreach (string sym1 in symbols1)
                        foreach (string sym2 in symbols2)
                        {
                            unifyRules[new SymbolPair(sym1, sym2)] = ruleResult;
                        }
                    //				unifyRules[new SymbolPair(s1,s2)]=r.ReadElementContentAsString();
                } while (r.ReadToNextSibling("UnifyRule"));
            r.ReadToNextSibling("BufferMap");
            if (r.ReadToDescendant("InsertBufferRule"))
                do
                {
                    string s1 = r.GetAttribute("Symbol1"),
                    s2 = r.GetAttribute("Symbol2");
                    List<string> symbols1, symbols2;
                    symbols1 = new List<string>();
                    symbols2 = new List<string>();
                    foreach (string s in s1.Split(';'))
                    {
                        if (s == "ALL")
                            symbols1.AddRange(symbols);
                        else if (s[0] == '!')
                            symbols1.Remove(s.Substring(1));
                        else
                            symbols1.Add(s);
                    }
                    foreach (string s in s2.Split(';'))
                    {
                        if (s == "ALL")
                            symbols2.AddRange(symbols);
                        else if (s[0] == '!')
                            symbols2.Remove(s.Substring(1));
                        else
                            symbols2.Add(s);
                    }
                    //				r.MoveToContent();
                    string ruleResult = r.ReadElementContentAsString();
                    foreach (string sym1 in symbols1)
                        foreach (string sym2 in symbols2)
                        {
                            bufferRules[new SymbolPair(sym1, sym2)] = ruleResult;
                        }
                    /*string s1=r.GetAttribute("Symbol1"),
                    s2=r.GetAttribute("Symbol2");
    //				r.MoveToContent();
                    bufferRules[new SymbolPair(s1,s2)]=r.ReadElementContentAsString();*/
                } while (r.ReadToNextSibling("InsertBufferRule"));
            r.Close();
            fs.Close();
        }

        public string[] TranslateSymbol(string silprSymbol)
        {
            return TranslateSymbol(silprSymbol, null);
        }

        public string[] TranslateSymbol(string silprSymbol, Word word)
        {
            if (word == null)
                return silprMap[silprSymbol];

            else
            {
                if ((word.Tag | TagTypes.Origin) == TagTypes.Foreign)
                    return foreignMap[silprSymbol];
                else
                    return silprMap[silprSymbol];
            }
        }

        public virtual string TranslatePhoneme(Phoneme p, Word w)
        {
            return null;
        }

        protected virtual string InsertBufferQuery(string s1, string s2)
        {
            return InsertBufferQuery(new SymbolPair(s1, s2));
        }

        protected string InsertBufferQuery(SymbolPair sp)
        {
            if (bufferRules.ContainsKey(sp))
                return bufferRules[sp];
            else
                return null;
        }

        public MBROLAElement InsertBuffer(MBROLAElement e1, MBROLAElement e2)
        {
            string s = InsertBufferQuery(e1.Symbol, e2.Symbol);
            if (s != null)
            {

                return MBROLAElement.CreateBuffer(e1, e2, s);
            }
            else
                return null;
        }

        protected virtual string UnifyQuery(string s1, string s2)
        {
            return UnifyQuery(new SymbolPair(s1, s2));
        }

        protected string UnifyQuery(SymbolPair sp)
        {
            if (unifyRules.ContainsKey(sp))
                return unifyRules[sp];
            else
                return null;
        }

        public MBROLAElement Unify(MBROLAElement e1, MBROLAElement e2)
        {
            string s = UnifyQuery(e1.Symbol, e2.Symbol);
            if (s != null)
            {
                return MBROLAElement.CreateUnify(e1, e2, s);
            }
            else
                return null;
        }

        public MBROLAElement TranslateElement(SpeechElement e, Word w)
        {
            if (e is Phoneme)
                return new MBROLAElement(TranslatePhoneme((Phoneme)e, w), 100, 100);
            else if ((e is Separator) && HebrewChar.IsPunctuation(e.Latin[0]))
                return new MBROLAElement("_", 120);
            else
                return new MBROLAElement("_", 100);
        }

        public bool IsSupported
        {
            get
            {
                return MbrPlay.DatabaseExist(this.name);
            }
        }

        bool Register()
        {
            if (Mbrola.Binding == MbrolaBinding.Library)
            {
                if (!MbrPlay.DatabaseExist(this.name))
                {
                    string localPath = Path.Combine(FileBindings.VoicePath, this.name);
                    if (File.Exists(localPath))
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder(260);
                        return MbrPlay.RegisterDatabase(this.name, localPath, this.name, false, sb, 260);
                    }
                    else
                        return false;
                }
            }
            return true;

        }

        public bool Activate()
        {
            if (Register())
            {
                if (Mbrola.Binding == MbrolaBinding.Library)
                {
                    Mbrola.Init(MbrPlay.RegGetDatabasePath(Name));
                    MbrPlay.SetDatabase(Name);
                    MbrPlay.Play("_ 1\n_ 1\n", (int)MbrFlags.Wait | (int)MbrOut.Disabled, null, null);
                }
                return true;
            }
            else
                return false;
        }
    }

    public class MBROLAPitchPoint
    {
        public double Time, Value;
        public MBROLAPitchPoint(double Time, double Value)
        {
            this.Time = Time;
            this.Value = Value;
        }
    }

    public class MBROLAElement
    {
        public string Symbol;
        public double Duration;
        //		public double Pitch, Pitch2=0;
        public List<MBROLAPitchPoint> Pitch = new List<MBROLAPitchPoint>();
        private string PitchString()
        {
            string result = "";
            foreach (MBROLAPitchPoint p in Pitch)
            {
                result += String.Format(" {0} {1:F3}", p.Time, p.Value);
            }
            return result;
        }

        public void SplitHalf(out MBROLAElement e1, out MBROLAElement e2)
        {
            e1 = new MBROLAElement(Symbol, Duration / 2);
            e2 = new MBROLAElement(Symbol, Duration / 2);
            e1.Pitch.Clear();
            e2.Pitch.Clear();
            foreach (MBROLAPitchPoint pp in Pitch)
            {
                if (pp.Time <= 50)
                    e1.Pitch.Add(new MBROLAPitchPoint(pp.Time * 2, pp.Value));
                else
                    e2.Pitch.Add(new MBROLAPitchPoint(pp.Time * 2, pp.Value));
            }
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}\r\n", Symbol, Duration, PitchString());
        }

        public static MBROLAElement CreateBuffer(MBROLAElement e1, MBROLAElement e2, string symbol/*,bool diphtong*/)
        {
            /*			if (diphtong) {
                a=*/
            double d1 = e1.Duration / (e1.Duration + e2.Duration);
            double d2 = e2.Duration / (e1.Duration + e2.Duration);
            double d;
            if (e2.Symbol == "_")
            {
                d = (e1.Duration) / 4;
                e1.Duration = 5 * d * d1;
            }
            else if (e1.Symbol == "_")
            {

                d = (e2.Duration) / 4;
                e2.Duration = 5 * d * d2;
            }
            else
            {
                d = (e1.Duration + e2.Duration) / 3;
                d = Math.Max(d, 20);
                e1.Duration -= d2 * d;
                e2.Duration -= d1 * d;
                /*e1.Duration=3*d*d1;
                e2.Duration=3*d*d2;*/
            }
            /*			for (int i=0;i<e1.Pitch.Count;i++) {
                PitchPoint p=e1.Pitch[i];
                p.Time*=0.5;
            }
            for (int i=0;i<e2.Pitch.Count;i++) {
                PitchPoint p=e2.Pitch[i];
                p.Time*=0.5;
                p.Time+=50;
            }*/
            return new MBROLAElement(symbol, d);
        }

        public static MBROLAElement CreateUnify(MBROLAElement e1, MBROLAElement e2, string symbol)
        {
            //            HebrewParser.Log.Synthesizer.WriteLine("Unifying: ");
            //            HebrewParser.Log.Synthesizer.WriteLine("\t" + e1.ToString());
            //            HebrewParser.Log.Synthesizer.WriteLine("\t" + e2.ToString());
            for (int i = 0; i < e1.Pitch.Count; i++)
            {
                MBROLAPitchPoint p = e1.Pitch[i];
                p.Time *= 0.5;
            }
            for (int i = 0; i < e2.Pitch.Count; i++)
            {
                MBROLAPitchPoint p = e2.Pitch[i];
                p.Time *= 0.5;
                p.Time += 50;
                e1.Pitch.Add(p);
            }
            MBROLAElement e = new MBROLAElement(symbol, e1.Duration + e2.Duration, e1.Pitch);
            //            HebrewParser.Log.Synthesizer.WriteLine("Result: ");
            //            HebrewParser.Log.Synthesizer.WriteLine("\t" + e.ToString());
            //			e1.Pitch.AddRange(e2.Pitch);
            return e;
        }

        public MBROLAElement(string symbol, double duration, IEnumerable<MBROLAPitchPoint> pitch)
            :
            this(symbol, duration)
        {
            Pitch = new List<MBROLAPitchPoint>(pitch);
        }

        public MBROLAElement(string symbol, double duration, params MBROLAPitchPoint[] pitch)
            :
            this(symbol, duration)
        {
            Pitch = new List<MBROLAPitchPoint>(pitch);
        }

        public MBROLAElement(string symbol, double duration)
        {
            Symbol = symbol;
            Duration = duration;
        }

        public MBROLAElement(string symbol, double duration, double pitch)
            : this(symbol, duration, new MBROLAPitchPoint(16, pitch))
        {

        }

        /*public MBROLAElement(Phone p,MBROLAVoice voice) {
            Duration=p.Duration;
            foreach (Qaryan.Core.PitchPoint pp in p.PitchCurve) {
                Pitch.Add(new Qaryan.Core.MBROLAPitchPoint(100*pp.Time/Duration,pp.Value));
            }
            Symbol=voice.TranslateSymbol(p.Symbol);
        }*/


        public static MBROLAElement[] CreateFragment(Phone p, MBROLAVoice voice)
        {
            string[] symbols = voice.TranslateSymbol(p.Symbol);
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

        public MBROLAElement()
        {

        }
    }

}
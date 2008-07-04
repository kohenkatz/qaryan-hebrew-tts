using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Qaryan.Core;
using Qaryan.Audio;
using Qaryan.Synths.MBROLA;
using System.IO;
using System.Xml;

namespace Qaryan.Synths.Native
{
    public class NativeVoice : IEqualityComparer<SymbolPair>
    {
        uint _sampleRate = 0;

        public uint SampleRate
        {
            get
            {
                return _sampleRate;
            }
        }

        WaveFormat _AudioFormat;

        public WaveFormat AudioFormat
        {
            get
            {
                return _AudioFormat;
            }
        }

        public bool Equals(SymbolPair x, SymbolPair y)
        {
            return (x.First == y.First) && (x.Second == y.Second);
        }

        public int GetHashCode(SymbolPair obj)
        {
            return (obj.First + "=>" + obj.Second).GetHashCode();
        }

        Dictionary<SymbolPair, byte[]> diphones;

        public byte[] GetDiphone(string Phone1, string Phone2)
        {
            SymbolPair sp = new SymbolPair(Phone1, Phone2);
            if (diphones.ContainsKey(sp))
                return diphones[sp];
            else
                return new byte[0];
        }

        public void LoadFromXml(string filename)
        {
            diphones = new Dictionary<SymbolPair, byte[]>(this);
            FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            XmlTextReader r = new XmlTextReader(fs);
            r.ReadToFollowing("Voice");
            //this._sampleRate=int.TryParse(r.GetAttribute("SampleRate"),0);
            this._AudioFormat = new WaveFormat();
            bool first = true;
            if (r.ReadToDescendant("Pair"))
                do
                {
                    string phone1 = r.GetAttribute("Phone1");
                    string phone2 = r.GetAttribute("Phone2");
                    string wavfilename = r.ReadElementContentAsString();
                    wavfilename = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(filename)), wavfilename);
                    WaveFileReader reader = new WaveFileReader(wavfilename);
                    if (first)
                    {
                        _AudioFormat.AverageBytesPerSecond = reader.Contents.format.dwAvgBytesPerSec;
                        _AudioFormat.BitsPerSample = (ushort)reader.Contents.format.dwBitsPerSample;
                        _AudioFormat.BlockAlign = reader.Contents.format.wBlockAlign;
                        _AudioFormat.Channels = reader.Contents.format.wChannels;
                        _AudioFormat.FormatTag = (WaveFormatTag)reader.Contents.format.wFormatTag;
                        _AudioFormat.SamplesPerSecond = reader.Contents.format.dwSamplesPerSec;
                        first = false;
                    }
                    //  if (_sampleRate == 0)
                    //    _sampleRate = reader.Contents.format.dwSamplesPerSec;
                    FileStream raw = File.Open(wavfilename, FileMode.Open, FileAccess.Read);
                    raw.Seek(reader.Contents.data.lFilePosition + reader.Contents.data.dwChunkSize, SeekOrigin.Begin);
                    byte[] buf = new byte[reader.Contents.data.dwNumSamples * reader.Contents.format.dwBitsPerSample / 8];
                    raw.Read(buf, 0, buf.Length);
                    raw.Close();
                    diphones.Add(new SymbolPair(phone1, phone2), buf);
                } while (r.ReadToNextSibling("Pair"));
            r.Close();
        }
    }

    public class NativeSynthesizer : Synthesizer<MBROLA.MBROLAElement>
    {
        NativeVoice _Voice;

        public NativeVoice Voice
        {
            get
            {
                return _Voice;
            }
            set
            {
                _Voice = value;
                this.AudioFormat = _Voice.AudioFormat;
            }
        }

        MBROLAElement LastPhone = null;

        protected override void BeforeConsumption()
        {
            base.BeforeConsumption();
            LastPhone = new MBROLAElement("_", 0);
        }

        protected override void Consume(Queue<MBROLAElement> InQueue)
        {
            MBROLAElement p = InQueue.Dequeue();
            if (LastPhone != null)
            {
                byte[] buf = Voice.GetDiphone(LastPhone.Symbol, p.Symbol);
/*                int[] ibuf = new uint[buf.Length/2];
                Buffer.BlockCopy(buf, 0, ibuf, 0, buf.Length);
                buf = 0;*/
                if (buf.Length > 0)
                    BufferReady(buf);
            }
            LastPhone = p;
        }

        protected override void AfterConsumption()
        {
            base.AfterConsumption();
            LastPhone = new MBROLAElement("_", 0);
            _DoneProducing();
            InvokeAudioFinished();
        }
    }
}
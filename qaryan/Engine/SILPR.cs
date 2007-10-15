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

/*
 * Created by SharpDevelop.
 * User: Moti Z
 * Date: 5/6/2007
 * Time: 4:36 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Qaryan.Core;

namespace Qaryan.Core {
	// Qaryan.Core: Synthesizer-Independent Linear Phonetic Representation
	/*
	public class IPA {
		public static readonly Dictionary<string,string> SymbolTable=new Dictionary<string,string>();
		
		static IPA() {
			SymbolTable[Consonants.Aleph]="ʔ";
			SymbolTable[Consonants.Bet]="b";
			SymbolTable[Consonants.Vet]="v";
			SymbolTable[Consonants.Gimmel]="g";
			SymbolTable[Consonants.Jimmel]="ʤ";
			SymbolTable[Consonants.Dalet]="d";
			SymbolTable[Consonants.Vav]="w";
			SymbolTable[Consonants.Zayin]="z";
			SymbolTable[Consonants.Zhayin]="ʒ";
			SymbolTable[Consonants.Het]="ħ";
			SymbolTable[Consonants.Tet]="t̴";
			SymbolTable[Consonants.Yud]="j";
			SymbolTable[Consonants.Kaf]="k";
			SymbolTable[Consonants.Khaf]="x";
			SymbolTable[Consonants.Lamed]="l";
			SymbolTable[Consonants.Mem]="m";
			SymbolTable[Consonants.Nun]="n";
			SymbolTable[Consonants.Samekh]="s";
			SymbolTable[Consonants.Ayin]="ʕ";
			SymbolTable[Consonants.Pe]="p";
			SymbolTable[Consonants.Fe]="f";
			SymbolTable[Consonants.Tsaddik]="ʦ";
			SymbolTable[Consonants.Tchaddik]="ʧ";
			SymbolTable[Consonants.Quf]="q";
			SymbolTable[Consonants.Resh]="r";
			SymbolTable[Consonants.Shin]=
		}
	}*/
	
	// Time in milliseconds, Value in Hz
	public class PitchPoint {
		public double Time,Value;
		public PitchPoint(double Time, double Value) {
			this.Time=Time;
			this.Value=Value;
		}
	}
	
	public struct PhoneContext {
		public bool IsAccented,IsNucleus;
		public double AccentStrength;
        public string NextSeparator, NextPunctuation;
	}
	
	public class Phone : ICloneable {
		public PhoneContext Context = new PhoneContext();
		public string Symbol;
		public double Duration;
		public int Sonority=0;
		private List<PitchPoint> pitch=new List<PitchPoint>();
		public List<PitchPoint> PitchCurve {
			get {
				return pitch;
			}
		}

		static Phone() {

		}
		public static Phone Create(SpeechElement e) {
			if (e==null)
				return null;
			if ((e is Vowel) && (e as Vowel).IsVowelIn(Vowels.Inaudible))
				return null;
			if (e.Silent)
				return null;
			Phone p=new Phone();
			if (e is Separator) {
				p.Symbol="_";
                if (e.Latin == "׃")
                    p.Duration = 260;
                else if (e.Latin == ",")
                    p.Duration = 60;
                else if (e.Latin == ";")
                    p.Duration = 120;
                else if (e.Latin == ".")
                    p.Duration = 250;
                else if (e.Latin == "?")
                    p.Duration = 250;
                else if (e.Latin == "!")
                    p.Duration = 250;
                else if (e.Latin == "-")
                    p.Duration = 90;
			}
			else {
				p.Symbol=e.Latin;
				if (e is Phoneme)
					p.Sonority=(e as Phoneme).Sonority;
			}
//			HebrewParser.Log.Qaryan.Core.WriteLine("Created phone "+p.Symbol);
			return p;
		}
		public Phone(string symbol,double duration) {
			Symbol=symbol;
			Duration=duration;
		}
		public Phone():this("",0) {
			
		}

		public override string ToString()
		{
			StringBuilder sb=new StringBuilder();
			sb.AppendFormat("{0} {1}",Symbol,
			                Duration);
			foreach (PitchPoint pp in PitchCurve) {
				sb.AppendFormat(" {0} {1}",pp.Time,pp.Value);
			}
			return sb.ToString();
		}
		
		public object Clone()
		{
			Phone p=new Phone(Symbol,Duration);
			p.pitch.AddRange(pitch);
			return p;
		}
	}
	
	public class Silpr {
		private List<Phone> phones=new List<Phone>();
		public List<Phone> Phones {
			get {
				return this.phones;
			}
		}
		public void ReadFromStream(Stream s) {
			StreamReader sr=new StreamReader(s,Encoding.UTF8);
			while (!sr.EndOfStream) {
				try {
					string line=sr.ReadLine();
					string[] parts=line.Trim().Split(' ','\t');
					Phone p=new Phone();
					p.Symbol=parts[0];
					p.Duration=Double.Parse(parts[1]);
					for (int i=2;i<parts.Length;i+=2) {
						p.PitchCurve.Add(new PitchPoint(Double.Parse(parts[2]),Double.Parse(parts[3])));
					}
					Phones.Add(p);
				} catch (IndexOutOfRangeException) {
					
				}

			}
		}
		
		public void WriteToStream(Stream s) {
			StreamWriter sw=new StreamWriter(s,Encoding.UTF8);
			foreach(Phone p in Phones) {
				sw.Write("{0} {1}",p.Symbol,p.Duration);
				foreach (PitchPoint pp in p.PitchCurve) {
					sw.Write(" {0} {1}",pp.Time,pp.Value);
				}
				sw.WriteLine();
			}
			sw.Flush();
		}
		
		public void WritePitchDataToStream(Stream s) {
			double t=0;
			double pitch=0;
			StreamWriter sw=new StreamWriter(s,Encoding.UTF8);
			sw.WriteLine("# Time\tPitch");
			foreach(Phone p in Phones) {
				bool first=true;
				if (p.Symbol!="_") {
					foreach (PitchPoint pp in p.PitchCurve) {
						if (t+pp.Time>t+p.Duration)
							break;
						sw.Write("{0}\t{1}",t+pp.Time,pitch=pp.Value);
						if (first) {
							sw.WriteLine("\t{0}",p.Symbol);
							first=false;
						}
						else
							sw.WriteLine("\t ");
					}
					if (first)
						sw.WriteLine("{0}\t{1}\t{2}",t,pitch,p.Symbol);
				}
				else
					sw.WriteLine();
				t+=p.Duration;
			}
			sw.WriteLine("{0}\t{1}",t,pitch);
			sw.Flush();
		}
		
		public string AsString {
			get {
				System.IO.MemoryStream ms=new MemoryStream();
				WriteToStream(ms);
				ms.Seek(0, SeekOrigin.Begin);
				return Encoding.UTF8.GetString(ms.ToArray());
			}
			set {
				System.IO.MemoryStream ms=new MemoryStream();
				byte[] b=Encoding.UTF8.GetBytes(value);
				ms.Write(b,0,b.Length);
				ms.Seek(0, SeekOrigin.Begin);
				ReadFromStream(ms);
			}
		}
	}
}

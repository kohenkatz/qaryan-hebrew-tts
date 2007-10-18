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
 * Date: 8/24/2007
 * Time: 10:15 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MotiZilberman;
using System.Xml;
using Qaryan.Core.Properties;

namespace Qaryan.Core
{
	public class Segment: IEnumerable<SpeechElement> {
		protected List<SpeechElement> Elements;
		
		protected Segment() {
			Elements=new List<SpeechElement>();
		}
		
		public SpeechElement this[int index] {
			get {
				return Elements[index];
			}
			set {
				Elements[index]=value;
			}
		}
		
		public void Add(SpeechElement Element) {
			Elements.Add(Element);
		}
		
		public IEnumerator<SpeechElement> GetEnumerator()
		{
			return Elements.GetEnumerator();
		}
		
		//[System.Runtime.InteropServices.DispIdAttribute()]
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Elements.GetEnumerator();
		}
	}
	
	/// <summary>
	/// Description of Segmenter.
	/// </summary>
	public class Segmenter: LookaheadConsumerProducer<SpeechElement,Segment>
	{
		Syllable curSyl;
		Segment curSegment;
		Word curWord;
		int curElementIndex=-1;
		
		List<SyllablePattern> StressHeuristics;
		
		bool relaxAudibleSchwa=true;

        string stressHeuristicsFile;

        Word.Stress defaultStress=Word.Stress.Milra;

        public Word.Stress DefaultStress
        {
            get { return defaultStress; }
            set { defaultStress = value; }
        }

		public bool RelaxAudibleSchwa {
			get {return relaxAudibleSchwa;}
			set {relaxAudibleSchwa=value;}
		}

        public void LoadStressHeuristics(string Filename)
        {
            StressHeuristics = LoadStressHeuristicsFromXml(Filename);
        }


        static object EnumParseFlags(Type enumType, string value)
        {
            if (value.IndexOf("|") < 0)
                return Enum.Parse(enumType, value);
            switch (enumType.Name)
            {
                case "Vowels":
                    Vowels result1 = (Vowels)0;
                    foreach (string s in value.Split('|'))
                    {
                        result1 |= (Vowels)Enum.Parse(enumType, s);
                    }
                    return result1;
                case "SyllableCoda":
                    SyllableCoda result2 = (SyllableCoda)0;
                    foreach (string s in value.Split('|'))
                    {
                        result2 |= (SyllableCoda)Enum.Parse(enumType, s);
                    }
                    return result2;
                case "SyllableOnset":
                    SyllableOnset result3 = (SyllableOnset)0;
                    foreach (string s in value.Split('|'))
                    {
                        result3 |= (SyllableOnset)Enum.Parse(enumType, s);
                    }
                    return result3;
                case "SyllableDisposition":
                    SyllableDisposition result4 = (SyllableDisposition)0;
                    foreach (string s in value.Split('|'))
                    {
                        result4 |= (SyllableDisposition)Enum.Parse(enumType, s);
                    }
                    return result4;
                case "SyllablePatternAnchor":
                    SyllablePatternAnchor result5 = (SyllablePatternAnchor)0;
                    foreach (string s in value.Split('|'))
                    {
                        result5 |= (SyllablePatternAnchor)Enum.Parse(enumType, s);
                    }
                    return result5;
            }
            return null;
        }
	
        static List<SyllablePattern> LoadStressHeuristicsFromXml(string Filename)
        {
            List<SyllablePattern> StressHeuristics = new List<SyllablePattern>();
//            Log.Analyzer.WriteLine("Loading stress heuristics from XML....");
            FileStream fs = File.Open(Filename, FileMode.Open);
            //			XmlReaderSettings xrs=new XmlReaderSettings();
            XmlTextReader r = new XmlTextReader(fs);
            r.ReadStartElement("StressHeuristics");
            while (r.ReadToFollowing("Pattern"))
            {
                SyllablePattern p;
                string result = r.GetAttribute("Result"),
                comment = r.GetAttribute("Description"),
                anchor = r.GetAttribute("Anchor");
                if (result == null)
                    result = "Milra";
                if (anchor == null)
                    anchor = "End";
                p = new SyllablePattern((Word.Stress)Enum.Parse(typeof(Word.Stress), result),
                                      (SyllablePatternAnchor)EnumParseFlags(typeof(SyllablePatternAnchor), anchor));
                if (comment != null)
                    p.Comment = comment;
                //				Log.Analyzer.WriteLine("--> Begin pattern");
                if (r.ReadToDescendant("Syllable"))
                    do
                    {
                        string nucleus = r.GetAttribute("Nucleus"),
                        coda = r.GetAttribute("Coda"),
                        onset = r.GetAttribute("Onset"),
                        start = r.GetAttribute("Start"),
                        end = r.GetAttribute("End"),
                        disposition = r.GetAttribute("Disposition");
                        if (nucleus == null)
                            nucleus = "Any";
                        if (coda == null)
                            coda = "Any";
                        if (onset == null)
                            onset = "Any";
                        if (start == null)
                            start = "";
                        if (end == null)
                            end = "";
                        if (disposition == null)
                            disposition = "Required";
                        SyllableMask m = new SyllableMask((Vowels)EnumParseFlags(typeof(Vowels), nucleus),
                                                        (SyllableCoda)EnumParseFlags(typeof(SyllableCoda), coda),
                                                        (SyllableOnset)EnumParseFlags(typeof(SyllableOnset), onset),
                                                        (SyllableDisposition)EnumParseFlags(typeof(SyllableDisposition), disposition)
                                                       );
                        m.StartsWith = start;
                        m.EndsWith = end;
                        p.Syllables.Add(m);
                        //					Log.Analyzer.WriteLine("Added mask "+m.ToString());
                        //					r.ReadEndElement();
                    } while (r.ReadToNextSibling("Syllable"));
                //r.ReadEndElement();
                StressHeuristics.Add(p);
                //Log.Analyzer.WriteLine("Loaded pattern "+p.ToString());
            }
            //			r.ReadEndElement();
//            Log.Analyzer.WriteLine(StressHeuristics.Count + " patterns loaded successfully.");
            r.Close();
            fs.Close();
            return StressHeuristics;
        }
	

		protected override void BeforeConsumption()
		{
			Console.WriteLine("segmenter started");			
			base.BeforeConsumption();
			curElementIndex=-1;
			curSegment=curWord=null;
			curSyl=null;
			LoadStressHeuristics(stressHeuristicsFile);
		}
		
		void AddAndProcessWord(Word w) {
			if ((curSyl!=null) && !w.Syllables.Contains(curSyl)) {
				w.Syllables.Add(curSyl);
				curSyl=null;
			}
			Console.WriteLine("segmenter: /{0}/",w.TranslitSyllables);
			w.PlaceStress(StressHeuristics,DefaultStress);
			foreach (Syllable syl in w.Syllables) {
				bool stressed=syl.IsStressed;
				for (int i=0;i<syl.Phonemes.Count;i++) {
					SpeechElement e=syl.Phonemes[i];
					if (e is Vowel) {
						Vowel v=(Vowel)e;
						if (v.vowel==Vowels.KamatzIndeterminate) {
							if ((syl.Coda== SyllableCoda.Closed) && (syl.Phonemes[syl.Phonemes.Count-1] is Consonant) /*&& ((w.Tag&TagTypes.Origin)!=TagTypes.Foreign)*/ && !stressed)
								v.vowel=Vowels.KamatzKatan;
							else
								v.vowel=Vowels.KamatzGadol;
//							Log.Analyzer.WriteLine("Kamatz determined to be "+v.vowel.ToString());
						}
						else if (v.vowel==Vowels.AudibleSchwa) {
							if ((w.Tag&TagTypes.Origin)==TagTypes.Foreign)
								v.vowel= Vowels.SilentSchwa;
							else {
								int j=w.Phonemes.IndexOf(e);
								if ((j+1<w.Phonemes.Count)
								    && (w.Phonemes[j+1] is Consonant)
								    && (j-1>=0)
								    && (w.Phonemes[j-1] is Consonant)
								    && (w.Phonemes[j+1].Latin!=w.Phonemes[j-1].Latin) ) {
									switch (w.Phonemes[j-1].Latin) {
											/*											case "k":
											case "l":
											case "b":
											case "m":
												break;*/
										case Consonants.Vav:
                                        case Consonants.Lamed:
											break;
										default:
											if (!relaxAudibleSchwa)
												break;
											int son=((Consonant)w.Phonemes[j+1]).Sonority -
												((Consonant)w.Phonemes[j-1]).Sonority;
											if (son>=0)
												v.Silent=true;
											break;
									}
									
								}
							}
						}
					}
				}
			}
			Emit(w);
		}
		
		protected override void Consume(Queue<SpeechElement> InQueue)
		{
			SpeechElement curElement=InQueue.Dequeue();
            _ItemConsumed(curElement);
			Console.WriteLine("segmenter: Current element is a {0} ({1})",curElement.GetType(),curElement.Latin);
			SpeechElement nextElement=null;
			if (InQueue.Count>0)
				nextElement=InQueue.Peek();
			if (curSegment==null) {
				if ((curElement is Phoneme) || (curElement is WordTag))
					curSegment=new Word();
				else
					curSegment=new SeparatorSegment();
				curElementIndex=-1;
			}
			if (curSegment is Word) {
				curWord=(curSegment as Word);
				if (curElement is Phoneme) {
					curWord.Add(curElement);
					curElementIndex++;
					if (curSyl==null) {
						curSyl=new Syllable(curWord);
						curSyl.Start=curElementIndex;
						curSyl.End=curElementIndex;
					}
					else if (curSyl.Nucleus==null) {
						curSyl.End++;
						if ((curElement is Vowel) &&
						    (curElement as Vowel).IsVowelIn(Vowels.LegalNuclei))
							curSyl.Nucleus=curElement as Vowel;
					}
					else if (curElement is Consonant) {
						if ((nextElement!=null) && (nextElement is Vowel) && (nextElement as Vowel).IsVowelIn(Vowels.Inaudible)) {
							curSyl.End++;
							nextElement=InQueue.Dequeue();
							Console.WriteLine("segmenter: About to chomp a {0} ({1})",nextElement.GetType(),nextElement.Latin);
						}
						else if ((nextElement==null) || (nextElement is Separator)) {
							curSyl.End++;
//							curWord.Syllables.Add(curSyl);
//							curSyl=null;
						}
						else {
							
							if (((curElement as Consonant).Flags&ConsonantFlags.StrongDagesh)!=0) {
								curSyl.End++;
								curSyl.HintStrongDagesh=true;
							}
							curWord.Syllables.Add(curSyl);
							curSyl=new Syllable(curWord);
							curSyl.Start=curElementIndex;
							curSyl.End=curElementIndex;
						}
					}
					else {
						// non-nucleic vowel, move on
						curSyl.End++;
					}
					
				}
				else if (curElement is WordTag)
					curWord.Tag=(curElement as WordTag).Tag;
				else if (curElement is Qaryan.Core.Cantillation)
					curWord.CantillationMarks.Add((curElement as Cantillation).Mark);
				else if (curElement is Separator) {
					AddAndProcessWord(curSegment as Word);
					curSegment=null;
				}
			}
			if (curElement is Separator) {
				if (curSegment!=null) {
					if (curSegment is SeparatorSegment) {
						curSegment.Add(curElement);
					}
					else {
						if (curSegment is Word)
							AddAndProcessWord(curSegment as Word);
						else
							Emit(curSegment);
						curSegment=new SeparatorSegment(curElement as Separator);
					}
					Emit(curSegment);
					Console.WriteLine("segmenter: Added separator segment");
					curSegment=null;
				}
				else {
					Emit(curSegment=new SeparatorSegment(curElement as Separator));
					Console.WriteLine("segmenter: Added separator segment");
					curSegment=null;
				}
			}
		}
		
		protected override void AfterConsumption()
		{
			base.AfterConsumption();
			if (curSegment!=null) {
				if (curSegment is Word)
					AddAndProcessWord(curSegment as Word);
				else
					Emit(curSegment);
				curSegment=null;
			}
            _DoneProducing();
			Console.WriteLine("segmenter finished");
		}
		
		public override void Run(Producer<SpeechElement> producer)
		{
			base.Run(producer,2);
		}
		
		public Segmenter()
		{
            this.stressHeuristicsFile = FileBindings.StressHeuristicsPath;
            
		}
	}
}

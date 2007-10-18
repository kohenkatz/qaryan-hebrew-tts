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
 * Time: 5:13 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using MotiZilberman;

namespace Qaryan.Core
{
	/// <summary>
	/// Description of Parser.
	/// </summary>
	public class Parser: LookaheadConsumerProducer<Token,SpeechElement>
	{
		const int windowSize=4;
		Token t=null;
		SpeechElement newElement=null;
		WordTag lastTag=null;
		char v;
		LetterToken prev=null;
		bool isFirstWindow=true;
		bool isLastWindow=false;
		Vowel prevVowel=null;
		Consonant prevConsonant=null;
		SpeechElement prevElement=null;
		
		public struct ContextOptions {
			public bool EverydayRegister;
		}
		
		public ContextOptions Options=new ContextOptions();
		
		void AddElement(SpeechElement eElement) {
			Console.WriteLine("parser: Current element is a {0} ({1})",eElement.GetType(),(eElement is Vowel)?(eElement as Vowel).vowel.ToString():eElement.Latin);
			this.Emit(eElement);
			prevElement=eElement;
			prevVowel=prevElement as Vowel;
			prevConsonant=prevElement as Consonant;
		}
		
		protected override void BeforeConsumption()
		{
			base.BeforeConsumption();
			Console.WriteLine("parser started");
			t=null;
			newElement=null;
			lastTag=null;
			prev=null;
			isFirstWindow=true;
			isLastWindow=false;
			prevVowel=null;
			prevConsonant=null;
			prevElement=null;
		}
		
		protected override void Consume(Queue<Token> InQueue)
		{
			//Token[] tokens=queue.ToArray();
			List<Token> tokens=new List<Token>(InQueue);
			int tokensToConsume=tokens.Count;
			if (tokensToConsume>windowSize)
				tokensToConsume-=windowSize;
			else
				isLastWindow=true;
			for (int z=0;z<tokensToConsume;z++)
				InQueue.Dequeue();
			for (int i=0;i<tokensToConsume;) {
				newElement=null;
				t=tokens[i];
                _ItemConsumed(t);
				if (!(t is LetterToken)) {
					if (t is TagToken) {
						newElement=new WordTag(((TagToken)t).Type);
						prev=null;
						i++;
					}
					else if (t is CantillationToken) {
						newElement=new Cantillation((t as CantillationToken).Value[0]);
						i++;
					}
					else {
						newElement=new Separator(t.Value);
						prev=null;
						i++;
					}
				}
				else {
					LetterToken next=null;
					LetterToken further=null;
					int nextIndex=-1,furtherIndex=-1;
					int j;
					/*j=i-1;
				while ((j>0)&&!(tokens[j] is LetterToken)) {
					if (!(tokens[j] is CantillationToken))
						break;
					j--;
				}
				if (j>=0) {
					prev=tokens[j] as LetterToken;
					prevIndex=j;
				}*/
					j=i+1;
					while ((j<tokens.Count-1)&&!(tokens[j] is LetterToken)) {
						if (!(tokens[j] is CantillationToken))
							break;
						j++;
					}
					if (j<tokens.Count) {
						next=tokens[j] as LetterToken;
						nextIndex=j;
					}
					j++;
					while ((j<tokens.Count-1)&&!(tokens[j] is LetterToken)) {
						if (!(tokens[j] is CantillationToken))
							break;
						j++;
					}
					if (j<tokens.Count) {
						further=tokens[j] as LetterToken;
						furtherIndex=j;
					}
					bool curIsWordEnd=(isLastWindow && (i==tokens.Count-1)) ||
						(next==null);
					bool curIsWordStart=(isFirstWindow && (i==0))||(prev==null);
					// Look for a consonant
					LetterToken l=(LetterToken)t;
					switch (l.Letter) {
						case 'א':
							newElement=new Consonant(Consonants.Aleph);
							break;
						case 'ב':
							if (l.HasDagesh())
								newElement=new Consonant(Consonants.Bet);
							else
								newElement=new Consonant(Consonants.Vet);
							break;
						case 'ג':
							if (l.HasApostrophe()) {
								newElement=new Consonant(Consonants.Jimmel);
							}
							else
								newElement=new Consonant(Consonants.Gimmel);
							break;
						case 'ד':
							newElement=new Consonant(Consonants.Dalet);
							break;
						case 'ה':
							newElement=new Consonant(Consonants.He);
							break;
						case 'ו':
							if ((l.HasDagesh()&&l.HasAnyVowels()) || l.HasAnyVowelsExcept('\u05B9'))
								newElement=new Consonant(Consonants.Vav);
							else if ((next!=null) && (next.Letter=='ו') && (next.HasAnyModifier('\u05B9',HebrewChar.Shuruk)))
								newElement=new Consonant(Consonants.Vav);
							else {
								v=l.FirstVowel;
								switch(v) {
									case '\u05B9':
										newElement=new Vowel(Vowels.HolamMale);
										break;
									case '\0':
										if (l.HasShuruk()) {
											if (curIsWordStart) {
												AddElement(newElement=new Consonant(Consonants.Aleph));
//												Log.Parser.WriteLine("Added consonant "+newElement.Latin+" (sonority "+((Consonant)newElement).Sonority+")");
											}
											newElement=new Vowel(Vowels.Shuruk);
										}
										else
											newElement=new Consonant(Consonants.Vav);
										break;
								}
							}
							if ((newElement is Consonant)&&(lastTag!=null)) {
								if ((lastTag.Tag&TagTypes.Origin)==TagTypes.Foreign)
									newElement=new Consonant(Consonants.W);
							}
							break;
						case 'ז':
							if (l.HasApostrophe())
								newElement=new Consonant(Consonants.Zhayin);
							else
								newElement=new Consonant(Consonants.Zayin);
							break;
						case 'ח':
							if (l.HasApostrophe())
								newElement=new Consonant(Consonants.Khaf);
							else
								newElement=new Consonant(Consonants.Het);
							break;
						case 'י':
							newElement=new Consonant(Consonants.Yud);
							break;
						case 'ט':
							newElement=new Consonant(Consonants.Tet);
							break;
						case 'כ':
						case 'ך':
							if (l.HasDagesh())
								newElement=new Consonant(Consonants.Kaf);
							else
								newElement=new Consonant(Consonants.Khaf);
							break;
						case 'ל':
							newElement=new Consonant(Consonants.Lamed);
							break;
						case 'מ':
						case 'ם':
							newElement=new Consonant(Consonants.Mem);
							break;
						case 'נ':
						case 'ן':
							newElement=new Consonant(Consonants.Nun);
							break;
						case 'ס':
							newElement=new Consonant(Consonants.Samekh);
							break;
						case 'ע':
							newElement=new Consonant(Consonants.Ayin);
							break;
						case 'פ':
						case 'ף':
							if (l.HasDagesh())
								newElement=new Consonant(Consonants.Pe);
							else
								newElement=new Consonant(Consonants.Fe);
							break;
						case 'צ':
						case 'ץ':
							if (l.HasApostrophe())
								newElement=new Consonant(Consonants.Tchaddik);
							else
								newElement=new Consonant(Consonants.Tsaddik);
							break;
						case 'ק':
							newElement=new Consonant(Consonants.Quf);
							break;
						case 'ר':
							newElement=new Consonant(Consonants.Resh);
							break;
						case 'ש':
							if (l.HasModifier(HebrewChar.SinDot))
								newElement=new Consonant(Consonants.Sin);
							else
								newElement=new Consonant(Consonants.Shin);
							break;
						case 'ת':
							newElement=new Consonant(Consonants.Tav);
							break;
					}
					if (newElement is Consonant) {
						TagTypes wordOrigin= TagTypes.Unrecognized;
						if (lastTag!=null) {
							wordOrigin=lastTag.Tag&TagTypes.Origin;
						}
						
						/*if ((i-1>=0)&&(tokens[i-1] is LetterToken))
							prev=(LetterToken)tokens[i-1];
						if ((i+1<tokensToConsume)&&(tokens[i+1] is LetterToken))
							next=(LetterToken)tokens[i+1];*/

						/*if ((parsed.Count>0) && (parsed[parsed.Count-1] is Vowel))
							prevVowel=(Vowel)(parsed[parsed.Count-1]);
						else if ((parsed.Count>0) && (parsed[parsed.Count-1] is Consonant))
							prevConsonant=(Consonant)(parsed[parsed.Count-1]);*/
						/*if (prev!=null) {
							prevVowel=parsed[parsed.Count-(i-prevIndex)] as Vowel;
							prevConsonant=parsed[parsed.Count-(i-prevIndex)] as Consonant;
						}*/
						Consonant curConsonant=(Consonant)newElement;
						if (l.HasDagesh()) {
							if (wordOrigin== TagTypes.Foreign) {
								if (!HebrewChar.IsBegedKefet(l.Letter))
									curConsonant.Flags|=ConsonantFlags.LightDagesh;
							}
							else if(!HebrewChar.IsGuttural(l.Letter) && l.Letter!='י') {
								if (!HebrewChar.IsBegedKefet(l.Letter)) {
									curConsonant.Flags|=ConsonantFlags.StrongDagesh;
								}
								else {
									if ((prev==null)||(prevVowel==null)||(prevVowel.vowel==Vowels.SilentSchwa))
										curConsonant.Flags|=ConsonantFlags.LightDagesh;
									else
										curConsonant.Flags|=ConsonantFlags.StrongDagesh;
								}
							}
						}
						v=l.FirstVowel;
						
						bool patahGnuva=false;
						
						if (curIsWordEnd /*&& (v=='\u05B7')*/
						    && ((curConsonant.Latin==Consonants.Het)||(curConsonant.Latin==Consonants.Ayin)))
							if ((prevVowel!=null) && prevVowel.IsVowelIn(Vowels.E|Vowels.I|Vowels.U|Vowels.O))
							if ((v=='\u05B7')||(v=='\0')) {
							AddElement(new Vowel(Vowels.PatahGnuva));
							patahGnuva=true;
						}
						
						if (Options.EverydayRegister) {
							if ((newElement.Latin==Consonants.Ayin)||(newElement.Latin==Consonants.Aleph)||(newElement.Latin==Consonants.He))
								newElement.Silent=true;
						}
						AddElement(newElement);
						
//						Log.Parser.WriteLine("Added consonant "+curConsonant.Latin+" (sonority "+curConsonant.Sonority+")");
						newElement=null;
						
						
						bool nextIsUnvoicedEhevi = (next!=null)
							&& HebrewChar.IsEhevi(next.Letter)
							&& !next.HasAnyVowels()
							&& !next.HasMappiq();
						
						if (nextIsUnvoicedEhevi) {
							if (next.Letter=='ו') {
								nextIsUnvoicedEhevi&=!l.HasAnyModifier('\u05B7','\u05B8');
							}
							if (further!=null) {
								if (further.Letter=='ו') {
									nextIsUnvoicedEhevi&=!further.HasAnyModifier('\u05B9' /* holam */,HebrewChar.Shuruk);
									nextIsUnvoicedEhevi&=!further.HasAnyVowelsExcept(HebrewChar.Shuruk);
								}
								
							}
							else if (next.Letter=='י')
								nextIsUnvoicedEhevi&=(l.HasModifier('\u05B4'));
						}
						bool nextHasHatafKamatz = (next!=null)
							&& next.HasModifier('\u05B3');
						bool nextHasSchwa = (next!=null)
							&& next.HasModifier('\u05B0');
						bool nextHasHataf = (next!=null)
							&& next.HasAnyModifier('\u05B1','\u05B2','\u05B3');
						bool nextIsBegedKefet = (next!=null)
							&& HebrewChar.IsBegedKefet(next.Letter);
						/*						if (nextIsUnvoicedEhevi)
							Log.Parser.WriteLine("Next token is an extender אהו\"י");*/
						i++;
						switch(v) {
							case '\u05B0':
                                 if (wordOrigin == TagTypes.Foreign)
									newElement=new Vowel(Vowels.SilentSchwa);
                                else if (prev == null)
                                    newElement = new Vowel(Vowels.AudibleSchwa);
								else  if (next == null)
                                    newElement = new Vowel(Vowels.SilentSchwa);
                                else if (nextHasSchwa|nextHasHataf)
									newElement=new Vowel(Vowels.SilentSchwa);
								else if (nextIsBegedKefet) {
									if (next.HasDagesh())
										newElement=new Vowel(Vowels.SilentSchwa);
									else
										newElement=new Vowel(Vowels.AudibleSchwa);
								}
								/*								else if (((curConsonant.Latin)==Consonants.Aleph) ||
								         ((curConsonant.Latin)==Consonants.Ayin) ||
								         ((curConsonant.Latin)==Consonants.Het) ||
								         ((curConsonant.Latin)==Consonants.He) ||
								         ((curConsonant.Latin)==Consonants.Resh))
									newElement=new Vowel(Vowels.AudibleSchwa);*/
								else if (prevVowel!=null) {
									switch(prevVowel.vowel) {
										case Vowels.SilentSchwa:
											newElement=new Vowel(Vowels.AudibleSchwa);
											break;
										default:
											if (prevVowel.IsVowelIn(Vowels.Short))
												newElement=new Vowel(Vowels.SilentSchwa);
											else if ((curConsonant.Flags&ConsonantFlags.StrongDagesh)!=0)
												newElement=new Vowel(Vowels.AudibleSchwa);
											//				else if (prevVowel.IsVowelIn(Vowels.Long|Vowels.VeryLong))
											//					newElement=new Vowel(Vowels.AudibleSchwa);
											else
												newElement=new Vowel(Vowels.SilentSchwa);
											break;
									}
								}
                                else if ((curConsonant.Flags & ConsonantFlags.StrongDagesh) != 0)
                                    newElement = new Vowel(Vowels.AudibleSchwa);
                                else
                                    newElement = new Vowel(Vowels.SilentSchwa);
								break;
							case '\u05B1':
								newElement=new Vowel(Vowels.HatafSegol);
								break;
							case '\u05B2':
								newElement=new Vowel(Vowels.HatafPatah);
								break;
							case '\u05B3':
								newElement=new Vowel(Vowels.HatafKamatz);
								break;
							case '\u05B4':
								if (nextIsUnvoicedEhevi)
									newElement=new Vowel(Vowels.HirikMale);
								else
									newElement=new Vowel(Vowels.HirikHaser);
								break;
							case '\u05B5':
								if (nextIsUnvoicedEhevi)
									newElement=new Vowel(Vowels.TzereMale);
								else
									newElement=new Vowel(Vowels.Tzere);
								break;
							case '\u05B6':
								if (nextIsUnvoicedEhevi)
									newElement=new Vowel(Vowels.SegolMale);
								else
									newElement=new Vowel(Vowels.Segol);
								break;
							case '\u05B7':
								if (!patahGnuva) {
									if (nextIsUnvoicedEhevi)
										newElement=new Vowel(Vowels.PatahMale);
									else
										newElement=new Vowel(Vowels.Patah);
								}
								break;
							case '\u05B8':
								if (nextIsUnvoicedEhevi)
									newElement=new Vowel(Vowels.KamatzMale);
								else if (nextHasHatafKamatz)
									newElement=new Vowel(Vowels.KamatzKatan);
								else
									newElement=new Vowel(Vowels.KamatzIndeterminate);
								break;
							case '\u05B9':
								if (nextIsUnvoicedEhevi)
									newElement=new Vowel(Vowels.HolamMale);
								else
									newElement=new Vowel(Vowels.HolamHaser);
								break;
							case '\u05BB':
								newElement=new Vowel(Vowels.Kubutz);
								break;
							default:
//								if (v!=(char)0)
//									Log.Parser.WriteLine("Unknown vowel char: {0:X4}",(int)v);
								break;
						}
						prev=l;
						if (newElement!=null) {
							/*if (curIsWordEnd && (((Vowel)newElement).vowel==Vowels.Patah)
							    && ((curConsonant.Latin==Consonants.Het)||(curConsonant.Latin==Consonants.Ayin)||(curConsonant.Latin==Consonants.He))) {
								((Vowel)newElement).vowel=Vowels.PatahGnuva;
								parsed.Insert(parsed.Count-1,newElement);
								Log.Parser.WriteLine("Added element "+((Vowel)newElement).vowel+" as patah gnuva");
							}
							else {*/
							AddElement(newElement);
//								Log.Parser.WriteLine("Added element "+((Vowel)newElement).vowel);
							//}
							newElement=null;
							if (nextIsUnvoicedEhevi) {
								tokens.GetRange(i,nextIndex-i).ForEach(delegate(Token tk) {
								                                       	if (tk is CantillationToken) {
								                                       		newElement=new Cantillation((tk as CantillationToken).Value[0]);
								                                       		AddElement(newElement);
//								                                       		Log.Parser.WriteLine("Added element "+newElement.Latin+" ("+newElement.GetType().Name+") while skipping unvoiced ehevi");
								                                       		newElement=null;
								                                       	}
								                                       }
								                                      );
								
								for (int z=0;z<nextIndex+1-tokensToConsume;z++)
                                    _ItemConsumed(InQueue.Dequeue());
								i=nextIndex+1;
							}
						}
						/*else if (i<tokensToConsume) {
							t=tokens[i];
							if (t is LetterToken) {
								l=(LetterToken)t;
								if (l.Letter=='ו') {
									
									if (newElement!=null) {
										AddElement(newElement);
										newElement=null;
										i++;
									}
								}
							}
						}*/
					} else// if (newElement!=null)
						i++;
				}
				if (newElement!=null) {
					AddElement(newElement);
					if (newElement is WordTag) {
//						Log.Parser.WriteLine("Added tag "+((WordTag)newElement).Tag);
						lastTag=(WordTag)newElement;
					}
					else {
						if (newElement is Separator)
							lastTag=null;
//						Log.Parser.WriteLine("Added element "+newElement.Latin+" ("+newElement.GetType().Name+")");
					}
					newElement=null;
				}
				
			}
			if (isFirstWindow)
				isFirstWindow=false;
		}

		protected override void AfterConsumption()
		{
			base.AfterConsumption();
            _DoneProducing();
			Console.WriteLine("parser finished");
		}
		
		public override void Run(Producer<Token> producer)
		{
			base.Run(producer,windowSize);
		}
	}
}

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
 * Time: 2:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Text;

using MotiZilberman;

namespace Qaryan.Core
{
	public class Token {
		public string Value;
		public Token(string Value) {
			this.Value=Value;
		}
	}

	public class TagToken : Token {
		
		
		public TagToken(string value) : base (value) {
		}
		
		public TagTypes Type {
			get {
				TagTypes result=TagTypes.Unrecognized;
				if (Value.Length<1)
					return result;
				int i;
				if (Value[0]=='ל') {
					result|= TagTypes.Foreign|TagTypes.User;
					i=1;
				}
				else
					i=0;
				switch(Value.Substring(i)) {
					case "ס": // smikhut, no stress
						result|=TagTypes.Unstressed|TagTypes.User;
						break;
					case "ר": // milra
						result|=TagTypes.Milra|TagTypes.User;
						break;
					case "ע": // milel
						result|=TagTypes.Milel|TagTypes.User;
						break;
					case "עע": // milel demilel
						result|=TagTypes.MilelDMilel|TagTypes.User;
						break;
					default:
						result|=TagTypes.Unrecognized;
						break;
				}
				return result;
			}
		}
	}
	
	public class PunctuationToken:Token {
		public PunctuationToken(string Value):base(Value){

		}
	}
	
	public class CantillationToken:Token {
		public CantillationToken(string Value):base(Value){

		}
	}
	
	public class LetterToken : Token {
		public char Letter;
		public string Modifiers;
		public bool HasModifier(char c) {
			return Modifiers.IndexOf(c)>-1;
		}
		public bool HasDagesh() {
			return HasModifier(HebrewChar.Dagesh);
		}
		
		public bool HasShuruk() {
			return (Letter=='ו')&&HasModifier(HebrewChar.Shuruk);
		}
		
		public bool HasMappiq() {
			return (Letter=='ה')&&HasModifier(HebrewChar.Mappiq);
		}
		
		public bool HasApostrophe() {
			return HasModifier('\'') || HasModifier('`') || HasModifier('´');
		}
		
		public bool HasAnyModifier(params char[] c) {
			return Modifiers.IndexOfAny(c)>-1;
		}
		
		public bool HasAnyVowels() {
			return HasAnyModifier(HebrewChar.Vowels);
		}
		
		public bool HasAnyVowelsExcept(params char[] v) {
			StringBuilder m=new StringBuilder(Modifiers);
			foreach(char c in v)
				m.Replace(c.ToString(),"");
			return m.ToString().IndexOfAny(HebrewChar.Vowels)>-1;
		}
		
		public char FirstVowel {
			get {
				if (HasAnyVowels())
					return Modifiers[Modifiers.IndexOfAny(new char[11] {'\u05B0','\u05B1','\u05B2','\u05B3','\u05B4','\u05B5','\u05B6','\u05B7','\u05B8','\u05B9','\u05BB'})];
				else
					return (char)0;
			}
		}
		
		public LetterToken(char letter,string modifiers) : base(letter+modifiers) {
			
			Letter=letter;
			Modifiers=modifiers;
		}
	}

	public enum TokenState {Neutral,Letter,Tag};
	
	/// <summary>
	/// Description of Tokenizer.
	/// </summary>
	public class Tokenizer: LookaheadConsumerProducer<char,Token>
	{
		TokenState state;
		StringBuilder sb;
		char letter;
		
		protected override void BeforeConsumption()
		{
			Console.WriteLine("tokenizer started");
			base.BeforeConsumption();
			state=TokenState.Neutral;
			sb=new StringBuilder();
			letter=(char)0;
		}
		
		protected override void Consume(Queue<char> InQueue)
		{
			char c=InQueue.Dequeue();
            _ItemConsumed(c);
//			Console.WriteLine("Tokenizing input...");
			if (state==TokenState.Letter) {
				if (HebrewChar.IsModifier(c)) {
					sb.Append(c);
					return;
				}
				else {
					LetterToken lt=new LetterToken(letter,sb.ToString());
					this.Emit(lt);
					Console.WriteLine("tokenizer: Ate letter "+lt.Value);
					sb.Length=0;
					state=TokenState.Neutral;
				}
			}
			
			switch(state) {
				case TokenState.Tag:
					if (c=='/') {
						state=TokenState.Neutral;
						TagToken tt=new TagToken(sb.ToString());
						this.Emit(tt);
						Console.WriteLine("tokenizer: Ate tag "+tt.Type);
						sb.Length=0;
					}
					else
						sb.Append(c);
					break;
				case TokenState.Neutral:
					if (HebrewChar.IsLetter(c)||(c=='/')) {
						if (sb.Length>0) {
							Token t=new Token(sb.ToString());
							this.Emit(t);
							Console.WriteLine("tokenizer: Ate neutral token "+t.Value);
						}
						if (c=='/')
							state=TokenState.Tag;
						else {
							letter=c;
							state=TokenState.Letter;
						}
						sb.Length=0;
					}
					else if (HebrewChar.IsCantillation(c)) {
						CantillationToken ct=new CantillationToken(c.ToString());
						this.Emit(ct);
						Console.WriteLine("tokenizer: Ate cantillation mark "+ct.Value);
					}
					else if (HebrewChar.IsPunctuation(c)) {
						PunctuationToken pt=new PunctuationToken(c.ToString());
						this.Emit(pt);
						Console.WriteLine("tokenizer: Ate punctuation "+pt.Value);
					}
					else
						sb.Append(c);
					break;
			}
		}
		
		protected override void AfterConsumption()
		{
			base.AfterConsumption();
			switch(state) {
				case TokenState.Neutral:
					if (sb.Length>0) {
						Token t=new Token(sb.ToString());
						this.Emit(new Token(sb.ToString()));
						Console.WriteLine("tokenizer: Ate neutral token "+t.Value);
					}
					break;
				case TokenState.Letter:
					LetterToken lt=new LetterToken(letter,sb.ToString());
					this.Emit(lt);
					Console.WriteLine("tokenizer: Ate letter "+lt.Value);
					break;
			}
            _DoneProducing();
			Console.WriteLine("tokenizer finished");
		}

		public void Run(string s) {
			Console.WriteLine("------ creating StringCharProducer ------");
			StringCharProducer producer=new StringCharProducer(s);
			Run(producer,1);
		}
	}
}

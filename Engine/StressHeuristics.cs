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
 * Date: 4/8/2007
 * Time: 3:39 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using MBROLA;

namespace Qaryan.Core {
	class WordTag : MetaElement {
		TagTypes tag;
		public TagTypes Tag {
			get {
				return tag;
			}
		}
		
		public WordTag(TagTypes tag):base("/"){
			this.tag=tag;
		}
	}

	[Flags]
	public enum SyllableCoda {
		Open=1,
		Closed=2,
		Any=Open|Closed
	}

	[Flags]
	public enum SyllableOnset {
		Simple=1,
		Compound=2,
		Any=Simple|Compound
	}

	public enum SyllableDisposition {
		Required=0,Optional=1
	}

    public struct SyllableMask
    {
		public Vowels AllowedVowels;
		public SyllableCoda AllowedCodas;
		public SyllableOnset AllowedOnsets;
		public SyllableDisposition Disposition;
		public string StartsWith,EndsWith;
		public SyllableMask(Vowels vowels,SyllableCoda codas,SyllableOnset onsets,SyllableDisposition disposition) {
			AllowedVowels=vowels;
			AllowedCodas=codas;
			AllowedOnsets=onsets;
			Disposition=disposition;
			StartsWith="";
			EndsWith="";
		}
		public SyllableMask(Vowels vowels,SyllableCoda codas,SyllableOnset onsets):this(vowels,codas,onsets,SyllableDisposition.Required) {
			
		}
		public SyllableMask(Vowels vowels,SyllableCoda codas) : this (vowels,codas,SyllableOnset.Any) {
		}
		public SyllableMask(Vowels vowels,SyllableOnset onsets) : this (vowels,SyllableCoda.Any,onsets) {
		}
		public bool Match(Syllable syl) {
			return syl.Match(this);
		}
		public override string ToString() {
			string s="-";
			if (StartsWith.Length>0)
				s=StartsWith+"-";
			if (EndsWith.Length>0)
				s+=EndsWith;
			if (s=="-")
				s="";
			else
				s=";"+s;
			if (Disposition== SyllableDisposition.Optional)
				return String.Format("?{0};{1};{2}{3}? ",AllowedVowels,AllowedCodas,AllowedOnsets,s);
			else
				return String.Format("<{0};{1};{2}{3}> ",AllowedVowels,AllowedCodas,AllowedOnsets,s);
		}
	}

	[Flags]
	public enum SyllablePatternAnchor {
		Start=1,End=2,Both=4,
		Whole=Both
	}
	
	public struct SyllablePattern {
		public SyllablePatternAnchor Anchor;
		public Word.Stress Result;
		public List<SyllableMask> Syllables;
		public string Comment;
		public SyllablePattern(Word.Stress result,SyllablePatternAnchor anchor,params SyllableMask[] syllables) {
			Result=result;
			Syllables=new List<SyllableMask>(syllables);
			Comment=null;
			Anchor=anchor;
		}
		public SyllablePattern(Word.Stress result,SyllablePatternAnchor anchor) {
			Result=result;
			Syllables=new List<SyllableMask>();
			Comment=null;
			Anchor=anchor;
		}
		public override string ToString() {
			StringBuilder result=new StringBuilder("");
			if (Comment!=null) {
				result.Append(Comment);
				result.Append(": ");
			}
			if (Anchor==SyllablePatternAnchor.End)
				result.Append("<=");
			foreach (SyllableMask m in Syllables)
				result.Append(String.Format("{0}",m));
			if (Anchor==SyllablePatternAnchor.Start)
				result.Append("=>");
			return result.ToString();
		}
		
		public bool Match(Word w) {
			int i,j;
			bool result=false,match;
			switch (Anchor){
				case SyllablePatternAnchor.Start:
				case SyllablePatternAnchor.Both:
					for (i=0,j=0;j<Syllables.Count;j++) {
						match=true;
						if (i<w.Syllables.Count)
							match=Syllables[j].Match(w.Syllables[i]);
						if (Syllables[j].Disposition==SyllableDisposition.Optional)
							match=true;
						else
							i++;
						result=match;
						if (!result)
							return false;
					}
					if (result&&(Anchor==SyllablePatternAnchor.Both))
						result=(i==w.Syllables.Count);
					break;
				case SyllablePatternAnchor.End:
					for (i=w.Syllables.Count-1,j=Syllables.Count-1;j>=0;j--) {
						match=false;
						if (i>=0)
							match=Syllables[j].Match(w.Syllables[i]);
						match=match||(Syllables[j].Disposition==SyllableDisposition.Optional);
						if (match)
							i--;
						result=match;
						if (!result)
							return false;
					}
					break;
			}
//			result=false;
			return result;
		}
	}
}

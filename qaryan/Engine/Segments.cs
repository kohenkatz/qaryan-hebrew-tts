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
using System.Text;

namespace Qaryan.Core
{

    public class Word : Segment
    {
        TagTypes tag;
        public TagTypes Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        List<char> cantillationMarks;

        public List<char> CantillationMarks
        {
            get
            {
                if (cantillationMarks == null)
                    cantillationMarks = new List<char>();
                return cantillationMarks;
            }
        }

        public enum Stress
        {
            None = -1, Milra = 1, Milel = 2, MilelDMilel = 3
        }
        public Stress StressPosition;

        public List<SpeechElement> Phonemes
        {
            get
            {
                return Elements;
            }
            set
            {
                Elements = value;
            }
        }
        public List<Syllable> Syllables = new List<Syllable>();

        public string Translit
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (SpeechElement e in Phonemes)
                {
                    result.Append(e.Translit);
                }
                return result.ToString();
            }
        }

        public string TranslitSyllables
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (Syllable syl in Syllables)
                {
                    foreach (SpeechElement e in syl.Phonemes)
                    {
                        result.Append(e.Latin);
                    }
                    result.Append("-");
                }
                if (result.Length > 0)
                    if (result[result.Length - 1] == '-')
                        result.Remove(result.Length - 1, 1);
                return result.ToString();
            }
        }

        public string TranslitSyllablesStress
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (Syllable syl in Syllables)
                {
                    foreach (SpeechElement e in syl.Phonemes)
                    {
                        if (syl.IsStressed)
                            result.Append(e.Latin.ToUpper());
                        else
                            result.Append(e.Latin);
                    }
                    result.Append("-");
                }
                if (result.Length > 0)
                    if (result[result.Length - 1] == '-')
                        result.Remove(result.Length - 1, 1);
                return result.ToString();
            }
        }

        public SyllablePattern? PlaceStress(List<SyllablePattern> StressHeuristics, Word.Stress DefaultStress)
        {
            /*            HebrewParser.Log.Analyzer.Write("/" + TranslitSyllables + "/");
                        if (Tag != TagTypes.Unrecognized)
                            HebrewParser.Log.Analyzer.WriteLine(" (tagged " + Tag.ToString() + ")");*/
            if (CantillationMarks.Count > 0)
            {
                StressPosition = Stress.None;
                for (int i = Syllables.Count - 1; (i >= 0) && (i > Syllables.Count - 3); i--)
                {
                    if (Syllables[i].CantillationMarks.Count > 0)
                    {
                        StressPosition = (Stress)(Syllables.Count - i);
                        if (!(Syllables[i].CantillationMarks.Contains(HebrewChar.Pashta) && (CantillationMarks.Count > 1)))
                            break;
                    }
                }
                //(" set to " + StressPosition.ToString() + " by cantillation mark");
            }
            else if ((Tag & TagTypes.Stress) != TagTypes.Unrecognized)
            {
                switch (Tag & TagTypes.Stress)
                {
                    case TagTypes.Unstressed:
                        StressPosition = Stress.None;
                        break;
                    case TagTypes.Milra:
                        StressPosition = Stress.Milra;
                        break;
                    case TagTypes.Milel:
                        StressPosition = Stress.Milel;
                        break;
                    case TagTypes.MilelDMilel:
                        StressPosition = Stress.MilelDMilel;
                        break;
                }
            }
            else
            {
                foreach (SyllablePattern p in StressHeuristics)
                {
                    if (p.Match(this))
                    {
                        StressPosition = p.Result;
                        return p;
                    }
                }
                StressPosition = DefaultStress;
                //                HebrewParser.Log.Analyzer.WriteLine(" defaulted to " + StressPosition.ToString());
            }
            if ((int)StressPosition > Syllables.Count)
            {
                StressPosition = (Stress)Syllables.Count;
                //                HebrewParser.Log.Analyzer.WriteLine("\t^--- Falling back to " + StressPosition.ToString());
            }
            return null;
        }

        public Word()
        {

        }

        public Word(List<SpeechElement> Elements)
        {
            Phonemes = Elements;
            List<SpeechElement> elems = new List<SpeechElement>(Phonemes);
            while ((elems.Count > 0) && (elems[elems.Count - 1] is Separator))
                elems.RemoveAt(elems.Count - 1);
            List<SpeechElement> nuclei = elems.FindAll(
                delegate(SpeechElement e)
                {
                    return (e is Vowel) && (!((Vowel)e).IsVowelIn(Vowels.VeryShort | Vowels.Inaudible | Vowels.PatahGnuva));
                }
            );
            Syllable syl;
            if (nuclei.Count > 0)
            {
                foreach (SpeechElement nucleus in nuclei)
                {
                    int i = elems.IndexOf(nucleus);
                    syl = new Syllable(this, (Vowel)nucleus, i, i);
                    for (int j = i + 1; j < elems.Count; j++)
                    {
                        if (elems[j] is Consonant)
                        {
                            if (((((Consonant)elems[j]).Flags & ConsonantFlags.StrongDagesh) != 0) ||
                                (j + 1 == elems.Count) ||
                                ((elems[j + 1] is Vowel) &&
                                 ((Vowel)elems[j + 1]).IsVowelIn(Vowels.Inaudible | Vowels.PatahGnuva)))
                            {
                                syl.HintStrongDagesh = (((Consonant)elems[j]).Flags & ConsonantFlags.StrongDagesh) != 0;
                                syl.End = j;
                            }
                            else
                                break;

                        }
                        else if ((elems[j] is Vowel) && (((Vowel)elems[j]).IsVowelIn(Vowels.Inaudible | Vowels.PatahGnuva)))
                            syl.End = j;
                        else
                            break;
                    }
                    Syllables.Add(syl);
                }
                syl = null;
                for (int i = 0; i < Syllables.Count; i++)
                {
                    syl = Syllables[i];
                    if (i == 0)
                        syl.Start = 0;
                    else if (Syllables[i - 1].HintStrongDagesh)
                        syl.Start = Syllables[i - 1].End;
                    else
                        syl.Start = Syllables[i - 1].End + 1;
                }
                if (syl != null)
                    if (syl.End < elems.Count - 1)
                        syl.End = elems.Count - 1;
                List<SpeechElement> cant = elems.FindAll(delegate(SpeechElement e)
                {
                    return (e is Cantillation);
                }
                                                      );
                foreach (Cantillation c in cant)
                {
                    int ci = Phonemes.IndexOf(c);
                    int si = Syllables.FindLastIndex(delegate(Syllable s)
                    {
                        return (s.Start < ci);
                    }
                                                  );
                    Syllables[si].CantillationMarks.Add(c.Mark);
                    this.CantillationMarks.Add(c.Mark);
                    Phonemes.RemoveAt(ci);
                    Syllables.ForEach(delegate(Syllable s)
                    {
                        if (s.Start > ci)
                            s.Start--;
                        if (s.End >= ci)
                            s.End--;
                    });

                }
            }
        }
    }

    public class SeparatorSegment : Segment
    {
        public SeparatorSegment()
        {

        }

        public SeparatorSegment(Separator s)
        {
            Elements.Add(s);
        }
    }

    public class Clause
    {
        public List<Word> Words = new List<Word>();
        public SpeechElement TerminatingPunctuation = null;

        public double BasePitchAt(double elem)
        {
            double x = Math.PI * (elem) / Words.Count;
            double y = Math.Cos(0.5 * x - 0.5);
            return y * 35 + 80;
        }

        public double BaseDeviationAt(double elem)
        {
            double x = Math.PI * (elem) / Words.Count;
            //			x-=0.5;
            double y = Math.Sin(x);
            return y * 20;
        }
    }

    public class Syllable
    {
        public Word Parent;
        public int Start, End;
        public Vowel Nucleus = null;
        public bool HintStrongDagesh = false;

        List<char> cantillationMarks;

        public List<char> CantillationMarks
        {
            get
            {
                if (cantillationMarks == null)
                    cantillationMarks = new List<char>();
                return cantillationMarks;
            }
        }

        public Syllable(Word parent)
        {
            Parent = parent;
        }

        public Syllable(Word parent, Vowel nucleus, int start, int end)
        {
            Parent = parent;
            Nucleus = nucleus;
            Start = start;
            End = end;
        }

        public SyllableCoda Coda
        {
            get
            {
                SyllableCoda result;
                if ((Parent.Phonemes[End] is Vowel) && !((Vowel)Parent.Phonemes[End]).IsVowelIn(Vowels.Inaudible))
                    result = SyllableCoda.Open;
                else
                    result = SyllableCoda.Closed;
                return result;
            }
        }

        public SyllableOnset Onset
        {
            get
            {
                if (Phonemes.IndexOf(Nucleus) > 1)
                    return SyllableOnset.Compound;
                else
                    return SyllableOnset.Simple;
            }
        }

        public bool IsStressed
        {
            get
            {
                int s = (int)Parent.StressPosition;
                int index = Parent.Syllables.Count - Parent.Syllables.IndexOf(this);
                return (index == s);
            }
        }

        public string Translit
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (SpeechElement e in Phonemes)
                {
                    result.Append(e.Translit);
                }
                return result.ToString();
            }
        }

        public bool Match(SyllableMask pattern)
        {
            bool match = true;
            if (pattern.StartsWith.Length > 0)
            {
                match = match && Translit.StartsWith(pattern.StartsWith);
            }
            if (pattern.EndsWith.Length > 0)
            {
                match = match && Translit.EndsWith(pattern.EndsWith);
            }
            return match && ((Onset & pattern.AllowedOnsets) == Onset) && ((Coda & pattern.AllowedCodas) == Coda) && (Nucleus != null) && (Nucleus.IsVowelIn(pattern.AllowedVowels));
        }

        public List<SpeechElement> Phonemes
        {
            get
            {
                return Parent.Phonemes.GetRange(Start, End - Start + 1);
            }
        }

    }

    public class Utterance
    {
        private List<Clause> clauses;
        public List<Clause> Clauses
        {
            get
            {
                return clauses;
            }
            private set
            {
                clauses = value;
            }
        }
        public Utterance(List<Clause> clauses)
        {
            Clauses = clauses;
        }
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (Clause c in Clauses)
            {
                foreach (Word w in c.Words)
                {
                    buf.Append(" ");
                    foreach (Syllable syl in w.Syllables)
                    {
                        bool stressed = syl.IsStressed;
                        foreach (SpeechElement e in w.Phonemes.GetRange(syl.Start, syl.End - syl.Start + 1))
                        {
                            if (e.Silent)
                                buf.Append("(");
                            if (stressed)
                                buf.Append(e.Latin.ToUpper());
                            else
                                buf.Append(e.Latin);
                            if (e.Silent)
                                buf.Append(")");
                        }
                        if (w.Syllables.IndexOf(syl) + 1 < w.Syllables.Count)
                            buf.Append("‧");
                    }

                }
                if (c.TerminatingPunctuation != null)
                    buf.Append(c.TerminatingPunctuation.Translit);
            }
            return buf.ToString();
        }
    }

}
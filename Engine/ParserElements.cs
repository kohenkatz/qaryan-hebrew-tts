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

namespace Qaryan.Core
{

    public abstract class SpeechElement
    {
        protected string latin;
        public bool Silent = false;

        public virtual string Latin
        {
            get
            {
                //throw new NotImplementedException();
                return latin;
            }
            protected set
            {
                latin = value;
            }
        }

        public virtual string Translit
        {
            get
            {
                return Latin;
            }
        }
    }

    public abstract class Phoneme : SpeechElement
    {
        public abstract int Sonority
        {
            get;
        }
    }

    public class Vowel : Phoneme
    {
        public override int Sonority
        {
            get
            {
                if (IsVowelIn(Vowels.HighVowels))
                    return 7;
                else
                    return 8;
            }
        }
        public Vowels vowel;

        public string VowelToLatinSimple()
        {
            if (IsVowelIn(Vowels.A))
                return "a";
            if (IsVowelIn(Vowels.E))
                return "e";
            if (IsVowelIn(Vowels.I))
                return "i";
            if (IsVowelIn(Vowels.O))
                return "o";
            if (IsVowelIn(Vowels.U))
                return "u";
            if (IsVowelIn(Vowels.Inaudible))
                return "";
            //if (vowel==Vowels.AudibleSchwa)
            //	return "@";
            return "#" + vowel.ToString() + "#";
        }

        private string VowelToLatin()
        {
            string s = VowelToLatinSimple();
            if (IsVowelIn(Vowels.VeryShort))
                return s + "̆";
            if (IsVowelIn(Vowels.Tzere | Vowels.TzereMale))
                return s + "́";
            return s;
        }

        public Vowel(Vowels v)
        {
            vowel = v;

        }

        public override string Latin
        {
            get { return VowelToLatin(); }
        }

        public bool IsVowelIn(Vowels VowelSet)
        {
            return (vowel & VowelSet) == vowel;
        }

    }

    [Flags]
    public enum ConsonantArticulation
    {
        Plosive = 1,
        Fricative = 2,
        Voiced = 4,
        Nasal = 8 | Voiced,
        Liquid = 16 | Voiced,
        Indeterminate = 0
    }

    public class Consonant : Phoneme
    {
        public ConsonantArticulation Articulation
        {
            get
            {
                switch (Latin)
                {
                    case Consonants.Aleph:
                    case Consonants.Tet:
                    case Consonants.Tav:
                    case Consonants.Quf:
                    case Consonants.Kaf:
                    case Consonants.Pe:
                        return ConsonantArticulation.Plosive;
                    case Consonants.Bet:
                    case Consonants.Gimmel:
                    case Consonants.Dalet:
                        if ((flags & ConsonantFlags.LightDagesh) != 0)
                            return ConsonantArticulation.Plosive | ConsonantArticulation.Voiced;
                        else
                            return ConsonantArticulation.Fricative | ConsonantArticulation.Voiced;
                    case Consonants.Ayin:
                    case Consonants.Samekh:
                    case Consonants.Shin:
                    case Consonants.Sin:
                    case Consonants.He:
                    case Consonants.Het:
                    case Consonants.Khaf:
                    case Consonants.Fe:
                        return ConsonantArticulation.Fricative;
                    case Consonants.Zayin:
                    case Consonants.Vav:
                    case Consonants.Vet:
                    //affricates:
                    case Consonants.Zhayin:
                    case Consonants.Jimmel:
                    case Consonants.Tsaddik:
                    case Consonants.Tchaddik:
                    case Consonants.Resh:
                        return ConsonantArticulation.Fricative | ConsonantArticulation.Voiced;
                    case Consonants.Mem:
                    case Consonants.Nun:
                        return ConsonantArticulation.Nasal;
                    case Consonants.Yud:
                    case Consonants.Lamed:
                        //					case Consonants.Resh:
                        return ConsonantArticulation.Liquid;
                    default:
                        return ConsonantArticulation.Indeterminate;
                }

            }
        }

        protected ConsonantFlags flags;

        public ConsonantFlags Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        public override string Latin
        {
            get
            {
                return latin;
            }
        }

        public override string Translit
        {
            get
            {
                string s;
                if ((Flags & ConsonantFlags.StrongDagesh) != 0)
                    s = latin + latin;
                else
                    s = latin;
                return s;
            }
        }

        public Consonant(string latin)
        {
            this.Latin = latin;
        }

        public bool IsBegedKefet
        {
            get
            {
                return (Latin == Consonants.Bet) || (Latin == Consonants.Vet) || (Latin == Consonants.Gimmel) || (Latin == Consonants.Dalet) || (Latin == Consonants.Kaf) || (Latin == Consonants.Khaf) || (Latin == Consonants.Pe) || (Latin == Consonants.Fe) || (Latin == Consonants.Tav);
            }
        }

        public bool IsLiquid
        {
            get
            {
                return (Latin == Consonants.Vav) ||
                    (Latin == Consonants.Zhayin) ||
                    (Latin == Consonants.Zayin) ||
                    (Latin == Consonants.Het) ||
                    (Latin == Consonants.Yud) ||
                    (Latin == Consonants.Khaf) ||
                    (Latin == Consonants.Lamed) ||
                    (Latin == Consonants.Mem) ||
                    (Latin == Consonants.Nun) ||
                    (Latin == Consonants.Samekh) ||
                    (Latin == Consonants.Fe) ||
                    (Latin == Consonants.Resh) ||
                    (Latin == Consonants.Shin) ||
                    (Latin == Consonants.Sin);
            }
        }

        public override int Sonority
        {
            get
            {
                switch (Articulation)
                {
                    case ConsonantArticulation.Plosive:
                        return 2;
                    case ConsonantArticulation.Fricative:
                        return 1;
                    case ConsonantArticulation.Plosive | ConsonantArticulation.Voiced:
                        return 3;
                    case ConsonantArticulation.Fricative | ConsonantArticulation.Voiced:
                        return 4;
                    case ConsonantArticulation.Nasal:
                        return 5;
                    case ConsonantArticulation.Liquid:
                        return 6;
                    case ConsonantArticulation.Indeterminate:
                    default:
                        return 0;
                }
            }
        }
    }

    public class Separator : SpeechElement
    {
        public Separator(string symbol)
        {
            this.Latin = symbol;
        }
    }

    public class MetaElement : SpeechElement
    {
        public MetaElement(string symbol)
        {
            this.Latin = symbol;
        }
    }

    public class Cantillation : SpeechElement
    {
        char mark;
        public char Mark
        {
            get
            {
                return mark;
            }
        }

        public Cantillation(char mark)
            : base()
        {
            this.mark = mark;
            this.latin = "*";
        }
    }
	
}
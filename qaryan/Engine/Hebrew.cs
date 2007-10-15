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
 * User: Moti Zilberman
 * Date: 4/5/2007
 * Time: 8:32 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Qaryan.Core
{
	[Flags]
    public enum TagTypes
    {
		Unrecognized=0,Unstressed=1,Milra=2,Milel=4,MilelDMilel=8,Foreign=16,Reserved=32,
		User=512,Inferred=1024,
		Stress=Unstressed|Milra|Milel|MilelDMilel,
		Origin=Foreign|Reserved,
		Certainty=User|Inferred
	}
	
	[Flags]
    public enum ConsonantFlags
    {
		LightDagesh=1,StrongDagesh=2,Mapik=4
	}
	
	[Flags]
	public enum Vowels {
		None=0,
		KamatzGadol=1,Tzere=2,TzereMale=4,HirikMale=8,HolamMale=16,PatahMale=32,SegolMale=64,KamatzMale=128,HolamHaser=256,Shuruk=512,
		Patah=1024,Segol=2048,HirikHaser=4096,KamatzKatan=8192,Kubutz=16384,
		HatafPatah=32768,HatafKamatz=65536,HatafSegol=131072,
		SilentSchwa=262144,AudibleSchwa=524288,
		PatahGnuva=1048576,
		KamatzIndeterminate=2097152,
		Any=4194303,
		
		VeryLong=TzereMale|HirikMale|HolamMale|Shuruk|KamatzMale|PatahMale|SegolMale,
		Long=KamatzIndeterminate|KamatzGadol|Tzere|HolamHaser,
		Short=Patah|PatahGnuva|Segol|HirikHaser|KamatzKatan|Kubutz,
		VeryShort=HatafKamatz|HatafPatah|HatafSegol|AudibleSchwa,
		Inaudible=SilentSchwa|None,
		A=KamatzIndeterminate|KamatzGadol|PatahMale|KamatzMale|Patah|HatafPatah|PatahGnuva,
		E=SegolMale|Tzere|TzereMale|Segol|HatafSegol|AudibleSchwa,
		I=HirikMale|HirikHaser,
		O=HolamMale|HolamHaser|KamatzKatan|HatafKamatz,
		U=Shuruk|Kubutz,
		HighVowels=I|U,
		LowVowels=A|E,
		
		AnyKamatz=KamatzIndeterminate|KamatzGadol|KamatzKatan|KamatzMale,
		AnyKamatzA=KamatzIndeterminate|KamatzGadol|KamatzMale,
		AnyPatah=Patah|PatahGnuva|PatahMale,
		AnyTzere=Tzere|TzereMale,
		AnySegol=Segol|SegolMale,
		AnyHolam=HolamHaser|HolamMale,
		AnyHirik=HirikMale|HirikHaser,
		
		LegalNuclei=Any & ~(VeryShort|Inaudible|PatahGnuva)
			/*,
		None*/
	}

	public sealed class Consonants {
		public const string Aleph="’";
		public const string Bet="b";
		public const string Vet="v";
		public const string Gimmel="g";
		public const string Jimmel="ğ";
		public const string Dalet="d";
		//public const string Dhalet=
		public const string He="h";
		public const string Vav="w";
		public const string Zayin="z";
		public const string Zhayin="ž";
		public const string Het="ḥ";
		public const string Tet="ṭ";
		public const string Yud="y";
		public const string Khaf="ḳ";
		public const string Kaf="k";
		public const string Lamed="l";
		public const string Mem="m";
		public const string Nun="n";
		public const string Samekh="s";
		public const string Ayin="‘";
		public const string Pe="p";
		public const string Fe="f";
		public const string Tchaddik="č";
		public const string Tsaddik="ẕ";
		public const string Quf="q";
		public const string Resh="r";
		public const string Sin="ś";
		public const string Shin="š";
		public const string Tav="t";
		public const string W="W";
	}
	
    public sealed class HebrewChar {
		public static readonly Dictionary<string,string> Expansions = new Dictionary<string,string>();
		
		static HebrewChar() {
			Expansions["א"]="אָלֶף";
			Expansions["ב"]="בֵּית";
			Expansions["ג"]="/ע/גִּמֶל";
			Expansions["ד"]="/ע/דָּלֶת";
			Expansions["ה"]="הֵי";
			Expansions["ו"]="וַו";
			Expansions["ז"]="זַיִן";
			Expansions["ח"]="חֵית";
			Expansions["ט"]="טֵית";
			Expansions["י"]="יוֹד";
			Expansions["כ"]="כַּף";
			Expansions["ך"]="כַף סוֹפִית";
			Expansions["ל"]="/ע/לָמֶּד";
			Expansions["מ"]="מֶם";
			Expansions["ם"]="מֶם סוֹפִית";
			Expansions["נ"]="נוּן";
			Expansions["ן"]="נוּן סוֹפִית";
			Expansions["ס"]="סָמֶך";
			Expansions["ע"]="עַיִן";
			Expansions["פ"]="פֶּי";
			Expansions["ף"]="פֶי סוֹפִית";
			Expansions["צ"]="/ע/צַדִיק";
			Expansions["ץ"]="/ע/צַדִיק סוֹפִית";
			Expansions["ק"]="קוֹף";
			Expansions["ר"]="רֵיש";
			Expansions["ש"]="שִין";
			Expansions["ת"]="תַיו";
			
			Expansions["A"]="אֵי";
			Expansions["B"]="בִּי";
			Expansions["C"]="סִי";
			Expansions["D"]="דִּי";
			Expansions["E"]="אִי";
			Expansions["F"]="אֶף";
			Expansions["G"]="גִּ'י";
			Expansions["H"]="אֵיְץ'";
			Expansions["I"]="אַי";
			Expansions["J"]="גֵּ'י";
			Expansions["K"]="קֵי";
			Expansions["L"]="אֶַל";
			Expansions["M"]="אֶם";
			Expansions["N"]="אֶן";
			Expansions["O"]="/ל/אוֹו";
			Expansions["P"]="פִּי";
			Expansions["Q"]="/ל/קְיוּ";
			Expansions["R"]="אָר";
			Expansions["S"]="אֶס";
			Expansions["T"]="טִי";
			Expansions["U"]="יוּ";
			Expansions["V"]="וִי";
			Expansions["W"]="/לעע/דַאבֶּליוּ";
			Expansions["X"]="אִקס";
			Expansions["Y"]="/ל/וַי";
			Expansions["Z"]="זִי";
		}
		
		public static bool IsNikud(char c) {
			return ((c>=0x05B0)&&(c<=0x05BC))
				|| (c==0x05C1) || (c==0x05C2);
		}
		public static bool IsCantillation(char c) {
			return !IsNikud(c) &&
				((c>=0x0591) && (c<=0x05AE)) || (c==0x05BD);
		}
		public static bool IsModifier(char c) {
			return IsNikud(c) || (c=='\'') || (c=='´') || (c=='`');
		}
		public static bool IsLetter(char c) {
			return (c>=0x05D0)&&(c<=0x05EA);
		}
		public static bool IsEhevi(char c) {
			return "אהוי".IndexOf(c)>-1;
		}
		public static bool IsBegedKefet(char c) {
			return "בגדכפת".IndexOf(c)>-1;
		}
		public static bool IsPunctuation(char c) {
			return "-?!.,:;׃׀".IndexOf(c)>-1;
		}
		public static bool IsGuttural(char c) {
			return "האחרע".IndexOf(c)>-1;
		}
		
		public const char Dagesh='ּ';
		public const char Mappiq='ּ';
		public const char Shuruk='ּ';
		public const char ShinDot='ׁ';
		public const char SinDot='ׂ';

        public const char HatafPatah = '\u05B2';
        public const char Patah = '\u05B7';
        public const char Kamatz = '\u05B8';
        public const char Segol = '\u05B6';
        public const char Tzere = '\u05B5';
        public const char HatafSegol = '\u05B1';
        public const char HatafKamatz = '\u05B3';
        public const char Hirik = '\u05B4';
        public const char Holam = '\u05B9';
        public const char Kubutz = '\u05BB';
        public const char Schwa = '\u05B0';

		public static readonly char[] Vowels=new char[11] {'\u05B0','\u05B1','\u05B2','\u05B3','\u05B4','\u05B5','\u05B6','\u05B7','\u05B8','\u05B9','\u05BB'};
		
		public const char Etnahta='\u0591';
		public const char TaamSegol='\u0592';
		public const char Shalshelet='\u0593';
		public const char ZaqefQatan='\u0594';
		public const char ZaqefGadol='\u0595';
		public const char Tipeha='\u0596';
		public const char Revia='\u0597';
		public const char Zarqa='\u0598';
		public const char Pashta='\u0599';
		public const char Yetiv='\u059a';
		public const char Tevir='\u059b';
		public const char Geresh='\u059c';
		public const char GereshMuqdam='\u059d';
		public const char Gershayim='\u059e';
		public const char QarneyPara='\u059f';
		public const char TelishaGedola='\u05a0';
		public const char Pazer='\u05a1';
		public const char Munah='\u05a3';
		public const char Mahapakh='\u05a4';
		public const char Merkha='\u05a5';
		public const char MerkhaKefula='\u05a6';
		public const char Darga='\u05a7';
		public const char Qadma='\u05a8';
		public const char TelishaQetana='\u05a9';
		public const char YerahBenYomo='\u05aa';
		public const char Ole='\u05ab';
		public const char Iluy='\u05ac';
		public const char Dehi='\u05ad';
		public const char Zinor='\u05ae';
		public const char MasoraCircle='\u05af';
		public const char Maqaf='\u05be';
		public const char Meteg='\u05bd';
		
		public static int DisjunctiveRank(char c) {
			switch(c) {
				case Pazer:
				case Geresh:
				case Gershayim:
				case TelishaGedola:
				case Munah:
				case QarneyPara:
					return 4;
				case Revia:
				case Zarqa:
				case Pashta:
				case Yetiv:
				case Tevir:
					return 3;
				case ZaqefQatan:
				case ZaqefGadol:
				case Tipeha:
				case TaamSegol:
				case Shalshelet:
					return 2;
				case Etnahta:
//				case Meteg:
					return 1;
				default:
					return 5;
			}
		}
	}
}

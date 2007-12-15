using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Qaryan.Core;

namespace Qaryan.GUI
{
    public class NikudTextBox : TextBox
    {
        bool useNikudMethod;
        public bool UseNikudMethod
        {
            get
            {
                return useNikudMethod;
            }
            set
            {
                useNikudMethod = value;
            }
        }

        static char[] letters = new char[] { 'à', 'á', 'â', 'ã', 'ä', 'å', 'æ', 'ç', 'è', 'é', 'ë', 'ì', 'î', 'ð', 'ñ', 'ò', 'ô', 'ö', '÷', 'ø', 'ù', 'ú', 'ê', 'í', 'ï', 'ó', 'õ' };
        static char[] engletters = new char[] { 't', 'c', 'd', 's', 'v', 'u', 'z', 'j', 'y', 'h', 'f', 'k', 'n', 'b', 'x', 'g', 'p', 'm', 'e', 'r', 'a', '>', 'l', 'o', 'i', ':', '<' };

        static char[] ehevi = new char[] { 'à', 'ä', 'å', 'é' };
        static char[] definiteConsonants = new char[] { 'á', 'â', 'ã', 'æ', 'ç', 'è', 'ë', 'ì', 'î', 'ð', 'ñ', 'ò', 'ô', 'ö', '÷', 'ø', 'ù', 'ú', 'ê', 'í', 'ï', 'ó', 'õ' };

        Dictionary<char, char> engToHeb;

        public NikudTextBox()
        {
            engToHeb = new Dictionary<char, char>();
            for (int i = 0; i < letters.Length; i++)
                engToHeb[char.ToLower(engletters[i])] = engToHeb[char.ToUpper(engletters[i])] = letters[i];
        }

        int CurLetterPos()
        {
            int i = Math.Min(Math.Max(0, this.SelectionStart - 1), 20);
            return Math.Max(0, this.Text.LastIndexOfAny(letters, Math.Max(0, this.SelectionStart - 1), i));
        }

        int PrevLetterPos(int cur)
        {
            int i = Math.Min(Math.Max(0, cur - 1), 20);
            int res = Math.Max(0, this.Text.LastIndexOfAny(letters, Math.Max(0, CurLetterPos() - 1), i));
            for (i = res; i < cur; i++)
            {
                char c = this.Text[i];
                if (!(HebrewChar.IsLetter(c) || HebrewChar.IsModifier(c) || HebrewChar.IsCantillation(c)))
                    return -1;
            }
            return res;
        }

        string GetLetterWithModifiers(int letterPos)
        {
            if (letterPos < 0)
                return "";
            if (letterPos >= Text.Length)
                return "";
            if (!HebrewChar.IsLetter(Text[letterPos]))
                return "";
            StringBuilder sb = new StringBuilder(20);
            int i = letterPos;
            do
            {
                sb.Append(this.Text[i]);
                i++;
            }
            while ((i < this.Text.Length) && HebrewChar.IsModifier(this.Text[i]));
            return sb.ToString();
        }

        string CurLetterWithModifiers()
        {
            return GetLetterWithModifiers(CurLetterPos());
        }

        string PrevLetterWithModifiers(int cur)
        {
            int prev = PrevLetterPos(cur);
            if (prev < 0)
                return "";
            else
                return GetLetterWithModifiers(cur);
        }


        void ReplaceLetter(int letterPos, string s)
        {
            bool setSel = letterPos == CurLetterPos();
            this.Text = this.Text.Remove(letterPos, GetLetterWithModifiers(letterPos).Length);
            this.Text = this.Text.Insert(letterPos, s);
            if (setSel)
                SelectionStart = letterPos + s.Length;
            ScrollToCaret();
        }

        void ReplaceCurLetter(string s)
        {
            ReplaceLetter(CurLetterPos(), s);
        }

        bool CurLetterHasModifier(char c)
        {
            string curLetter = CurLetterWithModifiers();
            return curLetter.IndexOf(c) > 0;
        }

        static char[] VowelsA = new char[] { HebrewChar.Patah, HebrewChar.Kamatz, HebrewChar.HatafPatah };
        static char[] VowelsE = new char[] { HebrewChar.Segol, HebrewChar.Tzere, HebrewChar.HatafSegol };
        //        static char[] VowelsI = new char[] { HebrewChar.Hirik };
        static char[] VowelsO = new char[] { HebrewChar.Holam, HebrewChar.HatafKamatz, HebrewChar.Kamatz };
        static char[] VowelsUI = new char[] { HebrewChar.Hirik, HebrewChar.Kubutz };


        static char[][] VowelCycles = new char[][] { VowelsA, VowelsE, VowelsO, VowelsUI, new char[] { HebrewChar.Schwa } };

        static char[] ShinDots = new char[] { HebrewChar.SinDot, HebrewChar.ShinDot };
        static char[][] ShinCycle = new char[][] { ShinDots };

        enum SequenceDisposition
        {
            First, Second, Last
        }

        void ToggleModifier(char c, SequenceDisposition disposition)
        {
            ToggleModifier(CurLetterPos(), c, disposition);
        }

        void ToggleModifier(int letterPos, char c, SequenceDisposition disposition)
        {
            string curLetter = GetLetterWithModifiers(letterPos);
            int i = curLetter.IndexOf(c);
            if (i > 0)
                ToggleModifier(letterPos, c, disposition, false);
            else
            {
                ToggleModifier(letterPos, c, disposition, true);
            }
        }

        void ToggleModifier(int letterPos, char c, SequenceDisposition disposition, bool toggleOn)
        {
            string letter = GetLetterWithModifiers(letterPos);
            string curLetter = CurLetterWithModifiers();
            int sel = CurLetterPos() + curLetter.Length;
            int i = letter.IndexOf(c);
            if ((i > 0) && (!toggleOn))
                for (; i > 0; i = letter.IndexOf(c))
                {
                    letter = letter.Remove(i);
                    sel--;
                }
            else if (toggleOn)
            {
                int pos;
                switch (disposition)
                {
                    case SequenceDisposition.Last:
                        pos = letter.Length;
                        break;
                    case SequenceDisposition.Second:
                        pos = Math.Min(2, letter.Length);
                        break;
                    case SequenceDisposition.First:
                    default:
                        pos = 1;
                        break;
                }
                letter = letter.Insert(pos, c.ToString());
                sel++;
            }
            ReplaceLetter(letterPos, letter);
            this.SelectionStart = sel;
        }

        void ToggleModifier(char c)
        {
            ToggleModifier(c, SequenceDisposition.Second);
        }

        void CycleModifiers(char[] modifiers, char[][] exclusivityGroup, SequenceDisposition disposition, bool emptyStep)
        {
            string curLetter = CurLetterWithModifiers();
            int i = curLetter.IndexOfAny(modifiers);
            if (i < 0)
            {
                int sel = CurLetterPos() + curLetter.Length;
                foreach (char[] arr in exclusivityGroup)
                {
                    if (arr == modifiers)
                        continue;
                    foreach (char toDelete in arr)
                    {
                        if (curLetter.IndexOf(toDelete) > -1)
                        {
                            ToggleModifier(toDelete);
                            ModifierRemoved(CurLetterPos(), toDelete, exclusivityGroup);
                        }
                    }
                }
                ToggleModifier(modifiers[0], disposition);
                ModifierAdded(CurLetterPos(), modifiers[0], exclusivityGroup);
            }
            else
            {
                char c = curLetter[i];
                int j = Array.IndexOf(modifiers, c);
                ToggleModifier(c);
                ModifierRemoved(CurLetterPos(), c, exclusivityGroup);
                j++;
                if ((!emptyStep) && (j >= modifiers.Length))
                    j = 0;
                if (j < modifiers.Length)
                {
                    ToggleModifier(modifiers[j], disposition);
                    ModifierAdded(CurLetterPos(), modifiers[j], exclusivityGroup);
                }
            }
        }

        void CycleModifiers(char[] modifiers, char[][] exclusivityGroup)
        {
            CycleModifiers(modifiers, exclusivityGroup, SequenceDisposition.Second, true);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                ToggleModifier(HebrewChar.Dagesh, SequenceDisposition.First);
            else if (e.KeyCode == Keys.F2)
                ToggleModifier('À');
        }

        void InsertText(string s)
        {
            int sel = SelectionStart;
            Text = Text.Remove(sel, SelectionLength);
            SelectionLength = 0;
            Text = Text.Insert(sel, s);
            SelectionStart = sel + s.Length;
            ScrollToCaret();
            //            Modified = true;
        }

        bool shift = false, ctrl = false;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            shift = e.Shift;
            ctrl = e.Control;
            base.OnKeyDown(e);
        }

        bool AtWordStart()
        {
            if (SelectionStart == 0)
                return true;
            return !(HebrewChar.IsNikud(Text[SelectionStart - 1]) || HebrewChar.IsLetter(Text[SelectionStart - 1]));
        }

        void ModifierAdded(int letterPos, char modifier, char[][] exclusivityGroup)
        {
            if (exclusivityGroup == VowelCycles)
            {
                string letter = GetLetterWithModifiers(letterPos);
                if (letter == "")
                    return;
                if ((letter[0] == 'å') && (modifier != HebrewChar.Holam) && (modifier != HebrewChar.Shuruk))
                {
                    int prev = PrevLetterPos(letterPos);
                    string lPrev = GetLetterWithModifiers(prev);
                    if (lPrev == "é")
                    {
                        prev = PrevLetterPos(prev);
                        lPrev = GetLetterWithModifiers(prev);
                    }
                    if (prev > -1)
                    {
                        if (lPrev.IndexOfAny(HebrewChar.Vowels) < 0)
                            ToggleModifier(prev, HebrewChar.Schwa, SequenceDisposition.Second, true);
                    }
                }
            }
        }

        void ModifierRemoved(int letterPos, char modifier, char[][] exclusivityGroup)
        {
            if (exclusivityGroup == VowelCycles)
            {
                string letter = GetLetterWithModifiers(letterPos);
                if (letter == "")
                    return;
                if ((letter[0] == 'å') && (modifier != HebrewChar.Holam) && (modifier != HebrewChar.Shuruk))
                {
                    int prev = PrevLetterPos(letterPos);
                    string lPrev = GetLetterWithModifiers(prev);
                    if (lPrev == "é")
                    {
                        prev = PrevLetterPos(prev);
                        lPrev = GetLetterWithModifiers(prev);
                    }
                    if (prev > -1)
                    {
                        ToggleModifier(prev, HebrewChar.Schwa, SequenceDisposition.Second, false);
                    }
                }
            }
        }

        bool IsInTag(int pos)
        {
            int i = -1, count = -1;
            do
            {
                i = Text.IndexOf('/', i + 1);
                count++;
            }
            while ((i < pos) && (i > -1));
            if (count > 0)
                return (count % 2) != 0;
            return false;
        }

        public event EventHandler CtrlEnter;

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((CtrlEnter != null) && ctrl && (e.KeyChar == (char)10))
            {
                e.Handled = true;
                CtrlEnter(this, new EventArgs());
                return;
            }
            if (UseNikudMethod)
            {
                bool hebrew = false, dagesh = false;
                if (shift)
                    if (engToHeb.ContainsKey(e.KeyChar))
                    {
                        dagesh = true;
                        e.KeyChar = engToHeb[e.KeyChar];
                    }
                    else if (e.KeyChar == '_')
                    {
                        CycleModifiers(VowelsA, VowelCycles);
                        e.Handled = true;
                        return;
                    }
                    else if (e.KeyChar == '+')
                    {
                        CycleModifiers(VowelsE, VowelCycles);
                        e.Handled = true;
                        return;
                    }
                    /*                else if (e.KeyChar == '|')
                                    {
                                        CycleModifiers(VowelsI, VowelCycles);
                                        e.Handled = true;
                                        return;
                                    }*/
                    else if (e.KeyChar == '}')
                    {
                        CycleModifiers(VowelsO, VowelCycles);
                        e.Handled = true;
                        return;
                    }
                    else if (e.KeyChar == '{')
                    {
                        CycleModifiers(VowelsUI, VowelCycles);
                        e.Handled = true;
                        return;
                    }
                    else if (e.KeyChar == '~')
                    {
                        string ltr = CurLetterWithModifiers();
                        if ((ltr != null) && (ltr[0] == 'ù'))
                        {
                            CycleModifiers(ShinDots, ShinCycle, SequenceDisposition.First, false);
                            e.Handled = true;
                            return;
                        }
                    }
                if (HebrewChar.IsLetter(e.KeyChar))
                    hebrew = true;
                if (hebrew)
                {
                    e.Handled = true;
                    if (!IsInTag(SelectionStart))
                    {
                        if (!AtWordStart())
                        {
                            bool addSchwa = false;
                            int prev0 = CurLetterPos(), prev1 = PrevLetterPos(prev0);
                            string lPrev0 = GetLetterWithModifiers(prev0), lPrev1 = GetLetterWithModifiers(prev1);
                            if (lPrev0.Length > 0)
                            {
                                if (HebrewChar.IsEhevi(lPrev0[0]))
                                {
                                    if (lPrev0[0] == 'å')
                                        addSchwa = lPrev0.IndexOfAny(new char[] { HebrewChar.Shuruk, HebrewChar.Holam }) < 0;
                                    else
                                        addSchwa = (lPrev0.IndexOfAny(HebrewChar.Vowels) < 0) && ((lPrev1 == "") || (lPrev1.IndexOfAny(HebrewChar.Vowels) < 0));
                                }
                                else
                                    addSchwa = (lPrev0.IndexOfAny(HebrewChar.Vowels) < 0);
                            }
                            if (addSchwa && (e.KeyChar != 'å'))
                                InsertText(HebrewChar.Schwa.ToString());
                        }
                        else
                            if (HebrewChar.IsBegedKefet(e.KeyChar))
                                dagesh = true;
                    }
                    if (dagesh)
                        InsertText(e.KeyChar + HebrewChar.Dagesh.ToString());
                    else
                        InsertText(e.KeyChar.ToString());
                }
                else
                    base.OnKeyPress(e);
            }
            else
                base.OnKeyPress(e);
        }
    }
}

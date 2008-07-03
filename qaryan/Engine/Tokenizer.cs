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
    /// <summary>
    /// Represents a single immutable token of any type.
    /// </summary>
    public class Token
    {
        private string value;

        /// <summary>
        /// The token's raw text value.
        /// </summary>
        public string Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Creates a token object with a given text value.
        /// </summary>
        /// <param name="Value"></param>
        public Token(string Value)
        {
            this.value = Value;
        }
    }

    /// <summary>
    /// Represents a tag token.
    /// <para>Tags are word prefixes enclosed in slashes which provide certain types of metainformation related to a word.</para>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Text</term>
    ///         <description>Meaning</description>
    ///     </listheader>
    ///     <item>
    ///         <term>/ל/</term>
    ///         <description>Designates a word of non-Hebrew origin.</description>
    ///     </item>
    ///     <item>
    ///         <term>/ס/</term>
    ///         <description>Forces the word stress to סמיכות (Smikhut [lit.: Compound noun]; no word stress), overriding all heuristics.</description>
    ///     </item>
    ///     <item>
    ///         <term>/ר/</term>
    ///         <description>Forces the word stress to מלרע (Milra; last syllable stressed), overriding all heuristics.</description>
    ///     </item>
    ///     <item>
    ///         <term>/ע/</term>
    ///         <description>Forces the word stress to מלעיל (Milel; penultimate syllable stressed), overriding all heuristics.</description>
    ///     </item>
    ///     <item>
    ///         <term>/עע/</term>
    ///         <description>Forces the word stress to מלעיל דמלעיל (Milel Demilel; antepenultimate syllable stressed), overriding all heuristics.</description>
    ///     </item>
    ///     <item>
    ///         <term>/לס/</term>
    ///         <description>Designates a word of non-Hebrew origin and forces the word stress to סמיכות (Smikhut [lit.: Compound noun]; no word stress), overriding all heuristics.</description>
    ///     </item>
    ///     <item>
    ///         <term>/לר/</term>
    ///         <description>Designates a word of non-Hebrew origin and forces the word stress to מלרע (Milra; last syllable stressed), overriding all heuristics.</description>
    ///     </item>
    ///     <item>
    ///         <term>/לע/</term>
    ///         <description>Designates a word of non-Hebrew origin and forces the word stress to מלעיל (Milel; penultimate syllable stressed), overriding all heuristics.</description>
    ///     </item>
    ///     <item>
    ///         <term>/לעע/</term>
    ///         <description>Designates a word of non-Hebrew origin and forces the word stress to מלעיל דמלעיל (Milel Demilel; antepenultimate syllable stressed), overriding all heuristics.</description>
    ///     </item>
    /// </list>
    /// </summary>
    public class TagToken : Token
    {
        /// <summary>
        /// Creates a tag token object with a given text value.
        /// </summary>
        /// <param name="value"></param>
        public TagToken(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Gets the tag token's value as represented by the <see cref="TagTypes">TagTypes</see> enumeration.
        /// </summary>
        public TagTypes Type
        {
            get
            {
                TagTypes result = TagTypes.Unrecognized;
                if (Value.Length < 1)
                    return result;
                int i;
                if (Value[0] == 'ל')
                {
                    result |= TagTypes.Foreign | TagTypes.User;
                    i = 1;
                }
                else
                    i = 0;
                switch (Value.Substring(i))
                {
                    case "ס": // smikhut, no stress
                        result |= TagTypes.Unstressed | TagTypes.User;
                        break;
                    case "ר": // milra
                        result |= TagTypes.Milra | TagTypes.User;
                        break;
                    case "ע": // milel
                        result |= TagTypes.Milel | TagTypes.User;
                        break;
                    case "עע": // milel demilel
                        result |= TagTypes.MilelDMilel | TagTypes.User;
                        break;
                    default:
                        result |= TagTypes.Unrecognized;
                        break;
                }
                return result;
            }
        }
    }

    /// <summary>
    /// Represents a punctuation token.
    /// </summary>
    public class PunctuationToken : Token
    {
        public PunctuationToken(string Value)
            : base(Value)
        {

        }
    }

    /// <summary>
    /// Represents a cantillation-mark token.
    /// </summary>
    public class CantillationToken : Token
    {
        public CantillationToken(string Value)
            : base(Value)
        {

        }
    }

    /// <summary>
    /// Represents a letter token along with any trailing modifiers (nikud).
    /// </summary>
    public class LetterToken : Token
    {
        char letter;

        /// <summary>
        /// Gets the letter associated with this token.
        /// </summary>
        public char Letter
        {
            get
            {
                return letter;
            }
        }

        string modifiers;

        /// <summary>
        /// Gets the string of modifiers (if any) attached to the letter.
        /// </summary>
        public string Modifiers
        {
            get
            {
                return modifiers;
            }
        }

        /// <summary>
        /// Queries for the presence of a specific modifier.
        /// </summary>
        /// <param name="c">The modifier to look for.</param>
        /// <returns><see langword="true"/> if the modifier is found, <see langword="false"/> otherwise.</returns>
        public bool HasModifier(char c)
        {
            return Modifiers.IndexOf(c) > -1;
        }

        /// <summary>
        /// Queries for the presence of the דגש (Dagesh) modifier.
        /// </summary>
        public bool HasDagesh
        {
            get
            {
                return HasModifier(HebrewChar.Dagesh);
            }
        }

        /// <summary>
        /// Queries for the presence of the שורוק (Shuruk) modifier in the proper context.
        /// </summary>
        public bool HasShuruk
        {
            get
            {
                return (Letter == 'ו') && HasModifier(HebrewChar.Shuruk);
            }
        }

        /// <summary>
        /// Queries for the presence of the מפיק (Mappiq) modifier in the proper context.
        /// </summary>
        public bool HasMappiq
        {
            get
            {
                return (Letter == 'ה') && HasModifier(HebrewChar.Mappiq);
            }
        }

        /// <summary>
        /// Queries for the presence of the apostrophe modifier.
        /// </summary>
        public bool HasApostrophe
        {
            get
            {
                return HasModifier('\'') || HasModifier('`') || HasModifier('´');
            }
        }

        /// <summary>
        /// Queries for the presence of any modifier from a specified list.
        /// </summary>
        /// <param name="c">The modifiers to look for.</param>
        /// <returns><see langword="true"/> if a modifier from the list is found, <see langword="false"/> otherwise.</returns>
        public bool HasAnyModifier(params char[] c)
        {
            return Modifiers.IndexOfAny(c) > -1;
        }

        /// <summary>
        /// Queries for the presence of vowel marks.
        /// </summary>
        public bool HasAnyVowels
        {
            get
            {
                return HasAnyModifier(HebrewChar.Vowels);
            }
        }

        /// <summary>
        /// Queries for the presence of vowel marks excluding a specified list.
        /// </summary>
        /// <param name="v">Vowels to exclude from the query.</param>
        /// <returns><see langword="true"/> if a vowel not listed in <paramref name="v"/> is found, <see langword="false"/> otherwise.</returns>
        public bool HasAnyVowelsExcept(params char[] v)
        {
            StringBuilder m = new StringBuilder(Modifiers);
            foreach (char c in v)
                m.Replace(c.ToString(), "");
            return m.ToString().IndexOfAny(HebrewChar.Vowels) > -1;
        }

        /// <summary>
        /// Gets the first modifier representing a vowel, or \u0000 if no vowels are found.
        /// </summary>
        public char FirstVowel
        {
            get
            {
                if (HasAnyVowels)
                    return Modifiers[Modifiers.IndexOfAny(new char[11] { '\u05B0', '\u05B1', '\u05B2', '\u05B3', '\u05B4', '\u05B5', '\u05B6', '\u05B7', '\u05B8', '\u05B9', '\u05BB' })];
                else
                    return (char)0;
            }
        }

        /// <summary>
        /// Creates a letter token object from its elements.
        /// </summary>
        /// <param name="letter">The letter part of the token.</param>
        /// <param name="modifiers">The modifier part of the token.</param>
        public LetterToken(char letter, string modifiers)
            : base(letter + modifiers)
        {

            this.letter = letter;
            this.modifiers = modifiers;
        }
    }

    /// <summary>
    /// Enumerates the permitted states of a <see cref="Tokenizer">Tokenizer</see>.
    /// </summary>
    public enum TokenState { Neutral, Letter, Tag };

    /// <summary>
    /// Classifies and structures an incoming stream of <see cref="char">char</see>s (assumed to represent Hebrew text), producing <see cref="Token">Token</see>s for further processing.
    /// </summary>
    /// <seealso cref="StringCharProducer"/>
    /// <seealso cref="Parser"/>
    public class Tokenizer : LookaheadConsumerProducer<char, Token>
    {
        TokenState state;
        StringBuilder sb;
        char letter;

        protected override void BeforeConsumption()
        {
            Log(LogLevel.MajorInfo, "Started");
            base.BeforeConsumption();
            state = TokenState.Neutral;
            sb = new StringBuilder();
            letter = (char)0;
        }

        protected override void Consume(Queue<char> InQueue)
        {
            char c = InQueue.Dequeue();
            _ItemConsumed(c);
            //			Console.WriteLine("Tokenizing input...");
            if (state == TokenState.Letter)
            {
                if (HebrewChar.IsModifier(c))
                {
                    sb.Append(c);
                    return;
                }
                else
                {
                    LetterToken lt = new LetterToken(letter, sb.ToString());
                    Log("Producing letter " + lt.Value);
                    this.Emit(lt);
                    sb.Length = 0;
                    state = TokenState.Neutral;
                }
            }

            switch (state)
            {
                case TokenState.Tag:
                    if (c == '/')
                    {
                        state = TokenState.Neutral;
                        TagToken tt = new TagToken(sb.ToString());
                        Log("Producing tag " + tt.Type);
                        this.Emit(tt);
                        sb.Length = 0;
                    }
                    else
                        sb.Append(c);
                    break;
                case TokenState.Neutral:
                    if (HebrewChar.IsLetter(c) || (c == '/'))
                    {
                        if (sb.Length > 0)
                        {
                            Token t = new Token(sb.ToString());
                            Log("Producing neutral token " + t.Value);
                            this.Emit(t);
                        }
                        if (c == '/')
                            state = TokenState.Tag;
                        else
                        {
                            letter = c;
                            state = TokenState.Letter;
                        }
                        sb.Length = 0;
                    }
                    else if (HebrewChar.IsCantillation(c))
                    {
                        CantillationToken ct = new CantillationToken(c.ToString());
                        Log("Producing cantillation mark " + ct.Value);
                        this.Emit(ct);
                    }
                    else if (HebrewChar.IsPunctuation(c))
                    {
                        PunctuationToken pt = new PunctuationToken(c.ToString());
                        Log("Producing punctuation " + pt.Value);
                        this.Emit(pt);
                    }
                    else
                        sb.Append(c);
                    break;
            }
        }

        protected override void AfterConsumption()
        {

            switch (state)
            {
                case TokenState.Neutral:
                    if (sb.Length > 0)
                    {
                        Token t = new Token(sb.ToString());
                        Log("Producing neutral token " + t.Value);
                        this.Emit(t);
                    }
                    break;
                case TokenState.Letter:
                    LetterToken lt = new LetterToken(letter, sb.ToString());
                    Log("Producing letter " + lt.Value);
                    this.Emit(lt);
                    break;
            }
            Log(LogLevel.MajorInfo, "Finished");
            base.AfterConsumption();
            _DoneProducing();
        }

        /// <summary>
        /// Tokenizes a string by creating a <see cref="StringCharProducer">StringCharProducer</see> instance internally.
        /// </summary>
        /// <param name="s">The string to tokenize.</param>
        public void Run(string s)
        {
            Log("Creating producer object from string");
            StringCharProducer producer = new StringCharProducer(s);
            Run(producer, 1);
        }
    }
}

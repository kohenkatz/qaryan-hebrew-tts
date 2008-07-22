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
    /// <summary>
    /// Represents a speech segment resulting from the Segmentation phase of synthesis.
    /// </summary>
    public class Segment : IEnumerable<SpeechElement>
    {
        protected List<SpeechElement> Elements;

        protected Segment()
        {
            Elements = new List<SpeechElement>();
        }

        /// <summary>
        /// Gets or sets a specific child element of this <see cref="T:Qaryan.Core.Segment">Segment</see>.
        /// </summary>
        /// <param name="index">Zero-based index of child element.</param>
        /// <returns>A <see cref="T:Qaryan.Core.Segment">Segment</see> object at the requested index.</returns>
        public SpeechElement this[int index]
        {
            get
            {
                return Elements[index];
            }
            set
            {
                Elements[index] = value;
            }
        }

        /// <summary>
        /// Adds a <see cref="SpeechElement">SpeechElement</see> to the current <see cref="T:Qaryan.Core.Segment">Segment</see>'s list of child elements.
        /// </summary>
        /// <param name="Element">The element to add.</param>
        public void Add(SpeechElement Element)
        {
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
    /// Given a "flat" input of <see cref="SpeechElement">SpeechElement</see>s, constructs a tree of <see cref="Segment">Segment</see> objects representing syllables, words, phrases and silence marks.
    /// </summary>
    /// <seealso cref="Parser"/>
    /// <seealso cref="Phonetizer"/>
    public class Segmenter : LookaheadConsumerProducer<SpeechElement, Segment>
    {
        Syllable curSyl;
        Segment curSegment;
        Word curWord;
        int curElementIndex = -1;

        List<SyllablePattern> StressHeuristics;

        bool relaxAudibleSchwa = true;

        string stressHeuristicsFile;

        Word.Stress defaultStress = Word.Stress.Milra;

        /// <summary>
        /// Gets or sets the default <see cref="Word.Stress">Word Stress</see> for words outputted by this <see cref="Segmenter">Segmenter</see>
        /// </summary>
        public Word.Stress DefaultStress
        {
            get { return defaultStress; }
            set { defaultStress = value; }
        }

        /// <summary>
        /// Enables or disables the pragmatic reduction of שווא נע (<see>Vowels.AudibleSchwa</see>) to silence following the casual speech style.
        /// </summary>
        public bool RelaxAudibleSchwa
        {
            get { return relaxAudibleSchwa; }
            set { relaxAudibleSchwa = value; }
        }

        /// <summary>
        /// Loads stress assignment rules from a named file.
        /// </summary>
        /// <param name="Filename">Name of the XML file to load.</param>
        public void LoadStressHeuristics(string Filename)
        {
            Log("Loading " + Path.GetFileName(Filename));
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
            Log(LogLevel.MajorInfo, "Started");
            base.BeforeConsumption();
            curElementIndex = -1;
            curSegment = curWord = null;
            curSyl = null;
            LoadStressHeuristics(stressHeuristicsFile);
        }

        /// <summary>
        /// Performs stress assignment and related processing on a word, and adds it to the queue of produced segments.
        /// </summary>
        /// <param name="w">The word obtained from the segmentation step.</param>
        void AddAndProcessWord(Word w)
        {
            if ((curSyl != null) && !w.Syllables.Contains(curSyl))
            {
                w.Syllables.Add(curSyl);
                curSyl = null;
            }
            SyllablePattern? sp = w.PlaceStress(StressHeuristics, DefaultStress);
//            Log(LogLevel.Debug, sp);
            bool beforeStress = true;
            foreach (Syllable syl in w.Syllables)
            {
                bool stressed = syl.IsStressed;
                if (stressed)
                    beforeStress = false;
                for (int i = 0; i < syl.Phonemes.Count; i++)
                {
                    SpeechElement e = syl.Phonemes[i];
                    if (e is Vowel)
                    {
                        Vowel v = (Vowel)e;
                        if (v.vowel == Vowels.KamatzIndeterminate)
                        {
                            if (beforeStress && (syl.Coda == SyllableCoda.Closed) && (syl.Phonemes[syl.Phonemes.Count - 1] is Consonant) /*&& ((w.Tag&TagTypes.Origin)!=TagTypes.Foreign)*/ && !stressed)
                                v.vowel = Vowels.KamatzKatan;
                            else
                                v.vowel = Vowels.KamatzGadol;
                        }
                        else if (v.vowel == Vowels.AudibleSchwa)
                        {
                            if ((w.Tag & TagTypes.Origin) == TagTypes.Foreign)
                                v.vowel = Vowels.SilentSchwa;
                            else
                            {
                                int j = w.Phonemes.IndexOf(e);
                                if ((j + 1 < w.Phonemes.Count)
                                    && (w.Phonemes[j + 1] is Consonant)
                                    && (j - 1 >= 0)
                                    && (w.Phonemes[j - 1] is Consonant)
                                    && (w.Phonemes[j + 1].Latin != w.Phonemes[j - 1].Latin))
                                {
                                    switch (w.Phonemes[j - 1].Latin)
                                    {
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
                                            int son = ((Consonant)w.Phonemes[j + 1]).Sonority -
                                                ((Consonant)w.Phonemes[j - 1]).Sonority;
                                            if (son >= 0)
                                                v.Silent = true;
                                            break;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            if (sp != null)
                Log(LogLevel.Info, "/{0}/ from pattern {1}", w.TranslitSyllablesStress, sp.ToString());
            else
                Log(LogLevel.Info, "/{0}/", w.TranslitSyllablesStress);
            Emit(w);
        }

        protected override void Consume(Queue<SpeechElement> InQueue)
        {
            if (InQueue.Count == 0)
                return;
            SpeechElement curElement = InQueue.Dequeue();
            Log("Consuming {0} ({1})", curElement.GetType().Name, curElement.Latin);
            _ItemConsumed(curElement);

            SpeechElement nextElement = null;
            if (InQueue.Count > 0)
                nextElement = InQueue.Peek();
            if (curSegment == null)
            {
                if ((curElement is Phoneme) || (curElement is WordTag))
                    curSegment = new Word();
                else
                    curSegment = new SeparatorSegment();
                curElementIndex = -1;
            }
            if (curSegment is Word)
            {
                curWord = (curSegment as Word);
                if (curElement is Phoneme)
                {
                    curWord.Add(curElement);
                    curElementIndex++;
                    if (curSyl == null)
                    {
                        curSyl = new Syllable(curWord);
                        curSyl.Start = curElementIndex;
                        curSyl.End = curElementIndex;
                    }
                    else if (curSyl.Nucleus == null)
                    {
                        curSyl.End++;
                        if ((curElement is Vowel) &&
                            (curElement as Vowel).IsVowelIn(Vowels.LegalNuclei))
                            curSyl.Nucleus = curElement as Vowel;
                    }
                    else if (curElement is Consonant)
                    {
                        if ((nextElement != null) && (nextElement is Vowel) && (nextElement as Vowel).IsVowelIn(Vowels.Inaudible))
                        {
                            curSyl.End++;
                            nextElement = InQueue.Dequeue();
                            Log("Assimilating a {1}", nextElement.GetType().Name, (nextElement as Vowel).vowel);
                        }
                        else if ((nextElement == null) || (nextElement is Separator))
                        {
                            curSyl.End++;
                            //							curWord.Syllables.Add(curSyl);
                            //							curSyl=null;
                        }
                        else
                        {

                            if (((curElement as Consonant).Flags & ConsonantFlags.StrongDagesh) != 0)
                            {
                                curSyl.End++;
                                curSyl.HintStrongDagesh = true;
                            }
                            curWord.Syllables.Add(curSyl);
                            curSyl = new Syllable(curWord);
                            curSyl.Start = curElementIndex;
                            curSyl.End = curElementIndex;
                        }
                    }
                    else
                    {
                        // non-nucleic vowel, move on
                        curSyl.End++;
                    }

                }
                else if (curElement is WordTag)
                    curWord.Tag = (curElement as WordTag).Tag;
                else if (curElement is Qaryan.Core.Cantillation)
                    curWord.CantillationMarks.Add((curElement as Cantillation).Mark);
                else if (curElement is Separator)
                {
                    AddAndProcessWord(curSegment as Word);
                    curSegment = null;
                }
            }
            if (curElement is Separator)
            {
                if (curSegment != null)
                {
                    if (curSegment is SeparatorSegment)
                    {
                        curSegment.Add(curElement);
                    }
                    else
                    {
                        if (curSegment is Word)
                            AddAndProcessWord(curSegment as Word);
                        else
                            Emit(curSegment);
                        curSegment = new SeparatorSegment(curElement as Separator);
                    }
                    Log("Adding separator segment");
                    Emit(curSegment);
                    curSegment = null;
                }
                else
                {
                    Log("Adding separator segment");
                    Emit(curSegment = new SeparatorSegment(curElement as Separator));
                    curSegment = null;
                }
            }
        }

        protected override void AfterConsumption()
        {
            if (curSegment != null)
            {
                if (curSegment is Word)
                    AddAndProcessWord(curSegment as Word);
                else
                    Emit(curSegment);
                curSegment = null;
            }
            Log(LogLevel.MajorInfo, "Finished");
            base.AfterConsumption();
            _DoneProducing();
        }

        public override void Run(Producer<SpeechElement> producer)
        {
            base.Run(producer, 2);
        }

        public Segmenter()
        {
            this.stressHeuristicsFile = FileBindings.StressHeuristicsPath;
        }
    }
}

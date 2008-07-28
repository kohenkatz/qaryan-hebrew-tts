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
using MotiZilberman;
using System.IO;
using System.Threading;
using Qaryan.Core;
using Qaryan.Synths.MBROLA;
using System.Diagnostics;
using CommandLine.OptParse;
using Qaryan.Audio;
using System.Reflection;

namespace Qaryan.CLI
{
    class Program
    {
        static LogLevel LogFilter;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Type `QaryanCLI -h` for help");
                Console.Error.WriteLine("If you meant input from stdin and output to soundcard, use `QaryanCLI -`");
                return;
            }
            OptionResultsDictionary opts = new OptionResultsDictionary();
            OptionDefinition[] optdefs = new OptionDefinition[] {
                new OptionDefinition("help",OptValType.Flag,typeof(string),"General options","Show this help message",new char[]{'h','?'},new string[]{"help"}),
                new OptionDefinition("verbose",OptValType.IncrementalFlag,typeof(IntPtr),"General options","Verbose output",new char[]{'v'},new string[]{"verbose"}),
                new OptionDefinition("voice",OptValType.ValueReq,typeof(string),"Voice options","Use the specified voice",new char[]{'V'},new string[]{"voice"}),
                new OptionDefinition("list-voices",OptValType.Flag,typeof(string),"Voice options","List the available voices",new char[]{'l'},new string[]{"list-voices"}),
                new OptionDefinition("pho",OptValType.ValueOpt,typeof(string),"Output options","Write phonetic information to the specified file ('-' for stdout, the default)",new char[]{'P'},new string[]{"pho"}),
                new OptionDefinition("out",OptValType.ValueOpt,typeof(string),"Output options","Write audio to the specified file ('-' for stdout, the default)",new char[]{'o'},new string[]{"out","audio"}),
                new OptionDefinition("raw",OptValType.Flag,typeof(string),"Output options","Output raw audio data instead of WAV",new char[]{'R'},new string[]{"raw"})
            };

            CommandLine.OptParse.Parser optp = ParserFactory.BuildParser(optdefs, opts);
            string[] arguments = optp.Parse(OptStyle.Unix, UnixShortOption.CollapseShort, DupOptHandleType.Allow, UnknownOptHandleType.NoAction, true, args);
            if (opts["help"] != null)
            {
                Assembly asm = Assembly.GetEntryAssembly();
                //                string s = (asm.ManifestModule.GetCustomAttributes(typeof(AssemblyProductAttribute), true)[0] as AssemblyProductAttribute).Product;
                //s+= " " + (asm.ManifestModule.GetCustomAttributes(typeof(AssemblyVersionAttribute), true)[0] as AssemblyVersionAttribute).Version;
                UsageBuilder usage = new UsageBuilder();
                usage.GroupOptionsByCategory = true;
                //                usage.BeginSection(s);
                //                usage.EndSection();

                usage.BeginSection("Usage");
                usage.AddParagraph(Path.GetFileName(asm.CodeBase) + " [options] [infile] [-P phofile] [-o outfile]");
                usage.EndSection();

                usage.BeginSection("Description");
                usage.AddParagraph("Bare-bones command line interface to the Qaryan text-to-speech engine.");
                usage.EndSection();

                usage.BeginSection("Options");
                usage.AddOptions(optdefs);
                usage.EndSection();

                usage.BeginSection("Environment Variables");
                usage.AddParagraph("Setting the following variables is not required, but might help if the defaults don't work as expected.");
                usage.BeginList(ListType.Unordered);
                usage.AddListItem("MBROLA holds the path to the MBROLA executable.");
                usage.AddListItem("MBROLA_DATABASE_DIR holds the path where MBROLA databases may be found.");
                usage.AddListItem("QARYAN_ROOT holds the path where shared Qaryan files may be found.");
                usage.EndList();
                usage.EndSection();
                usage.ToText(Console.Error, OptStyle.Unix, true);
                return;
            }
            string qaryanpath = System.Environment.GetEnvironmentVariable("QARYAN_ROOT");
            if (qaryanpath != null)
                FileBindings.EnginePath = qaryanpath;
            if (opts["list-voices"] != null)
            {
                string dir = FileBindings.VoicePath;
                Console.Error.WriteLine("Available voices:");
                foreach (string file in Directory.GetFiles(dir + "/", "*.xml"))
                {
                    Voice voice = new Voice();
                    voice.Load(file);
                    if (voice.BackendSupported)
                        Console.Error.WriteLine("{2} voice {0} ({1})",
                            Path.GetFileNameWithoutExtension(file), voice.DisplayName, voice.BackendName);
                    voice = null;

                }
                return;
            }


            //MBROLA.Mbrola.Binding = MBROLA.MbrolaBinding.Standalone;
            Console.InputEncoding = Encoding.UTF8;
            TextReaderCharProducer prod = new TextReaderCharProducer();
            prod.ItemProduced += new ProduceEventHandler<char>(prod_ItemProduced);
            QaryanEngine myEngine = new QaryanEngine();

            TextReader textSrc = Console.In;
            foreach (string argument in arguments)
            {
                if (File.Exists(argument))
                {
                    textSrc = File.OpenText(argument);
                    break;
                }
            }

            DelegateConsumer<MBROLAElement> cons = new DelegateConsumer<MBROLAElement>();
            cons.ItemConsumed += new ConsumeEventHandler<MBROLAElement>(cons_ItemConsumed);

            if (opts["verbose"] != null)
            {
                if (opts["verbose"].NumDefinitions > 2)
                    LogFilter = LogLevel.All;
                else if (opts["verbose"].NumDefinitions > 1)
                    LogFilter = LogLevel.All ^ LogLevel.Debug;
                else
                    LogFilter = LogLevel.All ^ (LogLevel.Debug | LogLevel.Info);
            }
            else
                LogFilter = LogLevel.All ^ (LogLevel.Debug | LogLevel.Info | LogLevel.MajorInfo);
            myEngine.LogLine += OnLogLine;
            string voiceName = "mbrola-hb2";
            if (opts["voice"] != null)
                voiceName = opts["voice"].Value as string;

            Voice myVoice;
            myVoice = new Voice();
            myVoice.Load(Path.Combine(FileBindings.VoicePath, voiceName + ".xml"));
            myEngine.Voice = myVoice;
            if (opts["pho"] != null)
            {
                if ((opts["pho"].Value as string == "-") || (opts["pho"].Value == null))
                    (myEngine.Backend as MbrolaBackend).Translator.ItemProduced += delegate(Producer<MBROLAElement> sender, ItemEventArgs<MBROLAElement> e)
                      {
                          Console.Write(e.Item);
                      };
                else
                {
                    StreamWriter sw = File.CreateText(opts["pho"].Value as string);
                    (myEngine.Backend as MbrolaBackend).Translator.ItemProduced += delegate(Producer<MBROLAElement> sender, ItemEventArgs<MBROLAElement> e)
                    {
                        sw.Write(e.Item);
                    };
                    (myEngine.Backend as MbrolaBackend).Translator.DoneProducing += delegate(object sender, EventArgs e)
                    {
                        sw.Close();
                    };
                }
            }

            if (opts["out"] != null)
            {
                if ((opts["out"].Value as string == "-") || (opts["out"].Value == null))
                {
                    if (opts["raw"] == null)
                        myEngine.SpeakToWavStream(textSrc, Console.OpenStandardOutput());
                    else
                        myEngine.SpeakToRawStream(textSrc, Console.OpenStandardOutput());

                }
                else
                {
                    if (opts["raw"] == null)
                        myEngine.SpeakToWavFile(textSrc, opts["out"].Value as string);
                    else
                        myEngine.SpeakToRawFile(textSrc, opts["out"].Value as string);
                }
            }
            else
                myEngine.Speak(textSrc);
            /*}
            else
            {
                myEngine.SpeakToNull(textSrc);
            }*/
        }

        static void OnLogLine(ILogSource sender, string message, LogLevel visibility)
        {
            if ((visibility & LogFilter) == visibility)
                Console.Error.WriteLine("{0}> {1}", sender.Name, message);
        }

        static void prod_ItemProduced(Producer<char> sender, ItemEventArgs<char> e)
        {
            //            Console.WriteLine("produced " + (int)e.Item);
        }

        static void cons_ItemConsumed(Consumer<MBROLAElement> sender, ItemEventArgs<MBROLAElement> e)
        {
            Console.Write(e.Item);
        }
    }
}

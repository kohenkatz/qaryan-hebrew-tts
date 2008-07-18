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
    public class TextReaderCharProducer : Producer<char>
    {
        Queue<char> queue;
        public event ProduceEventHandler<char> ItemProduced;
        public event EventHandler DoneProducing;
        bool isRunning = false;

        public TextReaderCharProducer()
        {
            queue = new Queue<char>();

        }

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public Queue<char> OutQueue
        {
            get
            {
                return queue;
            }
        }

        public void Run(TextReader reader)
        {
            isRunning = true;
            Thread t = new Thread(delegate()
            {
                try
                {
                    while (Thread.CurrentThread.IsAlive)
                    {
                        char c;
                        OutQueue.Enqueue(c = (char)(reader.Read()));
                        if (c == 65535)
                            break;
                        if (ItemProduced != null)
                            ItemProduced(this, new ItemEventArgs<char>(c));
                    }
                }
                catch (IOException)
                {
                }
                isRunning = false;
                if (DoneProducing != null)
                    DoneProducing(this, new EventArgs());

            });
            t.Start();
        }
    }

    class Program
    {
        static LogLevel LogFilter;

        static void Main(string[] args)
        {
            OptionResultsDictionary opts = new OptionResultsDictionary();
            OptionDefinition[] optdefs = new OptionDefinition[] {
                new OptionDefinition("help",OptValType.Flag,typeof(string),"General options","Show this help message",new char[]{'h','?'},new string[]{"help"}),
                new OptionDefinition("verbose",OptValType.IncrementalFlag,typeof(IntPtr),"General options","Verbose output",new char[]{'v'},new string[]{"verbose"}),
                new OptionDefinition("mbrola-voice",OptValType.ValueReq,typeof(string),"MBROLA configuration","Use the specified MBROLA \"voice\" (database) file",new char[]{'d'},new string[]{"mbrola-voice","mbrola-db"}),
                new OptionDefinition("pho",OptValType.ValueOpt,typeof(string),"Output options","Write phonetic information to the specified file ('-' for stdout, the default)",new char[]{'P'},new string[]{"pho"}),
                new OptionDefinition("out",OptValType.ValueOpt,typeof(string),"Output options","Write audio to the specified file ('-' for stdout, the default)",new char[]{'o'},new string[]{"out","audio"}),
                new OptionDefinition("raw",OptValType.Flag,typeof(string),"Output options","Output raw audio data instead of WAV",new char[]{'R'},new string[]{"raw"})
            };


            CommandLine.OptParse.Parser optp = ParserFactory.BuildParser(optdefs, opts);
            string[] arguments = optp.Parse(OptStyle.Unix, UnixShortOption.CollapseShort, DupOptHandleType.Allow, UnknownOptHandleType.NoAction, false, args);
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
                usage.AddParagraph("Setting the following optional variables may ease configuration of Qaryan on your machine.");
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

            MBROLA.Mbrola.Binding = MBROLA.MbrolaBinding.Standalone;
            Console.InputEncoding = Encoding.UTF8;
            TextReaderCharProducer prod = new TextReaderCharProducer();
            prod.ItemProduced += new ProduceEventHandler<char>(prod_ItemProduced);
            Tokenizer tok = new Tokenizer();
            Qaryan.Core.Parser par = new Qaryan.Core.Parser();
            Segmenter seg = new Segmenter();
            Phonetizer pho = new Phonetizer();
            FujisakiProcessor fuji = new FujisakiProcessor();
            MBROLATranslator tra = new MBROLATranslator();
            MBROLAProcessSynthesizer mbr = null;


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
            tok.LogLine += OnLogLine;
            par.LogLine += OnLogLine;
            seg.LogLine += OnLogLine;
            pho.LogLine += OnLogLine;
            fuji.LogLine += OnLogLine;
            tra.LogLine += OnLogLine;
            tra.Voice = new MBROLAVoice();
            string voiceName = "hb2";
            if (opts["mbrola-voice"] != null)
                voiceName = opts["mbrola-voice"].Value as string;
            tra.Voice.LoadFromXml(Path.Combine(FileBindings.VoicePath, voiceName + ".xml"));

            /* mbrola = new System.Diagnostics.Process();
             mbrola.StartInfo.FileName = "C:\\downloads\\mbrola_cygwin.exe";
             mbrola.StartInfo.Arguments = " ..\\..\\..\\Engine\\Voices\\hb2 - out3.wav";
             mbrola.StartInfo.RedirectStandardInput = true;
             mbrola.StartInfo.CreateNoWindow = true;
             mbrola.StartInfo.UseShellExecute = false;
             mbrola.Start();*/
            AudioTarget audio = null;
            if (opts["out"] != null)
            {
                if ((opts["out"].Value as string == "-") || (opts["out"].Value == null))
                {
                    audio = new StreamAudioTarget();
                    (audio as StreamAudioTarget).Stream = Console.OpenStandardOutput();
                    (audio as StreamAudioTarget).WriteHeader = opts["raw"] == null;
                }
                else
                {
                    audio = new WaveFileAudioTarget();
                    (audio as WaveFileAudioTarget).Filename = opts["out"].Value as string;
                    (audio as WaveFileAudioTarget).WriteHeader = opts["raw"] == null;
                }
            }
            else if (opts["pho"] == null)
            {
                audio = PlatformInstantiator<AudioTarget>.Create(typeof(LibaoAudioTarget), typeof(WaveOutAudioTarget));
            }
            if (audio != null)
            {
                mbr = new MBROLAProcessSynthesizer();
                mbr.Voice = tra.Voice;
            }
            else
            //            if ((opts["pho"] != null) || (opts["out"] == null))
            {
                if ((opts["pho"] == null) ||
                    ((opts["pho"] != null) &&
                    (opts["pho"].Value as string == "-") || (opts["pho"].Value == null)))
                    tra.ItemProduced += delegate(Producer<MBROLAElement> sender, ItemEventArgs<MBROLAElement> e)
                      {
                          Console.Write(e.Item);
                      };
                else
                {
                    StreamWriter sw = File.CreateText(opts["pho"].Value as string);
                    tra.ItemProduced += delegate(Producer<MBROLAElement> sender, ItemEventArgs<MBROLAElement> e)
                    {
                        sw.Write(e.Item);
                    };
                    tra.DoneProducing += delegate(object sender, EventArgs e)
                    {
                        sw.Close();
                    };
                }
            }
            prod.Run(textSrc);
            tok.Run(prod);
            par.Run(tok);
            seg.Run(par);
            pho.Run(seg);
            fuji.Run(pho);
            tra.Run(fuji);
            if ((mbr != null) && (audio != null))
            {
                mbr.LogLine += OnLogLine;
                string mbrpath = System.Environment.GetEnvironmentVariable("MBROLA");
                if (mbrpath != null)
                    mbr.MBROLAExecutable = mbrpath;
                else
                    mbr.MBROLAExecutable = "mbrola";
                string dbpath = System.Environment.GetEnvironmentVariable("MBROLA_DATABASE_DIR");
                if (dbpath != null)
                    mbr.MBROLAVoicePath = dbpath;
                mbr.Run(tra);
                audio.Run(mbr);
                audio.Join();
            }
            else
                tra.Join();
        }

        static void OnLogLine(ILogSource sender, string message, LogLevel visibility)
        {
            if ((visibility & LogFilter) == visibility)
                Console.Error.WriteLine("{0}: {1}", sender.Name, message);
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

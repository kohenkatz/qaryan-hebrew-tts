using System;
using System.Collections.Generic;
using System.Text;
using Qaryan.Core;
using Qaryan.Audio;
using MotiZilberman;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Qaryan.Synths.MBROLA
{
    /// <summary>
    /// A cross-platform interface to the main MBROLA executable, provided as an alternative
    /// to MBROLASynthesizer.
    /// </summary>
    public class MBROLAProcessSynthesizer : Synthesizer<MBROLAElement>
    {
        public override string Name
        {
            get
            {
                return "Synthesizer";
            }
        }
        Process mbrola;

        MBROLAVoice voice;

        long samplesBalance;

        bool isDoneConsuming = false;
        bool isDoneSynth = false;

        string _MBROLAExecutable = "mbrola";

        public string MBROLAExecutable
        {
            get
            {
                return _MBROLAExecutable;
            }
            set
            {
                _MBROLAExecutable = value;
            }
        }

        string _MBROLAVoicePath = FileBindings.VoicePath;

        public string MBROLAVoicePath
        {
            get
            {
                return _MBROLAVoicePath;
            }
            set
            {
                _MBROLAVoicePath = value;
                if (Voice != null)
                    Voice.MBROLAVoicePath = MBROLAVoicePath;
            }
        }

        public MBROLAVoice Voice
        {
            get { return voice; }
            set
            {
                voice = value;
                voice.MBROLAVoicePath = MBROLAVoicePath;
            }
        }


        public override WaveFormat AudioFormat
        {
            get
            {
                WaveFormat format = base.AudioFormat;
                format.Channels = 1;
                if (voice != null)
                    format.SamplesPerSecond = voice.SampleRate;
                else
                    format.SamplesPerSecond = 22050;
                format.BitsPerSample = 16;
                format.AverageBytesPerSecond = format.SamplesPerSecond * format.BitsPerSample / 8;
                format.BlockAlign = (ushort)(format.Channels * (format.BitsPerSample / 8));
                format.FormatTag = WaveFormatTag.Pcm;
                return format;
            }
            set
            {
                base.AudioFormat = value;
            }
        }
        
        Thread t;

        protected override void BeforeConsumption()
        {
            if (!File.Exists(Voice.FileName))
            {
                Log(LogLevel.Error, "The MBROLA voice '{0}' could not be found. Make sure that MBROLA_DATABASE_DIR is set correctly.", Voice.Name);
                //                Thread.CurrentThread.Abort();
                throw new FileNotFoundException(String.Format("The MBROLA voice '{0}' could not be found. Make sure that MBROLA_DATABASE_DIR is set to a correct value.", Voice.Name), Voice.FileName);
            }
            base.BeforeConsumption();
            Log(LogLevel.MajorInfo,"Started");
            samplesBalance = 0;
            isDoneConsuming = isDoneSynth = false;
            mbrola = new System.Diagnostics.Process();
            mbrola.StartInfo.FileName = MBROLAExecutable;


            mbrola.StartInfo.Arguments = "-e \"" + Voice.FileName + "\" - -";
            mbrola.StartInfo.RedirectStandardInput = true;
            mbrola.StartInfo.RedirectStandardOutput = true;
            //mbrola.StartInfo.RedirectStandardError = true;
            mbrola.StartInfo.CreateNoWindow = true;
            mbrola.StartInfo.UseShellExecute = false;
            //mbrola.ErrorDataReceived += new DataReceivedEventHandler(mbrola_ErrorDataReceived);

            try
            {
                mbrola.Start();
//                mbrola.BeginErrorReadLine();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Log(LogLevel.Error, "MBROLA failed to load properly. Make sure that MBROLA_DATABASE_DIR is set to a correct value.");
                Thread.CurrentThread.Abort();
                return;
            }
            catch( Exception e)
            {
                Log(LogLevel.Error,"Could not start MBROLA process. Make sure either that MBROLA is in the PATH, or that the MBROLA environment variable is set appropriately.");
                Log(LogLevel.Error,e);
                Thread.CurrentThread.Abort();
                return;
            }
            mbrola.StandardInput.AutoFlush = true;
            t = new Thread(delegate()
            {
                int ofs = 0;
                while (Thread.IsAlive && !mbrola.HasExited)
                {
                    Log("about to read from mbrola");
                    while (mbrola.StandardOutput.EndOfStream && !mbrola.HasExited)
                    {
                        Log("waiting on mbrola");
                        Thread.Sleep(0);
                    }
                    byte[] buf = new byte[2048];
                    int l = 0;
                    try
                    {
                        if (!mbrola.HasExited)
                        {
                            Log("reading...");
                            l = mbrola.StandardOutput.BaseStream.Read(buf, ofs, buf.Length - ofs);
                        }
                    }
                    catch (IOException)
                    {
                        Log("reading timed out");
                        break;
                    }
                    byte[] abuf = new byte[l];
                    Buffer.BlockCopy(buf, ofs, abuf, 0, l);
                    Log("Read {0} into buffer of {1}", l, buf.Length);
/*                    lock (this)
                        samplesBalance -= (uint)l;*/
                    Log("samplesBalance is now {0}", samplesBalance);
                    if (abuf.Length > 0)
                        BufferReady(abuf);
                    else if (mbrola.HasExited)
                    {
                        if (!mbrola.HasExited)
                        {
                            mbrola.Kill();
                        }
                        break;
                    }

                    ofs += l;
                    if (ofs >= buf.Length)
                        ofs = 0;
                }
                if (!mbrola.HasExited)
                    mbrola.StandardOutput.ReadToEnd();
                InvokeAudioFinished();
                isDoneSynth = true;
            }
            );
            t.Start();
        }

        void mbrola_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log(LogLevel.Warning, "From MBROLA '{0}'", e.Data);
        }

        protected override void AfterConsumption()
        {
            Log("After consumption");
            base.AfterConsumption();
            isDoneConsuming = true;
            Log("writing ^Z");
            mbrola.StandardInput.BaseStream.WriteByte(26);  // ^Z
            mbrola.StandardInput.BaseStream.WriteByte(10);  // ^Z
            mbrola.StandardInput.BaseStream.WriteByte(13);  // ^Z
            Log("reading from mbrola to end");
            Log("Waiting for mbrola to exit");
            mbrola.WaitForExit();
            Log(LogLevel.MajorInfo,"Finished");
        }

        protected override void Consume(Queue<MBROLAElement> InQueue)
        {
            while (InQueue.Count > 0)
            {
                MBROLAElement elem = InQueue.Dequeue();
/*                lock (this)
                    samplesBalance += (long)Math.Ceiling(elem.Duration * AudioFormat.SamplesPerSecond / 1000);*/
                Log("Writing ", elem.Symbol);
                mbrola.StandardInput.Write(elem);
            }
        }

        public override bool IsRunning
        {
            get
            {
                return !isDoneSynth;
            }
        }

        public override void Run(Producer<MBROLAElement> producer)
        {
            base.Run(producer, 1);
        }
    }
}

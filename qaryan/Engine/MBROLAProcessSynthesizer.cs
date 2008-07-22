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
    public class MBROLAProcessSynthesizer : MBROLASynthesizerBase
    {
        public override string Name
        {
            get
            {
                return "Synthesizer";
            }
        }
        Process mbrola;

//        bool isDoneConsuming = false;
        bool isDoneSynth = false;

        string _MBROLAExecutable = System.Environment.GetEnvironmentVariable("MBROLA") ?? "mbrola";

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

        Thread t;

        protected override void BeforeConsumption()
        {
            if (!File.Exists((Voice.BackendVoice as MbrolaVoiceNew).DatabaseFile))
            {
                Log(LogLevel.Error, "The MBROLA voice '{0}' could not be found. Make sure that MBROLA_DATABASE_DIR is set correctly.", (Voice.BackendVoice as MbrolaVoiceNew).Database);
                //                Thread.CurrentThread.Abort();
                throw new FileNotFoundException(String.Format("The MBROLA voice '{0}' could not be found. Make sure that MBROLA_DATABASE_DIR is set to a correct value.", (Voice.BackendVoice as MbrolaVoiceNew).Database), (Voice.BackendVoice as MbrolaVoiceNew).DatabaseFile);
            }
            Log(LogLevel.MajorInfo, "Started");

            base.BeforeConsumption();

            /*isDoneConsuming = */isDoneSynth = false;
            mbrola = new System.Diagnostics.Process();
            mbrola.EnableRaisingEvents = true;
            mbrola.StartInfo.FileName = MBROLAExecutable;

            mbrola.StartInfo.Arguments =
                string.Format("-v {1} -e \"{0}\" - -", (Voice.BackendVoice as MbrolaVoiceNew).DatabaseFile, (Voice.BackendVoice as MbrolaVoiceNew).VolumeRatio);
            
            mbrola.StartInfo.RedirectStandardInput = true;
            mbrola.StartInfo.RedirectStandardOutput = true;
            //mbrola.StartInfo.RedirectStandardError = true;
            mbrola.StartInfo.CreateNoWindow = true;
            mbrola.StartInfo.UseShellExecute = false;
            mbrola.ErrorDataReceived += new DataReceivedEventHandler(mbrola_ErrorDataReceived);

            try
            {
				Log("Starting MBROLA with arguments: {0}",mbrola.StartInfo.Arguments);
                mbrola.Start();
//                mbrola.BeginErrorReadLine();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Log(LogLevel.Error, "MBROLA failed to load properly. Your system might be missing an MBROLA binary - get one from <http://tcts.fpms.ac.be/synthesis/mbrola/mbrcopybin.html>");
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
                while (Thread.CurrentThread.IsAlive && 
                    !mbrola.HasExited)
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
                            Log("Reading...");
                            l = mbrola.StandardOutput.BaseStream.Read(buf, ofs, buf.Length - ofs);
                            
                        }
                    }
                    catch (IOException)
                    {
                        Log("reading timed out");
                        break;
                    }
                    if (l > 0)
                    {
                        byte[] abuf = new byte[l];
                        Buffer.BlockCopy(buf, ofs, abuf, 0, l);
                        Log("Read {0} into buffer of {1}", l, buf.Length);
                        BufferReady(abuf);
                    }
                    else if (mbrola.HasExited)
                        break;

                    ofs += l;
                    if (ofs >= buf.Length)
                        ofs = 0;
                }
                if (!mbrola.HasExited)
                    mbrola.StandardOutput.ReadToEnd();
                isDoneSynth = true;
                if (mbrola.HasExited)
                    Log(LogLevel.MajorInfo, "MBROLA has terminated");
                else
                {
                    mbrola.Kill();
                    Log(LogLevel.MajorInfo, "MBROLA was terminated");
                }
                InvokeAudioFinished();
            }
            );
            t.Start();
        }

        void mbrola_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log(LogLevel.Warning, "From MBROLA: '{0}'", e.Data);
        }

        protected override void AfterConsumption()
        {
            Log("After consumption");
            Log(LogLevel.MajorInfo, "Finished");
            base.AfterConsumption();
//            isDoneConsuming = true;
//            Log("writing ^Z");
            mbrola.StandardInput.BaseStream.WriteByte(26);  // ^Z
            mbrola.StandardInput.BaseStream.WriteByte(10);  // ^Z
            mbrola.StandardInput.BaseStream.WriteByte(13);  // ^Z
            Log("Closing MBROLA stdin");
            //mbrola.StandardInput.Close();
            mbrola.StandardInput.BaseStream.Close();
            //mbrola.WaitForExit();

        }

        protected override void Consume(Queue<MBROLAElement> InQueue)
        {
            while (InQueue.Count > 0)
            {
                MBROLAElement elem = InQueue.Dequeue();
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

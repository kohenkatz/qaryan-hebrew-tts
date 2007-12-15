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
 * Date: 8/25/2007
 * Time: 10:18 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Qaryan.Core;
using Qaryan.Audio;
using MotiZilberman;
using MBROLA;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections;

namespace Qaryan.Synths.MBROLA
{
 
    /// <summary>
    /// An interface to the MBROLA synthesizer implemented on top of MbrPlay.dll.
    /// </summary>
    public class MBROLASynthesizer : Synthesizer<MBROLAElement>
    {
        public event StringEventHandler Error;

        public override WaveFormat AudioFormat
        {
            get
            {
                WaveFormat format=base.AudioFormat;
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

        StringBuilder pho;

        MBROLAVoice voice;

        internal bool IsDoneConsuming = false;

        public MBROLAVoice Voice
        {
            get { return voice; }
            set
            {
                voice = value;
                voice.Activate();
            }
        }

        MbrThread myMbrThread;

        protected override void BeforeConsumption()
        {
            base.BeforeConsumption();
            pho = new StringBuilder();
            Log(LogLevel.MajorInfo,"Started");
            IsDoneConsuming = false;
//            Mbrola.Init("C:\\Documents and Settings\\Moti Z\\My Documents\\SharpDevelop Projects\\Qaryan.Core refactor\\Voices\\" + Voice.Name);
            MbrPlay.SetDatabase(voice.Name);
            MbrPlay.SetVolumeRatio(voice.VolumeRatio);
            myMbrThread = new MbrThread();
            myMbrThread.Error += delegate(object sender, string error)
            {
                Error(this, error);
            };
            myMbrThread.Start(this);
        }

        protected override void AfterConsumption()
        {


        


            myMbrThread.AddFragment(pho.ToString());
            myMbrThread.AddFragment("EOF");

            /*            Console.WriteLine("Synthesizing...");
                        Console.WriteLine(pho);
                        HebrewParser.tf = Environment.TickCount;
                        Console.WriteLine("Text to speech in {0}s", (HebrewParser.tf - HebrewParser.t0) / 1000);
                        MbrPlay.SetDurationRatio(0.9f);
                        MbrPlay.Play(pho.ToString(), (int)MbrOut.Wave | (int)MbrFlags.Wait | (int)MbrFlags.Callback | (int)MbrFlags.MsgInit | (int)MbrFlags.MsgRead | (int)MbrFlags.MsgEnd | (int)MbrOut.Disabled, null, mbrCallback);
                        System.Threading.Thread.Sleep(100);
                        if (File.Exists("mbrtemp.wav"))
                            new System.Media.SoundPlayer("mbrtemp.wav").Play();
                        StringBuilder error = new StringBuilder(256);
                        MbrPlay.LastError(error, 255);*/
            //			MbrPlay.Unload();
            /*			HebrewParser.Log.Synthesizer.WriteLine(pho.ToString());
            HebrewParser.Log.Synthesizer.WriteLine(error);
            HebrewParser.Log.General.WriteLine(error);*/
//            isRunning = false;
            //            Console.WriteLine(error);

            myMbrThread.Join();

            base.AfterConsumption();
            IsDoneConsuming = true;
            Log(LogLevel.MajorInfo,"Finished");
        }

//        bool isRunning = false;

        public class MbrThread
        {
            public bool Eof = false;
            public bool Finished = false;

            MBROLASynthesizer Synth;

            public event StringEventHandler Error;

            int mbrCallback(MbrMessage msg, IntPtr wParam, int lParam)
            {
                switch (msg)
                {
                    case MbrMessage.Init:
                        Synth.Log("Synth initialized");
//                        fout.Seek(0, SeekOrigin.End);
                        break;
//                    case MbrMessage.Read:
                        //                    fwrite((short*)wParam, sizeof(short), lParam, fout);
//                        Synth.Log("WM_MBR_READ wParam={0}, lParam={1}", wParam.ToInt32().ToString("X"), lParam);
//                        break;
                    case MbrMessage.Write:
                        Synth.Log("{0} samples received from synth", lParam);
                        byte[] buf = new byte[lParam * sizeof(short)];
                        Marshal.Copy(wParam, buf, 0, lParam * sizeof(short));
//                        AudioBufferInfo abuf = new AudioBufferInfo();
//                        abuf.Format
//                        Synth.Emit(abuf);
                        Synth.BufferReady(buf);
//                        fout.Write(buf, 0, buf.Length);
                        break;
                    case MbrMessage.End:
                        Finished = true;
                        Synth.Log("Synth terminated");
//                        fout.Close();
                        break;
                    default:
                        Synth.Log("MbrMessage " + msg);
                        break;
                }
                return 0;
            }

            Queue<String> Fragments = new Queue<string>();

            public void AddFragment(string phoFragment)
            {
                Fragments.Enqueue(phoFragment);
            }

            Thread t=null;

            PlayCallbackProc callback;

            public void Start(MBROLASynthesizer Synth)
            {
                callback = new PlayCallbackProc(mbrCallback);
                this.Synth = Synth;
                t = new Thread(delegate()
                {
                    while (Thread.CurrentThread.IsAlive)
                    {
                        if (Fragments.Count == 0)
                        {
                            Thread.Sleep(0);
                            continue;
                        }
                        string fragment = Fragments.Dequeue();
                        if (fragment == "EOF")
                        {
                            while (!Finished)
                                Thread.Sleep(0);
                            Eof = true;
                            Synth.InvokeAudioFinished();
                            return;
                        }
                        else
                        {
                            Synth.Log("Sent {0} characters to synth",fragment.Length);
                            
                            MbrPlay.Play(fragment, (int)MbrFlags.Queued | (int)MbrFlags.Wait | (int)MbrFlags.MsgInit | (int)MbrFlags.MsgWrite | (int)MbrFlags.MsgEnd | (int)MbrFlags.Callback | (int)MbrOut.Disabled, null, callback);
                            
                            StringBuilder error = new StringBuilder(256);
                            MbrPlay.LastError(error, 255);
                            if (error.Length>0)
                                this.Error(this, error.ToString());
                        }
                    }
                });
                t.Name = "MbrThread";
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }

            public void Join()
            {
                if (t != null)
                    t.Join();
            }

            public void Stop()
            {
                if (t != null)
                    t.Abort();
                t = null;
            }
        }

        protected override void Consume(Queue<MBROLAElement> InQueue)
        {
            //			base.Consume(InQueue);
            //isRunning = true;
            while (InQueue.Count > 0)
            {
                //                mbrParser.ConsumeSingle(InQueue.Dequeue());
                MBROLAElement element = InQueue.Dequeue(),e1,e2;
                _ItemConsumed(element);
                if ((element.Symbol == "_") && (pho.Length>0))
                {
                    element.SplitHalf(out e1, out e2);
                    pho.Append(e1);
                    myMbrThread.AddFragment(pho.ToString());
                    pho.Remove(0, pho.Length);
                    pho.Append(e2);
                }
                else
                    pho.Append(element);
                
            }
        }

        public override bool IsRunning
        {
            get
            {
                if (IsDoneConsuming && (myMbrThread != null))
                    return !(myMbrThread.Finished&&myMbrThread.Eof);
                else
                    return base.IsRunning;
            }
        }

        public override void Run(Producer<MBROLAElement> producer)
        {

            base.Run(producer, 1);
        }


        public MBROLASynthesizer()
        {
        }
    }
}

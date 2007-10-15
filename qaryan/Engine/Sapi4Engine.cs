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
 * Date: 9/28/2007
 * Time: 7:50 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using DirectTextToSpeech;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using MotiZilberman;
using Qaryan.Core;
using Qaryan.Synths.MBROLA;
using Qaryan.Audio;

namespace Qaryan.Interfaces.SAPI4
{
    [ComVisible(true)]
    public class TTSEnumImpl : ITTSEnum
    {
        public static IntPtr Offset(IntPtr src, int offset)
        {
            unsafe { return new IntPtr((byte*)src + offset); }
        }
        public static IntPtr Offset(IntPtr src, long offset)
        {
            unsafe { return new IntPtr((byte*)src + offset); }
        }

        int modeIndex = 0;

        Sapi4Engine.ModeInfo[] modes;

        internal TTSEnumImpl(Sapi4Engine.ModeInfo[] modeArray)
        {
            Sapi4Engine.miniLog("EnumImpl created");
            Sapi4Engine.miniLog("\tmodeArray=" + modeArray.ToString());
            modes = modeArray;
        }

        #region ITTSEnum Members

        public void Clone(IntPtr ppenum)
        {
            try
            {
                Sapi4Engine.miniLog("EnumImpl.Clone");
                IntPtr penum = Marshal.GetComInterfaceForObject(new TTSEnumImpl(modes), typeof(ITTSEnum));
                Marshal.WriteIntPtr(ppenum, penum);
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        public void Next(uint celt, IntPtr rgelt, IntPtr pceltFetched)
        {
            Sapi4Engine.miniLog("EnumImpl.Next");
            Sapi4Engine.miniLog("\tmodes=" + modes.ToString());
            Sapi4Engine.miniLog(String.Format("\t({0},{1},{2})", celt, rgelt.ToString(), pceltFetched.ToString()));
            int c = Math.Min((int)celt, (modes.Length - modeIndex));
            uint celtFetched;
            if (c > 0)
            {
                celtFetched = (uint)c;
            }
            else
                celtFetched = 0;
            Sapi4Engine.miniLog("\tfetching " + celtFetched + " elements");
            //Marshal.AllocCoTaskMem(Marshal.SizeOf(TTSMODEINFO));
            //            rgelt = new TTSMODEINFO[pceltFetched];
            for (int i = 0; i < (int)celtFetched; i++)
            {
                //    rgelt[i] = modes[modeIndex + i]; 
                Sapi4Engine.miniLog("\tat " + (modeIndex + i) + ": " + modes[modeIndex + i].SapiMode);
                Marshal.StructureToPtr(modes[modeIndex + i].SapiMode, rgelt, false);
                rgelt = Offset(rgelt, Marshal.SizeOf(typeof(TTSMODEINFO)));
            }
            if (pceltFetched != IntPtr.Zero)
                Marshal.WriteInt32(pceltFetched, (int)celtFetched);
            modeIndex += (int)celtFetched;
        }

        public void Reset()
        {
            Sapi4Engine.miniLog("EnumImpl.Reset");
            modeIndex = 0;
        }

        public void Select(Guid gModeID, out ITTSCentral ppiTTSCentral, IntPtr pIUnknownForAudio)
        {
            Sapi4Engine.miniLog(String.Format("EnumImpl.Select({0})", gModeID));
            ppiTTSCentral = new Sapi4Engine(Array.Find<Sapi4Engine.ModeInfo>(modes, delegate(Sapi4Engine.ModeInfo mode)
            {
                return mode.SapiMode.gModeID == gModeID;
            }), pIUnknownForAudio);
        }

        public void Skip(uint celt)
        {
            Sapi4Engine.miniLog("EnumImpl.Skip");
            modeIndex += (int)celt;
            throw new Exception("ACK!@");
        }

        #endregion
    }
    #region ITTSEnum Members

    /// <summary>
    /// Description of Sapi4Engine.
    /// </summary>
    [Guid("ca519c69-c703-45e1-87b6-443cfa90f6db")]
    [ComDefaultInterface(typeof(ITTSCentral))]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Qaryan.Core.Sapi4Engine")]
    [ComVisible(true)]
    public class Sapi4Engine : ITTSAttributes, ITTSCentral, ITTSDialogs, ITTSEnum, ILexPronounce, IDisposable
    {

        internal struct ModeInfo
        {
            public TTSMODEINFO SapiMode;
            public MBROLAVoice Voice;
        }

        Dictionary<uint, ITTSNotifySink> notifySinks = new Dictionary<uint, ITTSNotifySink>();
        uint nextSinkKey;

        ModeInfo modeInfo;

        IAudioDest audioObject;

        internal Sapi4Engine(ModeInfo modeInfo, IntPtr pIUnknownForAudio)
        {
            this.modeInfo = modeInfo;
            Marshal.AddRef(pIUnknownForAudio);
            audioObject = Marshal.GetObjectForIUnknown(pIUnknownForAudio) as IAudioDest;
        }

        internal static void miniLog(string s)
        {
            TextWriter tw = File.AppendText("c:\\" + System.Reflection.Assembly.GetExecutingAssembly().FullName + ".log.txt");

            tw.WriteLine(s);
            tw.Flush();
            tw.Close();
        }


        public Sapi4Engine()
        {
            miniLog("instance created");
            myEnumImpl = new TTSEnumImpl(modes);
            notifySinks = new Dictionary<uint, ITTSNotifySink>();
            nextSinkKey = 0;
        }


        ushort Pitch = 80;
        uint Speed = 100;
        uint Volume = 100;

        #region ITTSAttributes Members

        void ITTSAttributes.PitchGet(ref ushort pwPitch)
        {
            miniLog("PitchGet");
            pwPitch = Pitch;
        }

        void ITTSAttributes.PitchSet(ushort wPitch)
        {
            miniLog("PitchSet");
            Pitch = wPitch;
        }

        void ITTSAttributes.RealTimeGet(ref uint pdwRealTime)
        {
            miniLog("RealTimeGet");
            pdwRealTime = 200;
        }

        void ITTSAttributes.RealTimeSet(uint dwRealTime)
        {
            miniLog("RealTimeSet");
        }

        void ITTSAttributes.SpeedGet(ref uint pdwSpeed)
        {
            miniLog("SpeedGet");
            pdwSpeed = Speed;
        }

        void ITTSAttributes.SpeedSet(uint dwSpeed)
        {
            miniLog("SpeedSet");
            Speed = dwSpeed;
        }

        void ITTSAttributes.VolumeGet(ref uint pdwVolume)
        {
            miniLog("VolumeGet");
            pdwVolume = Volume;
        }

        void ITTSAttributes.VolumeSet(uint dwVolume)
        {
            miniLog("VolumeSet");
            Volume = dwVolume;
        }

        #endregion

        #region ITTSCentral Members

        void ITTSCentral.AudioPause()
        {
            try
            {
                miniLog("AudioPause");
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.AudioReset()
        {
            try
            {

                miniLog("AudioReset");
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.AudioResume()
        {
            try
            {

                miniLog("AudioResume");
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.Inject(string pszTag)
        {
            try
            {
                miniLog("Inject");
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.ModeGet(ref TTSMODEINFO pttsInfo)
        {
            try
            {

                miniLog("ModeGet");
                pttsInfo = modeInfo.SapiMode;
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.Phoneme(VOICECHARSET eCharacterSet, uint dwFlags, SDATA dText, out SDATA pdPhoneme)
        {
            try
            {

                miniLog("Phoneme");
                pdPhoneme = new SDATA();
                pdPhoneme.dwSize = 0;
            }
            catch (Exception e)
            {
                pdPhoneme = new SDATA();
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.PosnGet(out ulong pqwTimeStamp)
        {
            try
            {
                miniLog("PosnGet");
                pqwTimeStamp = 0;
            }
            catch (Exception e)
            {
                pqwTimeStamp = 0;
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.Register(object pNotifyInterface, Guid IIDNotifyInterface, out uint pdwKey)
        {
            try
            {

                miniLog("Register " + IIDNotifyInterface + " " + pNotifyInterface.ToString());
                notifySinks.Add(nextSinkKey, (ITTSNotifySink)pNotifyInterface);
                pdwKey = nextSinkKey++;
            }
            catch (Exception e)
            {
                pdwKey = 0;
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.TextData(VOICECHARSET eCharacterSet, uint dwFlags, SDATA dText, object pNotifyInterface, Guid IIDNotifyInterface)
        {
            try
            {
                miniLog("TextData " + dText.AsString);
                StringCharProducer scp = new StringCharProducer(dText.AsString);
                Tokenizer tokenizer = new Tokenizer();
                Parser parser = new Parser();
                Segmenter segmenter = new Segmenter();
                Phonetizer phonetizer = new Phonetizer();
                FujisakiProcessor fujisaki = new FujisakiProcessor();
                MBROLATranslator translator = new MBROLATranslator();
                MBROLASynthesizer synthesizer = new MBROLASynthesizer();
                synthesizer.Voice = translator.Voice = modeInfo.Voice;
                tokenizer.Run(scp);
                parser.Run(tokenizer);
                segmenter.Run(parser);
                phonetizer.Run(segmenter);
                fujisaki.Run(phonetizer);
                translator.Run(fujisaki);
                synthesizer.Run(translator);

                /*                DelegateConsumer<MBROLAElement> delegc = new DelegateConsumer<MBROLAElement>();
                                delegc.ItemConsumed += delegate(MBROLAElement item)
                                {
                                    miniLog("dummy sees: " + item.ToString());
                                };
                                delegc.Run(translator);
                                delegc.Join();*/
                Sapi4AudioTarget target = new Sapi4AudioTarget(audioObject);
                target.Run(synthesizer);
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.ToFileTime(ref ulong pqTimeStamp, out _FILETIME pFT)
        {
            try
            {
                miniLog("ToFileTime");
                pFT.dwHighDateTime = pFT.dwLowDateTime = 0;
            }
            catch (Exception e)
            {
                pFT = new _FILETIME();
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSCentral.UnRegister(uint dwKey)
        {
            try
            {
                miniLog("UnRegister");
                if (notifySinks.ContainsKey(dwKey))
                    notifySinks.Remove(dwKey);
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        #endregion

        #region ITTSDialogs Members

        void ITTSDialogs.AboutDlg(ref IntPtr hWndParent, string pszTitle)
        {
            miniLog("AboutDlg");
        }

        void ITTSDialogs.GeneralDlg(ref IntPtr hWndParent, string pszTitle)
        {
            miniLog("GeneralDlg");
        }

        void ITTSDialogs.LexiconDlg(ref IntPtr hWndParent, string pszTitle)
        {
            miniLog("LexiconDlg");
        }

        void ITTSDialogs.TranslateDlg(ref IntPtr hWndParent, string pszTitle)
        {
            miniLog("TranslateDlg");
        }

        #endregion

        static ModeInfo[] modes;

        static Sapi4Engine()
        {
            miniLog("static Sapi4Engine()");
            modes = new ModeInfo[1];
            modes[0].SapiMode = new TTSMODEINFO();
            modes[0].SapiMode.gModeID = new Guid("c26bec88-4df1-4e18-a562-9946ca1231a0");
            modes[0].SapiMode.gEngineID = new Guid("ca519c69-c703-45e1-87b6-443cfa90f6db");
            modes[0].SapiMode.dwEngineFeatures = 0;
            modes[0].SapiMode.dwFeatures = (int)(TTSFEATURE.ANYWORD | TTSFEATURE.PCOPTIMIZED | TTSFEATURE.THREADSAFE);
            modes[0].SapiMode.dwInterfaces = (int)(TTSI.ITTSAttributes | TTSI.ITTSCentral | TTSI.ITTSDialogs);
            modes[0].SapiMode.szMfgName = "The Qaryan.Core project";
            modes[0].SapiMode.szModeName = "Esther (Female #1), Hebrew, Qaryan.Core";
            modes[0].SapiMode.szProductName = "Qaryan.Core";
            modes[0].SapiMode.szSpeaker = "Esther";
            modes[0].SapiMode.szStyle = "Business";
            modes[0].SapiMode.wGender = (short)GENDER.GENDER_FEMALE;
            modes[0].SapiMode.wAge = (short)TTSAGE.ADULT;
            modes[0].SapiMode.language = new LANGUAGE();
            modes[0].SapiMode.language.LanguageID = 0x040d;
            modes[0].SapiMode.language.szDialect = "Israeli Standard";
            modes[0].Voice = new MBROLAVoice();
            modes[0].Voice.LoadFromXml(Path.Combine(FileBindings.StressHeuristicsPath, "hb2.xml"));         
        }



        TTSEnumImpl myEnumImpl;

        void ITTSEnum.Clone(IntPtr ppenum)
        {
            miniLog("Clone");
            myEnumImpl.Clone(ppenum);
        }

        void ITTSEnum.Next(uint celt, IntPtr rgelt, IntPtr pceltFetched)
        {
            try
            {
                miniLog("Next");
                myEnumImpl.Next(celt, rgelt,
                    pceltFetched);
            }
            catch (Exception e)
            {
                Sapi4Engine.miniLog(e + ": " + e.Message);
            }
        }

        void ITTSEnum.Reset()
        {
            miniLog("Reset");
            myEnumImpl.Reset();
        }

        [STAThread]
        void ITTSEnum.Select(Guid gModeID, out ITTSCentral ppiTTSCentral, IntPtr pIUnknownForAudio)
        {
            miniLog("Select");
            myEnumImpl.Select(gModeID, out ppiTTSCentral, pIUnknownForAudio);
        }

        void ITTSEnum.Skip(uint celt)
        {
            miniLog("Skip");
            myEnumImpl.Skip(celt);
        }

    #endregion

        #region ILexPronounce Members

        void ILexPronounce.Add(VOICECHARSET CharSet, string pszText, string pszPronounce, VOICEPARTOFSPEECH PartOfSpeech, IntPtr pEngineInfo, int dwEngineInfoSize)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        void ILexPronounce.Get(VOICECHARSET CharSet, string pszText, short wSense, string pszPronounce, int dwPronounceSize, ref int pdwPronounceNeeded, ref VOICEPARTOFSPEECH pPartOfSpeech, IntPtr pEngineInfo, int dwEngineInfoSize, ref int pdwEngineInfoNeeded)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        void ILexPronounce.Remove(string pszText, short wSense)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Marshal.ReleaseComObject(audioObject);
        }

        #endregion
    }
}

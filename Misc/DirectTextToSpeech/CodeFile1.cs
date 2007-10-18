using System;
using System.Runtime.InteropServices;

namespace DirectTextToSpeech
{
    public enum VOICECHARSET
    {
        CHARSET_TEXT = 0,
        CHARSET_IPAPHONETIC = 1,
        CHARSET_ENGINEPHONETIC = 2
    }

    public enum TTSAGE : short
    {
        BABY = 1,
        TODDLER = 3,
        CHILD = 6,
        ADOLESCENT = 14,
        ADULT = 30,
        ELDERLY = 70
    }

    public enum GENDER : short
    {
        GENDER_NEUTRAL = 0,
        GENDER_FEMALE = 1,
        GENDER_MALE = 2
    }

    public enum VOICEPARTOFSPEECH : int
    {
        VPS_UNKNOWN = 0,
        VPS_NOUN = 1,
        VPS_VERB = 2,
        VPS_ADVERB = 3,
        VPS_ADJECTIVE = 4,
        VPS_PROPERNOUN = 5,
        VPS_PRONOUN = 6,
        VPS_CONJUNCTION = 7,
        VPS_CARDINAL = 8,
        VPS_ORDINAL = 9,
        VPS_DETERMINER = 10,
        VPS_QUANTIFIER = 11,
        VPS_PUNCTUATION = 12,
        VPS_CONTRACTION = 13,
        VPS_INTERJECTION = 14,
        VPS_ABBREVIATION = 15,
        VPS_PREPOSITION = 16
    };

    [Flags]
    public enum TTSFEATURE : int
    {
        ANYWORD = 1,
        VOLUME = 2,
        SPEED = 4,
        PITCH = 8,
        TAGGED = 16,
        IPAUNICODE = 32,
        VISUAL = 64,
        WORDPOSITION = 128,
        PCOPTIMIZED = 256,
        PHONEOPTIMIZED = 512,
        FIXEDAUDIO = 1024,
        SINGLEINSTANCE = 2048,
        THREADSAFE = 4096,
        IPATEXTDATA = 8192,
        PREFERRED = 16384,
        TRANSPLANTED = 32768,
        SAPI4 = 65536
    }

    [Flags]
    public enum TTSI : int
    {
        ILexPronounce = 1,
        ITTSAttributes = 2,
        ITTSCentral = 4,
        ITTSDialogs = 8,
        Attributes = 16,
        IAttributes = 32,
        ILexPronounce2 = 64
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TTSMOUTH
    {
        public byte bMouthHeight;
        public byte bMouthWidth;
        public byte bMouthUpturn;
        public byte bJawOpen;
        public byte bTeethUpperVisible;
        public byte bTeethLowerVisible;
        public byte bTonguePosn;
        public byte bLipTension;
    }

    [StructLayout(LayoutKind.Sequential, Size = 8, Pack = 1)]
    public struct SDATA
    { // sdata
        public IntPtr pData;
        public int dwSize;

        public string AsString
        {
            get
            {
                return Marshal.PtrToStringUni(pData, dwSize / 2);
            }
            set
            {
                pData = Marshal.StringToCoTaskMemUni(value);
                dwSize = (value.Length + 1) * 2;
            }
        }

        public WAVEFORMATEX AsWaveFormatEx
        {
            get
            {
                dwSize = Marshal.SizeOf(typeof(WAVEFORMATEX));
                WAVEFORMATEX result=(WAVEFORMATEX) Marshal.PtrToStructure(pData, typeof(WAVEFORMATEX));
                dwSize += result.cbSize;
                return result;
            }
            set
            {
                dwSize = Marshal.SizeOf(typeof(WAVEFORMATEX))+value.cbSize;
                pData = Marshal.AllocHGlobal(dwSize);
                Marshal.StructureToPtr(value,pData,false);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LANGUAGE
    { // lang
        public ushort LanguageID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szDialect;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct TTSMODEINFO
    { // ttsmi
        public Guid gEngineID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 262)]
        public string szMfgName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 262)]
        public string szProductName;
        public Guid gModeID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 262)]
        public string szModeName;
        public LANGUAGE language;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 262)]
        public string szSpeaker;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 262)]
        public string szStyle;
        public short wGender;
        public short wAge;
        public int dwFeatures;
        public int dwInterfaces;
        public int dwEngineFeatures;
    }

    [StructLayoutAttribute(LayoutKind.Sequential,
          Pack = 4, Size = 0)]
    public struct _FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Size = 18, Pack = 1)]
    public struct WAVEFORMATEX
    {
        public short wFormatTag;
        public short nChannels;
        public int nSamplesPerSec;
        public int nAvgBytesPerSec;
        public short nBlockAlign;
        public short wBitsPerSample;
        public short cbSize;
    };

    [Guid("6B837B20-4A47-101B-931A-00AA0047BA4F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ITTSEnum
    {
        void Next(uint celt,

     IntPtr rgelt,
            IntPtr pceltFetched);
        void Skip(uint celt);
        void Reset();
        void Clone(IntPtr ppenum);


        void Select(Guid gModeID, out ITTSCentral ppiTTSCentral,
            IntPtr pIUnknownForAudio);

    }

    [Guid("47F59D00-4A47-101B-931A-00AA0047BA4F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ITTSDialogs
    {
        void AboutDlg(ref IntPtr hWndParent, string pszTitle);
        void LexiconDlg(ref IntPtr hWndParent, string pszTitle);
        void GeneralDlg(ref IntPtr hWndParent, string pszTitle);
        void TranslateDlg(ref IntPtr hWndParent, string pszTitle);
    }

    [Guid("28016060-4A47-101B-931A-00AA0047BA4F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ITTSCentral
    {
        void Inject(string pszTag);
        void ModeGet(ref TTSMODEINFO pttsInfo);
        void Phoneme(VOICECHARSET eCharacterSet, uint dwFlags, SDATA dText, out SDATA pdPhoneme);
        void PosnGet(out ulong pqwTimeStamp);
        void TextData(VOICECHARSET eCharacterSet, uint dwFlags, SDATA dText, [MarshalAs(UnmanagedType.Interface,IidParameterIndex=4)] object pNotifyInterface, Guid IIDNotifyInterface);
        void ToFileTime(ref ulong pqTimeStamp, out _FILETIME pFT);
        void AudioPause();
        void AudioResume();
        void AudioReset();
        void Register(
            [MarshalAs(UnmanagedType.Interface,IidParameterIndex=1)]
            object pNotifyInterface, Guid IIDNotifyInterface, out uint pdwKey);

        void UnRegister(uint dwKey);
    }

    [Guid("1287A280-4A47-101B-931A-00AA0047BA4F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ITTSAttributes
    {
        void PitchGet(ref ushort pwPitch);
        void PitchSet(ushort wPitch);
        void RealTimeGet(ref uint pdwRealTime);
        void RealTimeSet(uint dwRealTime);
        void SpeedGet(ref uint pdwSpeed);
        void SpeedSet(uint dwSpeed);
        void VolumeGet(ref uint pdwVolume);
        void VolumeSet(uint dwVolume);
    }

    [Guid("090CD9A2-DA1A-11CD-B3CA-00AA0047BA4F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ILexPronounce
    {
        void Add(
               VOICECHARSET CharSet,
                [MarshalAs(UnmanagedType.LPWStr)]
	            string pszText,
                [MarshalAs(UnmanagedType.LPWStr)]
	            string pszPronounce,
                VOICEPARTOFSPEECH PartOfSpeech,
                IntPtr pEngineInfo,
                int dwEngineInfoSize);

        void Get(
            VOICECHARSET CharSet,
            [MarshalAs(UnmanagedType.LPWStr)]
	        string pszText,
            short wSense,
            [MarshalAs(UnmanagedType.LPWStr)]
	        string pszPronounce,
            int dwPronounceSize,
            ref int pdwPronounceNeeded,
            ref VOICEPARTOFSPEECH pPartOfSpeech,
            IntPtr pEngineInfo,
            int dwEngineInfoSize,
            ref int pdwEngineInfoNeeded);

        void Remove([MarshalAs(UnmanagedType.LPWStr)] string pszText, short wSense);
    }

    [Guid("C0FA8F40-4A46-101B-931A-00AA0047BA4F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ITTSNotifySink
    {
        void AttribChanged(int dwAttribute);
        void AudioStart(long qTimeStamp);
        void AudioStop(long qTimeStamp);
        void Visual(
            long qTimeStamp,
            [MarshalAs(UnmanagedType.U2)]
			char cIPAPhoneme,
            [MarshalAs(UnmanagedType.U2)]
			char cEnginePhoneme,
            int dwHints,
            [MarshalAs(UnmanagedType.LPStruct)]
			ref TTSMOUTH pTTSMouth);
    }

    [Guid("F546B340-C743-11cd-80E5-00AA003E4B50")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface IAudio
    {
        void Flush();
        void LevelGet(out int pdwLevel);
        void LevelSet(int dwLevel);
        void PassNotify([MarshalAs(UnmanagedType.Interface,IidParameterIndex=1)] object pNotifyInterface, Guid IIDNotifyInterface);
        void Claim();
        void UnClaim();
        void Start();
        void Stop();
        void TotalGet(out long pqWord);
        void ToFileTime(ref long pqWord, ref System.Runtime.InteropServices.ComTypes.FILETIME pFT);
        void WaveFormatGet(
            [MarshalAs(UnmanagedType.Struct)]
            out SDATA pdWFEX);
        void WaveFormatSet(
            [MarshalAs(UnmanagedType.Struct)]
            SDATA dWFEX);
    }

    [Guid("2EC34DA0-C743-11cd-80E5-00AA003E4B50")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface IAudioDest
    {
        void FreeSpace(ref int pdwBytes, [MarshalAs(UnmanagedType.Bool)] ref bool pfEOF);
        void DataSet(IntPtr pBuffer, int dwSize);
        void BookMark(int dwMarkID);
    }

    [Guid("ACB08C00-C743-11cd-80E5-00AA003E4B50")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface IAudioDestNotifySink
    {
        void AudioStop(short wReason);
        void AudioStart();
        void FreeSpace(int dwBytes, [MarshalAs(UnmanagedType.Bool)] bool fEOF);
        void BookMark(int dwMarkID, [MarshalAs(UnmanagedType.Bool)] bool fFlush);
    }
}

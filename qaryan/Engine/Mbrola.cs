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
 * User: Moti Zilberman
 * Date: 4/5/2007
 * Time: 7:16 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MBROLA
{
    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate bool EnumDatabaseCallback([MarshalAs(UnmanagedType.LPStr)] string lpszID, int dwUserData);



    public enum MbrMessage : int
    {
        Init = (0x0400 + 0x1BFF),
        Read = (0x0400 + 0x1C00),
        Wait = (0x0400 + 0x1C01),
        Write = (0x0400 + 0x1C02),
        End = (0x0400 + 0x1C03),
        Tag = (0x0400 + 0x1C04)
    }

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate int PlayCallbackProc(MbrMessage msg, IntPtr wParam, int lParam);

    [Flags]
    public enum MbrFlags
    {
        MsgInit = 1,
        MsgRead = 2,
        MsgWait = 4,
        MsgWrite = 8,
        MsgEnd = 16,
        MsgAll = 31,
        ByFile = 32,
        Wait = 64,
        Callback = 128,
        Queued = 256,
        AsPHS = 512
    }

    public enum MbrOut
    {
        SoundBoard = 0,
        Raw = 1024,
        Wave = 2048,
        Au = 4096,
        Aiff = 8192,
        Alaw = 16384,
        Mulaw = 32768,
        Disabled = 65536
    }

    public enum MbrErr
    {
        NoMbrolaDll = -12,
        NoRegistry = -11,
        NoResource = -10,
        NoThread = -9,
        DatabaseNotValid = -8,
        CantOpenDeviceOut = -7,
        ErrorDeviceOut = -6,
        BadCommand = -5,
        CantOpenFile = -4,
        WriteError = -3,
        MbrolaError = -2,
        CancelledByUser = -1,
        NoError = 0
    }

    public enum AudioType : int
    {
        LIN16 = 0,
        LIN8,
        ULAW,
        ALAW
    }

    public enum StatePhone : byte
    {
        PHO_OK,
        PHO_EOF,
        PHO_FLUSH
    };

    public abstract class MbrParser
    {
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        delegate void reset_ParserFunction(ref MbrParser.Parser ps);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        delegate void close_ParserFunction(ref MbrParser.Parser ps);
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        delegate StatePhone nextphone_ParserFunction(ref MbrParser.Parser ps, ref IntPtr ph);

        [StructLayout(LayoutKind.Sequential)]
        struct Parser : IDisposable
        {
            public IntPtr self;
            public reset_ParserFunction reset;
            public close_ParserFunction close;
            public nextphone_ParserFunction nextphone;
            internal Parser(MbrParser mbrp)
            {
                reset = delegate(ref Parser ps)
                {
                    mbrp.Reset();
                };
                close = delegate(ref Parser ps)
                {
                    mbrp.Close();
                };
                nextphone = delegate(ref Parser ps, ref IntPtr ph)
                {
                    return mbrp.NextPhone(ref ph);
                };
                self = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Parser)));
                Marshal.StructureToPtr(this, self, false);
            }

            #region IDisposable Members

            public void Dispose()
            {
                Marshal.DestroyStructure(self, this.GetType());
            }

            #endregion
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "setParser_MBR", CharSet = CharSet.Ansi)]
        private static extern void setParser_MBR(IntPtr pParser);

        Parser myParser;

        protected MbrParser()
        {
            myParser = new Parser(this);
        }

        public void Register()
        {
            setParser_MBR(myParser.self);
        }

        public abstract void Reset();
        public abstract void Close();
        public abstract StatePhone NextPhone(ref IntPtr ph);

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "init_Phone", CharSet = CharSet.Ansi)]
        protected static extern IntPtr InitPhone(string phone, float fdur);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "reset_Phone", CharSet = CharSet.Ansi)]
        protected static extern void ResetPhone(IntPtr hPhoneme);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "close_Phone", CharSet = CharSet.Ansi)]
        protected static extern void ClosePhone(IntPtr hPhoneme);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "appendf0_Phone", CharSet = CharSet.Ansi)]
        protected static extern void Appendf0Phone(IntPtr hPhoneme, float percent, float pitch);
    }

    public class MbrolaException : Exception
    {
        public MbrolaException(string message)
            : base(message)
        {
        }
    }

    public enum MbrolaBinding
    {
        Library, Standalone
    }

    public sealed class Mbrola
    {
        static MbrolaBinding? binding;

        public static MbrolaBinding Binding
        {
            get
            {
                if (binding.HasValue)
                    return binding.Value;
                try
                {
                    int i = Mbrola.LastError_MBR();
                    return MbrolaBinding.Library;
                }
                catch
                {
                    return MbrolaBinding.Standalone;
                }
            }
            set
            {
                binding = value;
            }
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "init_MBR", CharSet = CharSet.Ansi)]
        private static extern int Init_MBR(string dbaname);

        public static void Init(string dbName)
        {
            if (Init_MBR(dbName) < 0)
                throw new MbrolaException(Mbrola.LastErrorString);

        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "init_rename_MBR", CharSet = CharSet.Ansi)]
        private static extern int InitRename_MBR(string dbaname, string replacing, string cloning);

        public static void InitRename(string dbName, string replacing, string cloning)
        {
            if (InitRename_MBR(dbName, replacing, cloning) < 0)
                throw new MbrolaException(Mbrola.LastErrorString);
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "write_MBR", CharSet = CharSet.Ansi)]
        public static extern int Write(string buffer_in);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "read_MBR", CharSet = CharSet.Ansi)]
        //public static extern int Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] short[] buffer_out, int nb_wanted);
        public static unsafe extern int Read(IntPtr buffer_out, int nb_wanted);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "readtype_MBR", CharSet = CharSet.Ansi)]
        public static extern int ReadType([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] short[] buffer_out, int nb_wanted, AudioType filter);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "close_MBR", CharSet = CharSet.Ansi)]
        public static extern void Close();
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "reset_MBR", CharSet = CharSet.Ansi)]
        public static extern void Reset();

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "lastError_MBR", CharSet = CharSet.Ansi)]
        private static extern int LastError_MBR();
        public static int LastError
        {
            get
            {
                return LastError_MBR();
            }
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "lastErrorStr_MBR", CharSet = CharSet.Ansi)]
        private static extern int LastErrorStr_MBR(StringBuilder buffer_err, int nb_wanted);
        public static string LastErrorString
        {
            get
            {
                StringBuilder sb = new StringBuilder(260);
                sb.EnsureCapacity(260);
                LastErrorStr_MBR(sb, sb.Capacity);
                return sb.ToString();
            }
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "setNoError_MBR", CharSet = CharSet.Ansi)]
        private static extern void setNoError_MBR(int noError);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "getNoError_MBR", CharSet = CharSet.Ansi)]
        private static extern int getNoError_MBR();

        public static bool NoErrorMode
        {
            get
            {
                return (getNoError_MBR() != 0);
            }
            set
            {
                setNoError_MBR(value ? (-1) : 0);
            }
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "flush_MBR", CharSet = CharSet.Ansi)]
        public static extern int Flush();

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "getVersion_MBR", CharSet = CharSet.Ansi)]
        private static extern int getVersion_MBR(StringBuilder buffer, int nb_wanted);
        public static string Version
        {
            get
            {
                StringBuilder sb = new StringBuilder(260);
                sb.EnsureCapacity(260);
                getVersion_MBR(sb, sb.Capacity);
                return sb.ToString();
            }
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "setFreq_MBR", CharSet = CharSet.Ansi)]
        private static extern void setFreq_MBR(int nFreq);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "getFreq_MBR", CharSet = CharSet.Ansi)]
        private static extern int getFreq_MBR();
        public static int Freq
        {
            get
            {
                return getFreq_MBR();
            }
            set
            {
                setFreq_MBR(value);
            }
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "setVolumeRatio_MBR", CharSet = CharSet.Ansi)]
        private static extern void setVolumeRatio_MBR(float fVol);
        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "getVolumeRatio_MBR", CharSet = CharSet.Ansi)]
        private static extern float getVolumeRatio_MBR();
        public static float VolumeRatio
        {
            get
            {
                return getVolumeRatio_MBR();
            }
            set
            {
                setVolumeRatio_MBR(value);
            }
        }

        [DllImport("Mbrola.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "getDatabaseInfo_MBR", CharSet = CharSet.Ansi)]
        private static extern int getDatabaseInfo_MBR(StringBuilder buffer, int nb_wanted, int idx);

        public static int DatabaseInfoCount
        {
            get
            {
                int i = 0;
                while (getDatabaseInfo_MBR(null, 0, i) > 0)
                    i++;
                return i;
            }
        }

        public static string GetDatabaseInfo(int idx)
        {
            int capacity = getDatabaseInfo_MBR(null, 0, idx) + 1;
            StringBuilder sb = new StringBuilder(capacity);
            sb.EnsureCapacity(capacity);
            getDatabaseInfo_MBR(sb, capacity, idx);
            return sb.ToString();
        }
    }

    public class MbrPlay
    {
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_MBRUnload")]
        public static extern void Unload();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_Play")]
        public static extern MbrErr Play([MarshalAs(UnmanagedType.LPStr)] string lpszText, int dwFlags, [MarshalAs(UnmanagedType.LPStr)] string lpszOutFile,
            [MarshalAs(UnmanagedType.FunctionPtr)]
            PlayCallbackProc dwCallback);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_Stop")]
        public static extern MbrErr Stop();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_WaitForEnd")]
        public static extern MbrErr WaitForEnd();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_SetPitchRatio")]
        public static extern MbrErr SetPitchRatio(float fPitch);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_SetDurationRatio")]
        public static extern MbrErr SetDurationRatio(float fDuration);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_SetVolumeRatio")]
        public static extern MbrErr SetVolumeRatio(float fVol);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_SetVoiceFreq")]
        public static extern MbrErr SetVoiceFreq(int lFreq);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetPitchRatio")]
        public static extern float GetPitchRatio();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetDurationRatio")]
        public static extern float GetDurationRatio();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetVoiceFreq")]
        public static extern int GetVoiceFreq();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetVolumeRatio")]
        public static extern float GetVolumeRatio();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_SetNoError")]
        public static extern MbrErr SetNoError(bool bNoError);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetNoError")]
        public static extern bool GetNoError();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_SetDatabase")]
        public static extern MbrErr SetDatabase([MarshalAs(UnmanagedType.LPStr)] string lpszID);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_SetDatabaseEx")]
        public static extern MbrErr SetDatabaseEx([MarshalAs(UnmanagedType.LPStr)] string lpszID,
                                                  [MarshalAs(UnmanagedType.LPStr)] string lpszRename,
                                                  [MarshalAs(UnmanagedType.LPStr)] string lpszCopy);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetDatabase")]
        public static extern MbrErr GetDatabase([MarshalAs(UnmanagedType.LPStr)] StringBuilder lpID, int dwSize);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_IsPlaying")]
        public static extern bool IsPlaying();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_LastError")]
        public static extern MbrErr LastError([MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszError, int dwSize);

        // Synthesizer general information

        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetVersion")]
        public static extern void GetVersion([MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszError, int dwSize);

        // Current database info

        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetDefaultFreq")]
        public static extern int GetDefaultFreq();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetDatabaseInfo")]
        public static extern MbrErr GetDatabaseInfo(int idx, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpMsg, int dwSize);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_GetDatabase")]
        public static extern MbrErr GetDatabaseAllInfo([MarshalAs(UnmanagedType.LPStr)] StringBuilder lpMsg, int dwSize);

        // Registry related functions
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegEnumDatabase")]
        public static extern MbrErr RegEnumDatabase([MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszData, int dwSize);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegEnumDatabaseCallback")]
        public static extern MbrErr RegEnumDatabaseCallback([MarshalAs(UnmanagedType.FunctionPtr)] EnumDatabaseCallback lpedCallback, int dwUserData);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegGetDatabaseLabel")]
        public static extern MbrErr RegGetDatabaseLabel([MarshalAs(UnmanagedType.LPStr)] string lpszID, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpLabel, int dwSize);

        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegGetDatabasePath")]
        private static extern MbrErr _RegGetDatabasePath([MarshalAs(UnmanagedType.LPStr)] string lpszID, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpPath, int dwSize);

        public static string RegGetDatabasePath(string dbID)
        {
            StringBuilder sb = new StringBuilder(260);
            _RegGetDatabasePath(dbID, sb, sb.Capacity);
            return sb.ToString();
        }

        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegGetDatabaseCount")]
        public static extern int RegGetDatabaseCount();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegGetDefaultDatabase")]
        public static extern MbrErr RegGetDefaultDatabase([MarshalAs(UnmanagedType.LPStr)] StringBuilder lpID, int dwSize);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegSetDefaultDatabase")]
        public static extern MbrErr RegSetDefaultDatabase([MarshalAs(UnmanagedType.LPStr)] string lpszID);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegisterDatabase")]
        public static extern bool RegisterDatabase([MarshalAs(UnmanagedType.LPStr)] string dbId,
                                                   [MarshalAs(UnmanagedType.LPStr)] string dbPath,
                                                   [MarshalAs(UnmanagedType.LPStr)] string dbLabel,
                                                   bool isDef,
                                                   [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpBuffer,
                                                   int dwSize);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_UnregisterDatabase")]
        public static extern bool UnregisterDatabase([MarshalAs(UnmanagedType.LPStr)] string dbId);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_UnregisterAll")]
        public static extern bool UnregisterAll();
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_DatabaseExist")]
        public static extern bool DatabaseExist([MarshalAs(UnmanagedType.LPStr)] string lpszID);

        // Registry related functions, databases accessed by index
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegIdxGetDatabaseId")]
        public static extern bool RegIdxGetDatabaseId(int nIdx, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszId, int dwSize);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegIdxGetDatabasePath")]
        public static extern bool RegIdxGetDatabasePath(int nIdx, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszPath, int dwSize);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegIdxGetDatabaseLabel")]
        public static extern bool RegIdxGetDatabaseLabel(int nIdx, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpszLabel, int dwSize);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegIdxGetDatabaseIndex")]
        public static extern int RegIdxGetDatabaseIndex([MarshalAs(UnmanagedType.LPStr)] string lpszID);
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_RegIdxGetDefaultDatabase")]
        public static extern int RegIdxGetDefaultDatabase();

        // Control panel access
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_StartControlPanel")]
        public static extern MbrErr StartControlPanel(int hParent);

        // Callback message base
        [DllImport("MbrPlay.Dll", EntryPoint = "MBR_SetCallbackMsgBase")]
        public static extern MbrErr SetCallbackMsgBase(int dwBase);




    }
}

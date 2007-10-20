using System;
using System.Collections.Generic;
using System.Text;

namespace MotiZilberman
{
    [Flags]
    public enum LogLevel
    {
        Debug=1,
        Info=2,
        MajorInfo=4,
        Warning=8,
        Error=16,

        All=Debug|Info|MajorInfo|Warning|Error
    }

    public delegate void LogLineHandler(ILogSource sender, string message, LogLevel visibility);
    public interface ILogSource
    {
        string Name
        {
            get;
        }
        event LogLineHandler LogLine;
    }
}

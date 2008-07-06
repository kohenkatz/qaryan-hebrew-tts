using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;

namespace MotiZilberman
{
    [Flags]
    public enum LogLevel
    {
        Debug = 1,
        Info = 2,
        MajorInfo = 4,
        Warning = 8,
        Error = 16,

        All = Debug | Info | MajorInfo | Warning | Error
    }

    public delegate void LogLineHandler(ILogSource sender, string message, LogLevel visibility);
    public delegate void TimedLogLineHandler(ILogSource sender, DateTime time, string message, LogLevel visibility);
    public interface ILogSource
    {
        string Name
        {
            get;
        }
        event LogLineHandler LogLine;
    }

    public abstract class Logger
    {
        public void Add(ILogSource source)
        {
            source.LogLine += this.OnLogLine;
            if (!Sources.Contains(source))
                Sources.Add(source);
        }

        public void Remove(ILogSource source)
        {
            source.LogLine -= this.OnLogLine;
        }

        protected List<ILogSource> Sources = new List<ILogSource>();
        protected abstract void OnLogLine(ILogSource source, string message, LogLevel level);
    }

}

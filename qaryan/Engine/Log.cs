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

    //public class FilterLogger : Logger
    //{
    //    struct LogMessage
    //    {
    //        public DateTime time;
    //        public ILogSource source;
    //        public string message;
    //        public LogLevel level;
    //    }

    //    public LogLevel LogMask = LogLevel.All;
    //    public event TimedLogLineHandler LogLine;

    //    Queue<LogMessage> messages;

    //    protected override void OnLogLine(ILogSource source, string message, LogLevel level)
    //    {
    //        if ((level & LogMask) != level)
    //            return;
    //        LogMessage msg = new LogMessage();
    //        msg.time = DateTime.Now;
    //        msg.source = source;
    //        msg.message = message;
    //        msg.level = level;
    //        messages.Enqueue(msg);
    //    }

    //    Thread myThread;

    //    public FilterLogger()
    //    {
    //        messages = new Queue<LogMessage>();
    //        myThread = new Thread(ThreadFunc);
    //        myThread.Start();
    //    }

    //    void ThreadFunc()
    //    {
    //        while (Thread.CurrentThread.IsAlive)
    //        {
    //            if ((messages.Count > 0) && (LogLine != null))
    //            {
    //                LogMessage msg = messages.Dequeue();
    //                LogLine(msg.source,msg.time,msg.message,msg.level);
    //            }
    //            else
    //                Thread.Sleep(0);
    //        }
    //    }
    //}

    //public class XmlLogger : Logger
    //{
    //    XmlWriter writer;

    //    public XmlWriter Writer
    //    {
    //        get
    //        {
    //            return writer;
    //        }
    //        set
    //        {
    //            writer = value;
    //        }
    //    }

    //    bool started = false;

    //    public void StartFile(string filename)
    //    {
    //        writer = XmlWriter.Create(filename);
    //        Start();
    //    }

    //    public void Start()
    //    {
    //        if (writer == null)
    //            return;
    //        if (!started)
    //        {
    //            writer.WriteStartDocument();
    //            writer.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"resx:///?QaryanLog_xsl\"");
    //            writer.WriteStartElement("Log");
    //        }
    //        started = true;
    //    }

    //    public void Stop()
    //    {
    //        if (writer == null)
    //            return;
    //        if (started)
    //        {
    //            writer.WriteEndDocument();
    //            writer.Close();
    //        }
    //        started = false;
    //    }

    //    protected override void OnLogLine(ILogSource source, string message, LogLevel level)
    //    {
    //        if (writer == null)
    //            return;
    //        lock (writer)
    //        {
    //            if ((writer.WriteState == WriteState.Closed) || (writer.WriteState == WriteState.Error))
    //                return;
    //            //if (!started)
    //            //Start();

    //            writer.WriteStartElement("LogLine");
    //            writer.WriteAttributeString("Time", DateTime.Now.ToLocalTime().ToString());
    //            writer.WriteAttributeString("Component", source.Name);
    //            writer.WriteAttributeString("ComponentNum", Sources.IndexOf(source).ToString());
    //            writer.WriteAttributeString("Level", ((int)level).ToString());
    //            writer.WriteAttributeString("TextLevel", Enum.GetName(typeof(LogLevel), level));
    //            writer.WriteValue(message);
    //            writer.WriteEndElement();
    //        }
    //    }
    //}
}

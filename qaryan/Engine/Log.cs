using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

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
    public interface ILogSource
    {
        string Name
        {
            get;
        }
        event LogLineHandler LogLine;
    }

    public class XmlLogger
    {
        XmlWriter writer;

        public XmlWriter Writer
        {
            get
            {
                return writer;
            }
            set
            {
                writer = value;
            }
        }

        bool started = false;

        public void StartFile(string filename)
        {
            writer = XmlWriter.Create(filename);
            Start();
        }

        public void Start()
        {
            if (writer == null)
                return;
            if (!started)
            {
                writer.WriteStartDocument();
                writer.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"QaryanLog.xslt\"");
                writer.WriteStartElement("Log");
            }
            started = true;
        }

        public void Stop()
        {
            if (writer == null)
                return;
            if (started)
            {
                writer.WriteEndDocument();
                writer.Close();
            }
            started = false;
        }

        public void Add(ILogSource source)
        {
            source.LogLine += this.LogLine;
            if (!Sources.Contains(source))
                Sources.Add(source);
        }

        public void Remove(ILogSource source)
        {
            source.LogLine -= this.LogLine;
        }

        List<ILogSource> Sources = new List<ILogSource>();

        protected void LogLine(ILogSource source, string message, LogLevel level)
        {
            if (writer == null)
                return;
            lock (writer)
            {
                if ((writer.WriteState == WriteState.Closed) || (writer.WriteState == WriteState.Error))
                    return;
                //if (!started)
                //Start();

                writer.WriteStartElement("LogLine");
                writer.WriteAttributeString("Time", DateTime.Now.ToLocalTime().ToString());
                writer.WriteAttributeString("Component", source.Name);
                writer.WriteAttributeString("ComponentNum", Sources.IndexOf(source).ToString());
                writer.WriteAttributeString("Level", ((int)level).ToString());
                writer.WriteAttributeString("TextLevel", Enum.GetName(typeof(LogLevel), level));
                writer.WriteValue(message);
                writer.WriteEndElement();
            }
        }
    }
}

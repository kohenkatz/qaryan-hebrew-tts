using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Qaryan.Core
{
    public sealed class FileBindings
    {
        static string enginePath = Environment.CurrentDirectory;

        public static string EnginePath
        {
            set
            {
                enginePath = value;
            }
            get
            {
                return enginePath;
            }
        }

        public static string StressHeuristicsPath
        {
            get
            {
                return Path.Combine(EnginePath, "StressHeuristics.xml");
            }
        }

        public static string VoicePath
        {
            get
            {
                return Path.Combine(EnginePath, "Voices");
            }
        }
    }
}

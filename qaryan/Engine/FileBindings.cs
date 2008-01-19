using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Qaryan.Core
{
    /// <summary>
    /// A collection of "globals" holding various essential paths.
    /// </summary>
    public sealed class FileBindings
    {
        static string enginePath = Environment.CurrentDirectory;

        /// <summary>
        /// The path where Qaryan's core files are found.
        /// </summary>
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

        /// <summary>
        /// The path to <c>StressHeuristics.xml</c>.
        /// </summary>
        public static string StressHeuristicsPath
        {
            get
            {
                return Path.Combine(EnginePath, "StressHeuristics.xml");
            }
        }

        /// <summary>
        /// The path to Qaryan's voice definition files.
        /// </summary>
        public static string VoicePath
        {
            get
            {
                return Path.Combine(EnginePath, "Voices");
            }
        }
    }
}

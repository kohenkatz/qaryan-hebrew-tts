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

using System;

namespace Qaryan.Audio
{
    /// <summary>
    ///     Contains constants that describe waveform-audio format types.
    /// </summary>
    public enum WaveFormatTag
    {
        /// <summary>
        /// PCM
        /// </summary>
        Pcm = 1
    }

    /// <summary>
    /// A structure compatible with Microsoft's DirectSound wave format structure, duplicated so as to remove the dependency on the MDX assemblies.
    /// </summary>
    public struct WaveFormat
    {
        /// <summary>
        ///     Retrieves and sets the required average data-transfer rate, in bytes per
        ///     second, for the format type.
        /// </summary>
        public uint AverageBytesPerSecond;

        /// <summary>
        ///     Retrieves and sets the bits per sample for the format type.
        /// </summary>
        public ushort BitsPerSample;

        /// <summary>
        ///     Retrieves and sets the minimum atomic unit of data, in bytes, for the format
        ///     type.
        /// </summary>
        public ushort BlockAlign;

        /// <summary>
        ///     Retrieves and sets the number of channels in the waveform-audio data, for
        ///     the format type.
        /// </summary>
        public ushort Channels;

        /// <summary>
        ///     Retrieves and sets the waveform-audio format type, for the format type.
        /// </summary>
        public WaveFormatTag FormatTag;

        /// <summary>
        ///     Retrieves and sets the sample rate, in samples per second (hertz), for the
        ///     format type.
        /// </summary>
        public uint SamplesPerSecond;

    }

    /// <summary>
    /// An audio buffer carrying format information.
    /// </summary>
    public struct AudioBufferInfo
    {
        public WaveFormat Format;
        public byte[] Data;
    }

}
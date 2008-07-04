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
//
//    Original WaveFileReader class by Jonathan Kade
//    <http://www.codeproject.com/KB/audio-video/WaveEdit.aspx>

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace Qaryan.Audio
{
    public class WaveFile
    {
        /// <summary>
        /// A class for loading into memory and manipulating wave file data.
        /// </summary>
        public class riffChunk
        {
            public string FileName;
            //These three fields constitute the riff header
            public string sGroupID;         //RIFF
            public uint dwFileLength;		//In bytes, measured from offset 8
            public string sRiffType;        //WAVE, usually
        }

        public class fmtChunk
        {
            public string sChunkID;    	//Four bytes: "fmt "
            public uint dwChunkSize;     //Length of header
            public ushort wFormatTag;  	//1 if uncompressed
            public ushort wChannels;       //Number of channels: 1-5
            public uint dwSamplesPerSec; //In Hz
            public uint dwAvgBytesPerSec;//For estimating RAM allocation
            public ushort wBlockAlign;     //Sample frame size in bytes
            public uint dwBitsPerSample; //Bits per sample
            /* More data can be contained in this chunk; specifically
             * the compression format data.  See MS website for this.
             */
        }

        /* The fact chunk is used for specifying the compression
           ratio of the data */
        public class factChunk
        {
            public string sChunkID;    		//Four bytes: "fact"
            public uint dwChunkSize;    	//Length of header
            public uint dwNumSamples;    	//Number of audio frames;
            //numsamples/samplerate should equal file length in seconds.
        }

        public class dataChunk
        {
            public string sChunkID;    		//Four bytes: "data"
            public uint dwChunkSize;    	//Length of header

            //The following non-standard fields were created to simplify
            //editing.  We need to know, for filestream seeking purposes,
            //the beginning file position of the data chunk.  It's useful to
            //hold the number of samples in the data chunk itself.  Finally,
            //the minute and second length of the file are useful to output
            //to XML.
            public long lFilePosition;	//Position of data chunk in file
            public uint dwMinLength;		//Length of audio in minutes
            public double dSecLength;		//Length of audio in seconds
            public uint dwNumSamples;		//Number of audio frames
            //Different arrays for the different frame sizes
            //public byte  [] byteArray; 	//8 bit - unsigned
            //public short [] shortArray;    //16 bit - signed
        }

        public riffChunk maindata;
        public fmtChunk format;
        public factChunk fact;
        public dataChunk data;
    }

    /// <summary>
    /// This class gives you repurposable read/write access to a wave file.
    /// </summary>
    class WaveFileReader : IDisposable
    {
        BinaryReader reader;
        WaveFile contents;
        public WaveFile Contents
        {
            get
            {
                return contents;
            }
        }

        WaveFile.riffChunk mainfile;
        WaveFile.fmtChunk format;
        WaveFile.factChunk fact;
        WaveFile.dataChunk data;

        #region General Utilities
        /*
		 * WaveFileReader(string) - 2004 July 28
		 * A fairly standard constructor that opens a file using the filename supplied to it.
		 */
        public WaveFileReader(string filename)
        {
            reader = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read));
            contents = new WaveFile();
            contents.maindata = this.ReadMainFileHeader();
            contents.maindata.FileName = filename;

            while (this.GetPosition() < (long)contents.maindata.dwFileLength)
            {
                string temp = this.GetChunkName();
                if (temp == "fmt ")
                {
                    contents.format = this.ReadFormatHeader();
                    if (this.GetPosition() +
                      contents.format.dwChunkSize ==
                      contents.maindata.dwFileLength)
                        break;
                }
                else if (temp == "fact")
                {
                    contents.fact = this.ReadFactHeader();
                    if (this.GetPosition() +
                      contents.fact.dwChunkSize ==
                      contents.maindata.dwFileLength)
                        break;
                }
                else if (temp == "data")
                {
                    contents.data = this.ReadDataHeader();
                    if (this.GetPosition() +
                      contents.data.dwChunkSize ==
                      contents.maindata.dwFileLength)
                        break;
                }
                else
                {    //This provides the required skipping of unsupported chunks.

                    this.AdvanceToNext();
                }
            }
            reader.Close();
        }

        /*
         * long GetPosition() - 2004 July 28
         * Returns the current position of the reader's BaseStream.
         */
        public long GetPosition()
        {
            return reader.BaseStream.Position;
        }

        /*
         * string GetChunkName() - 2004 July 29
         * Reads the next four bytes from the file, converts the 
         * char array into a string, and returns it.
         */
        public string GetChunkName()
        {
            return new string(reader.ReadChars(4));
        }

        /*
         * void AdvanceToNext() - 2004 August 2
         * Advances to the next chunk in the file.  This is fine, 
         * since we only really care about the fmt and data 
         * streams for now.
         */
        public void AdvanceToNext()
        {
            long NextOffset = (long)reader.ReadUInt32(); //Get next chunk offset
            //Seek to the next offset from current position
            reader.BaseStream.Seek(NextOffset, SeekOrigin.Current);
        }
        #endregion
        #region Header Extraction Methods
        /*
		 * WaveFileFormat ReadMainFileHeader - 2004 July 28
		 * Read in the main file header.  Not much more to say, really.
		 * For XML serialization purposes, I "correct" the dwFileLength
		 * field to describe the whole file's length.
		 */
        public WaveFile.riffChunk ReadMainFileHeader()
        {
            mainfile = new WaveFile.riffChunk();

            mainfile.sGroupID = new string(reader.ReadChars(4));
            mainfile.dwFileLength = reader.ReadUInt32() + 8;
            mainfile.sRiffType = new string(reader.ReadChars(4));
            return mainfile;
        }

        //fmtChunk ReadFormatHeader() - 2004 July 28
        //Again, not much to say.
        public WaveFile.fmtChunk ReadFormatHeader()
        {
            format = new WaveFile.fmtChunk();

            format.sChunkID = "fmt ";
            format.dwChunkSize = reader.ReadUInt32();
            format.wFormatTag = reader.ReadUInt16();
            format.wChannels = reader.ReadUInt16();
            format.dwSamplesPerSec = reader.ReadUInt32();
            format.dwAvgBytesPerSec = reader.ReadUInt32();
            format.wBlockAlign = reader.ReadUInt16();
            format.dwBitsPerSample = reader.ReadUInt32();
            return format;
        }

        //factChunk ReadFactHeader() - 2004 July 28
        //Again, not much to say.
        public WaveFile.factChunk ReadFactHeader()
        {
            fact = new WaveFile.factChunk();

            fact.sChunkID = "fact";
            fact.dwChunkSize = reader.ReadUInt32();
            fact.dwNumSamples = reader.ReadUInt32();
            return fact;
        }


        //dataChunk ReadDataHeader() - 2004 July 28
        //Again, not much to say.
        public WaveFile.dataChunk ReadDataHeader()
        {
            data = new WaveFile.dataChunk();

            data.sChunkID = "data";
            data.dwChunkSize = reader.ReadUInt32();
            data.lFilePosition = reader.BaseStream.Position;
            if (fact!=null)
                data.dwNumSamples = fact.dwNumSamples;
            else
                data.dwNumSamples = data.dwChunkSize / (format.dwBitsPerSample / 8 * format.wChannels);
            //The above could be written as data.dwChunkSize / format.wBlockAlign, but I want to emphasize what the frames look like.
            data.dwMinLength = (data.dwChunkSize / format.dwAvgBytesPerSec) / 60;
            data.dSecLength = ((double)data.dwChunkSize / (double)format.dwAvgBytesPerSec) - (double)data.dwMinLength * 60;
            return data;
        }
        #endregion
        #region IDisposable Members

        public void Dispose()
        {
            if (reader != null)
                reader.Close();
        }

        #endregion


    }
}

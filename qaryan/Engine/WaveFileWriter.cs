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
//    Original WaveFileWriter class by Colin Savage
//    <http://dotnet.org.za/colin/archive/2006/09/22/Using-DirectSound-to-record-PCM-data.aspx>

using System;
using System.IO;

namespace Qaryan.Audio
{
	class WaveFileWriter
	{
		private Stream _stream;
		private BinaryWriter _binaryWriter;
		private uint _dataSize = 0;
		private const uint HEADERSIZE = 8;
		private const uint FORMATDATASIZE = 18;
		private const uint WAVESIZE = 4;
		private const short FORMAT = 1;

        bool WriteHeader = true;

		public WaveFileWriter(Stream stream,ushort channels,uint samplesPerSecond,uint averageBytesPerSecond,ushort blockAlign,ushort bitsPerSample,bool writeHeader)
		{
			_stream = stream;
			_binaryWriter = new BinaryWriter(stream,System.Text.Encoding.ASCII);
            
            WriteHeader = writeHeader;

			WriteRiffHeader();
			WriteFormatHeader(channels,samplesPerSecond,averageBytesPerSecond,blockAlign,bitsPerSample);
		}

		private void WriteRiffHeader()
		{
            if (!WriteHeader)
                return;
			_binaryWriter.Seek(0,SeekOrigin.Begin);
			_binaryWriter.Write(0x46464952);//RIFF
			_binaryWriter.Write(WAVESIZE + HEADERSIZE + FORMATDATASIZE + HEADERSIZE + _dataSize);
			_binaryWriter.Write(0x45564157);//WAVE
		}

		private void WriteFormatHeader(ushort channels,uint samplesPerSecond,uint averageBytesPerSecond,ushort blockAlign,ushort bitsPerSample)
		{
            if (!WriteHeader)
                return;
			_binaryWriter.Seek(12,SeekOrigin.Begin);
			_binaryWriter.Write(0x20746D66);//fmt 4
			_binaryWriter.Write(FORMATDATASIZE);
			_binaryWriter.Write(FORMAT);
			_binaryWriter.Write(channels);
			_binaryWriter.Write(samplesPerSecond);
			_binaryWriter.Write(averageBytesPerSecond);
			_binaryWriter.Write(blockAlign);
			_binaryWriter.Write(bitsPerSample);
		}

		public void WriteData(byte[] data,int index, int count)
		{
            if (WriteHeader)
            {
                _dataSize += (uint)count;

                WriteRiffHeader();
                WriteDataHeader();
            }
			_binaryWriter.Seek(0,SeekOrigin.End);
			_binaryWriter.Write(data,index,count);
			
		}

		private void WriteDataHeader()
		{
            if (!WriteHeader)
                return;
			_binaryWriter.Seek(38,SeekOrigin.Begin);
			_binaryWriter.Write(0x61746164);//data
			_binaryWriter.Write(_dataSize);			
		}

		public void Close()
		{
            Flush();
			_binaryWriter.Close();
			_stream.Close();
		}

        public void Flush()
        {
            try
            {
                _binaryWriter.Flush();
                _stream.Flush();
            }
            catch (ObjectDisposedException)
            {
            }
        }
	}
}

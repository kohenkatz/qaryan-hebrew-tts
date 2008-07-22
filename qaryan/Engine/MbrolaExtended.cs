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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Media;
using Qaryan.Audio;

namespace MBROLA
{

    /// <summary>
    /// Provides a way to retrieve information from MBROLA diphone databases directly, without depending on MBROLA itself.
    /// Members in this class are experimental and should be considered highly unstable.
    /// </summary>
	public class DiphoneDB {
        public static WaveFormat WaveFormatFromFile(string filename)
        {
            FileStream fs=File.OpenRead(filename);
            DiphoneDB db = new DiphoneDB();
            db.ReadFromStream(fs);
            fs.Close();
            WaveFormat result = new WaveFormat();
            result.FormatTag = WaveFormatTag.Pcm;
            result.BitsPerSample = 16;
            result.Channels = 1;
            result.SamplesPerSecond = db.Rate;
            result.AverageBytesPerSecond = result.SamplesPerSecond * result.BitsPerSample / 8;
            result.BlockAlign = (ushort)(result.Channels * (result.BitsPerSample / 8));
            return result;
        }

		public struct Header {
			public string Software;
			public string Version;
			public uint Count;
			public int i1,i2;
			public ushort Rate;
			public byte FrameShift;
			public byte b1;
		};
		
		public class DiphoneHeader {
			public uint Offset;
			public string Phone1, Phone2;
			public byte b1,b2,b3,b4;
			public uint Address;
			public uint Size {
				get {
					return (uint)(b3+1)*256/* + b3*/;
				}
			}
			public uint PaddedSize {
				get {
					//return ((uint)Math.Ceiling((double)(b4*256+b3)/100))*100+60;
					return  Size;
				}
			}
			public uint AlignedSize;
			public override string ToString()
			{
				return Phone1+"-"+Phone2;
			}
		}
		
		Header header;
		
		public ushort Rate {
			get {
				return header.Rate;
			}
			set {
				header.Rate=value;
			}
		}
		
		public List<DiphoneHeader> diphoneHeaders;
		
		public string Version {
			get { return header.Version; }
			set { header.Version=value; }
		}
		
		public string Software {
			get { return header.Software; }
			set { header.Software=value; }
		}
		
		public uint Count {
			get { return header.Count; }
		}
		
		public uint sample_offset;
		
		/// <summary>
		/// Read an AsciiZ string in from the binary reader
		/// </summary>
		/// <param name="Reader">Binary reader instance</param>
		/// <returns>String, null terminator is truncted,
		/// stream reader positioned at byte after null</returns>
		public static string ReadStringZ(BinaryReader reader) {
			StringBuilder sb=new StringBuilder();
			//string result = "";
			char c;
			for (int i = 0; i < reader.BaseStream.Length; i++) {
				if ((c = (char) reader.ReadByte()) == 0) {
					break;
				}
				sb.Append(c.ToString());
			}
			return sb.ToString();
		}
		
		public int i_min=int.MaxValue,j_min=int.MaxValue;
		
		DiphoneHeader ReadDiphone(BinaryReader br) {
			DiphoneHeader result=new DiphoneHeader();
			result.Phone1=ReadStringZ(br);
			result.Phone2=ReadStringZ(br);
			/*			int i=br.ReadInt16(),j=br.ReadInt16();
			if (i<i_min)
				i_min=i;
			if (j<j_min)
				j_min=j;
			Console.WriteLine(String.Format("{0}->{1} {2:x} {3:x}",phone1,phone2,i,j));*/
			result.b1=br.ReadByte();
			result.b2=br.ReadByte();
			result.b3=br.ReadByte();
			result.b4=br.ReadByte();
			result.Offset=(uint)br.BaseStream.Position;
			return result;
			//	diphones++;
		}
		
		void WriteDiphone(DiphoneHeader dh,BinaryWriter bw) {
			bw.Write(dh.Phone1.ToCharArray());
			bw.Write((byte)0);
			bw.Write(dh.Phone2.ToCharArray());
			bw.Write((byte)0);
			bw.Write(new byte[4] {dh.b1,dh.b2,dh.b3,dh.b4});
		}
		
		public void ReadFromStream(Stream stream) {
			header=new Header();
//			header.Count=0;
			BinaryReader br=new BinaryReader(stream,ASCIIEncoding.ASCII);
			header.Software=ASCIIEncoding.ASCII.GetString(br.ReadBytes(6));
			header.Version=ASCIIEncoding.ASCII.GetString(br.ReadBytes(5));
//			int i=br.ReadInt32(),j=br.ReadInt16();
			header.Count=br.ReadUInt32();
			header.i1=br.ReadInt32();
			header.i2=br.ReadInt32();
			header.Rate=br.ReadUInt16();
			header.FrameShift=br.ReadByte();
			header.b1=br.ReadByte();
			diphoneHeaders=new List<DiphoneHeader>();
			
			uint offset=14799;
			sample_offset=offset;
			for (int x=0;x<header.Count;x++) {
				DiphoneHeader dh=ReadDiphone(br);
				dh.Address=offset;
				//dh.AlignedSize=dh.Size;//+dh.Address%60+8*((uint)x+1);
				diphoneHeaders.Add(dh);
				//offset+=(uint)(dh.Size+(dh.b4*4+(dh.b4-dh.b3-1)*256));
				offset+=(uint)dh.b4*260;
			}
			//Console.WriteLine("Headers end at "+stream.Position);
			/*			sample_offset=14799;
			for(int i=0;i<diphoneHeaders.Count;i++) {
				DiphoneHeader dih=diphoneHeaders[i];
				dih.Address=sample_offset+dih.Address;
			}*/
			//br.Close();
		}
		
		public void WriteToStream(Stream stream) {
			BinaryWriter bw=new BinaryWriter(stream,ASCIIEncoding.ASCII);
			header.Count=(uint)diphoneHeaders.Count;
			bw.Write(header.Software.ToCharArray());
			bw.Write(header.Version.ToCharArray());
			bw.Write(header.Count);
			bw.Write(header.i1);
			bw.Write(header.i2);
			bw.Write(header.Rate);
			bw.Write(header.FrameShift);
			bw.Write(header.b1);
			foreach(DiphoneHeader dh in diphoneHeaders) {
				WriteDiphone(dh,bw);
			}
			//bw.Close();
		}
		
		public DiphoneHeader GetDiphoneHeader(string phone1,string phone2) {
			return diphoneHeaders.Find(delegate (DiphoneHeader dh) {
			                           	return (dh.Phone1==phone1)&&(dh.Phone2==phone2);
			                           });
			
		}
	}
}

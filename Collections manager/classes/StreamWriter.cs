using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_manager.classes
{
	class StreamWriter
	{
		public static List<byte> data = new List<byte>();
		public static long currentPos;

		public static void writeBytes(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				data.Add(bytes[i]);
				currentPos += 1;
			}
		}
		public static void writeByte(byte input)
		{
			byte[] t = new byte[1];
			t[0] = input;
			writeBytes(t);
		}
		//only need string and int for collection.db
		public static void writeInt(int value)
		{
			byte[] intBytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
				Array.Reverse(intBytes);
			writeBytes(intBytes);
		}
		public static void writeULEB128(long input)
		{
			writeBytes(encodeULEB128(input));
		}
		public static byte[] encodeULEB128(long Value)
		{
			List<byte> p = new List<byte>();
			List<byte> orig_p = p;
			do
			{
				byte Byte = (byte)(Value & 0x7f);
				Value >>= 7;
				if (Value != 0)
					Byte |= 0x80; // Mark this byte to show that more bytes will follow.
				p.Add(Byte);
			} while (Value != 0);
			return p.ToArray();
		}
		public static void writeString(string input)
		{
			//write 0x0b then ULEB128 as length then the string
			writeByte(0x0b);
			writeULEB128(input.Length);
			writeBytes(Encoding.UTF8.GetBytes(input));
		}
		public static void flushFile(string filename)
		{
			File.WriteAllBytes(filename,data.ToArray());
		}
		public static void flushFileR(string filename)
		{
			File.WriteAllBytes(filename, data.ToArray());
			data.Clear();
			currentPos = 0;
		}
	}
}

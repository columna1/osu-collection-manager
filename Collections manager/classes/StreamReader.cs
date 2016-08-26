using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_manager.classes
{
    class StreamReader
    {
		public static bool ready = true; // Check if file is currently being read.
		public static byte[] fileData; // Now you can read these from outside the readOsuDB function.
		public static long currentPos;

		public static byte[] getBytes (long length)
		{
			byte[] currentData = new byte[length];
			for (int i = 0; i < length; i++)
			{
				currentData[i] = fileData[currentPos + i];
			}
			currentPos += length;
			return currentData;
		}
		public static byte readByte()
		{
			return getBytes(1)[0];
		}

		public static bool readBool()
		{
			return readByte() >= 1;
		}

		public static short readShort()
		{
			byte[] data = getBytes(2);
			return (short)(data[0] + (data[1] << 8));
		}

        public static int readInt()
		{ 
			byte[] data = getBytes(4);
			return data[0] + (data[1] << 8) + (data[2] << 16) + (data[3] << 24);
		}

		public static byte[] reverseByteOrder ( byte[] bytes )
		{
			byte[] nBytes = new byte[bytes.Count()];
			for (int i=0; i<bytes.Count(); i++)
			{
				nBytes[i] = bytes[bytes.Count() - i-1];
			}
			return nBytes;
		}

        public static long readLong()
        {
            byte[] data = getBytes(8); // Hmm.
            //data = reverseByteOrder(data);
            long byte1 = (long)(data[0]);
            long byte2 = (long)(data[1]) << 8;
            long byte3 = (long)(data[2]) << 16;
            long byte4 = (long)(data[3]) << 24;
            long byte5 = (long)(data[4]) << 32;
            long byte6 = (long)(data[5]) << 40;
            long byte7 = (long)(data[6]) << 48;
            long byte8 = (long)(data[7]) << 56;
            long returnval = byte1 + byte2 + byte3 + byte4 + byte5 + byte6 + byte7 + byte8;
            return returnval;
        }

        public static string readString()
		{
			if (readByte() == 11) {
				byte[] buffer = getBytes(readULEB128());
				return System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
			}
			else
				return "";
		}
		public static float readFloat()
		{
			return System.BitConverter.ToSingle(getBytes(4), 0);
		}
		public static double readDouble()
		{
			return System.BitConverter.ToDouble(getBytes(8), 0);
		}

		public static long readULEB128()
		{
			long result = 0;
			int shift = 0;
			while (true)
			{
				byte curByte = readByte();
				byte bit7 = (byte)(curByte & 0x80);
				//Console.WriteLine(bit7);
				int tmp = 0;
				if (bit7 == 0)
					tmp = curByte;
				else
				{
					tmp = curByte - 128;
					//Console.WriteLine("negated from " + curByte + " to " + tmp);
				}

				//Console.WriteLine("old " + result);
				result = (tmp << shift) + result;
				//Console.WriteLine("new " + result);
				if (bit7 == 0)
					break;
				shift = shift + 7;
			}
			return result;
		}
	}
}

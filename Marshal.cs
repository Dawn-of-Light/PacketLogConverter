/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using System.Text;

namespace PacketLogConverter
{
	/// <summary>
	/// Provides basic functionality to convert data types
	/// </summary>
	public sealed class Marshal
	{
		/// <summary>
		/// Converts a byte c-style string byte-array 
		/// to a c# string
		/// </summary>
		/// <param name="cstyle">the bytes</param>
		/// <returns>the string</returns>
		public static string ConvertToString(byte[] cstyle)
		{
			if(cstyle==null)
				return null;

			for (int i = 0; i < cstyle.Length; i++)
			{
				if (cstyle[i] == 0)
					return Encoding.Default.GetString(cstyle, 0, i);
			}
			return Encoding.Default.GetString(cstyle);
		}

		/// <summary>
		/// Converts 8 bytes to an long value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <returns>the long value</returns>
		public static long ConvertToInt64(byte[] val)
		{
			return ConvertToInt64(val, 0);
		}

		/// <summary>
		/// Converts 8 bytes to an long value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <param name="startIndex">where to read the values from</param>
		/// <returns>the long value</returns>
		public static long ConvertToInt64(byte[] val, int startIndex)
		{
			return ConvertToInt64(val[startIndex], val[startIndex+1], val[startIndex+2], val[startIndex+3], val[startIndex+4], val[startIndex+5], val[startIndex+6], val[startIndex+7]);
		}

		/// <summary>
		/// Converts 8 bytes to an long value
		/// in high to low order
		/// </summary>
		/// <param name="v1">the first bytes</param>
		/// <param name="v2">the second bytes</param>
		/// <param name="v3">the third bytes</param>
		/// <param name="v4">the fourth bytes</param>
		/// <returns>the long value</returns>
		public static long ConvertToInt64(byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7, byte v8)
		{
			long result = v1;
			result = result << 8 | v2;
			result = result << 8 | v3;
			result = result << 8 | v4;
			result = result << 8 | v5;
			result = result << 8 | v6;
			result = result << 8 | v7;
			result = result << 8 | v8;
			return result;
//			return (long)((v1 << 56) | (v2 << 48) | (v3 << 40) | (v4 << 32) | (v5 << 24) | (v6 << 16) | (v7 << 8) | v8);
		}

		/// <summary>
		/// Converts 8 bytes to an unsigned long value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <returns>the long value</returns>
		public static ulong ConvertToUInt64(byte[] val)
		{
			return ConvertToUInt64(val, 0);
		}

		/// <summary>
		/// Converts 8 bytes to an unsigned long value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <param name="startIndex">where to read the values from</param>
		/// <returns>the long value</returns>
		public static ulong ConvertToUInt64(byte[] val, int startIndex)
		{
			return ConvertToUInt64(val[startIndex], val[startIndex+1], val[startIndex+2], val[startIndex+3], val[startIndex+4], val[startIndex+5], val[startIndex+6], val[startIndex+7]);
		}

		/// <summary>
		/// Converts 8 bytes to an unsigned long value
		/// in high to low order
		/// </summary>
		/// <param name="v1">the first bytes</param>
		/// <param name="v2">the second bytes</param>
		/// <param name="v3">the third bytes</param>
		/// <param name="v4">the fourth bytes</param>
		/// <returns>the long value</returns>
		public static ulong ConvertToUInt64(byte v1, byte v2, byte v3, byte v4, byte v5, byte v6, byte v7, byte v8)
		{
			long result = v1;
			result = result << 8 | v2;
			result = result << 8 | v3;
			result = result << 8 | v4;
			result = result << 8 | v5;
			result = result << 8 | v6;
			result = result << 8 | v7;
			result = result << 8 | v8;
			return (ulong)result;
//			return (ulong)((v1 << 56) | (v2 << 48) | (v3 << 40) | (v4 << 32) | (v5 << 24) | (v6 << 16) | (v7 << 8) | v8);
		}
		/// <summary>
		/// Converts 4 bytes to an integer value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <returns>the integer value</returns>
		public static int ConvertToInt32(byte[] val)
		{
			return ConvertToInt32(val,0);
		}
		
		/// <summary>
		/// Converts 4 bytes to an integer value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <param name="startIndex">where to read the values from</param>
		/// <returns>the integer value</returns>
		public static int ConvertToInt32(byte[] val, int startIndex)
		{
			return ConvertToInt32(val[startIndex], val[startIndex+1], val[startIndex+2], val[startIndex+3]);
		}
		
		/// <summary>
		/// Converts 4 bytes to an integer value
		/// in high to low order
		/// </summary>
		/// <param name="v1">the first bytes</param>
		/// <param name="v2">the second bytes</param>
		/// <param name="v3">the third bytes</param>
		/// <param name="v4">the fourth bytes</param>
		/// <returns>the integer value</returns>
		public static int ConvertToInt32(byte v1, byte v2, byte v3, byte v4)
		{
			return (int)((v1 << 24) | (v2 << 16) | (v3 << 8) | v4);
		}

		/// <summary>
		/// Converts 4 bytes to an unsigned integer value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <returns>the integer value</returns>
		public static uint ConvertToUInt32(byte[] val)
		{
			return ConvertToUInt32(val,0);
		}
		
		/// <summary>
		/// Converts 4 bytes to an unsigned integer value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <param name="startIndex">where to read the values from</param>
		/// <returns>the integer value</returns>
		public static uint ConvertToUInt32(byte[] val, int startIndex)
		{
			return ConvertToUInt32(val[startIndex], val[startIndex+1], val[startIndex+2], val[startIndex+3]);
		}
		
		/// <summary>
		/// Converts 4 bytes to an unsigned integer value
		/// in high to low order
		/// </summary>
		/// <param name="v1">the first bytes</param>
		/// <param name="v2">the second bytes</param>
		/// <param name="v3">the third bytes</param>
		/// <param name="v4">the fourth bytes</param>
		/// <returns>the integer value</returns>
		public static uint ConvertToUInt32(byte v1, byte v2, byte v3, byte v4)
		{
			return (uint)((v1 << 24) | (v2 << 16) | (v3 << 8) | v4);
		}
		
		/// <summary>
		/// Converts 2 bytes to an short value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <returns>the integer value</returns>
		public static short ConvertToInt16(byte[] val)
		{
			return ConvertToInt16(val,0);
		}
		
		/// <summary>
		/// Converts 2 bytes to an short value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <param name="startIndex">where to read the values from</param>
		/// <returns>the integer value</returns>
		public static short ConvertToInt16(byte[] val, int startIndex)
		{
			return ConvertToInt16(val[startIndex], val[startIndex+1]);
		}
		
		/// <summary>
		/// Converts 2 bytes to an short value
		/// in high to low order
		/// </summary>
		/// <param name="v1">the first bytes</param>
		/// <param name="v2">the second bytes</param>
		/// <returns>the integer value</returns>
		public static short ConvertToInt16(byte v1, byte v2)
		{
			return (short)((v1 << 24) | (v2 << 16));
		}
		
		/// <summary>
		/// Converts 2 bytes to an unsigned short value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <returns>the integer value</returns>
		public static ushort ConvertToUInt16(byte[] val)
		{
			return ConvertToUInt16(val,0);
		}
		
		/// <summary>
		/// Converts 2 bytes to an unsigned short value
		/// in high to low order
		/// </summary>
		/// <param name="val">the bytes</param>
		/// <param name="startIndex">where to read the values from</param>
		/// <returns>the integer value</returns>
		public static ushort ConvertToUInt16(byte[] val, int startIndex)
		{
			return ConvertToUInt16(val[startIndex], val[startIndex+1]);
		}
		
		/// <summary>
		/// Converts 2 bytes to an integer value
		/// in high to low order
		/// </summary>
		/// <param name="v1">the first bytes</param>
		/// <param name="v2">the second bytes</param>
		/// <returns>the integer value</returns>
		public static ushort ConvertToUInt16(byte v1, byte v2)
		{
			return (ushort)((v2 & 0xff) | (v1 & 0xff) << 8);
		}

		/// <summary>
		/// Converts a byte array into a hex dump
		/// </summary>
		/// <param name="description">Dump description</param>
		/// <param name="dump">byte array</param>
		/// <returns>the converted hex dump</returns>
		public static string ToHexDump(string description, byte[] dump)
		{
			StringBuilder hexDump = new StringBuilder(description+"\n");
			for(int i=0; i<dump.Length; i+=16) 
			{
				StringBuilder text = new StringBuilder();
				StringBuilder hex = new StringBuilder();
				hex.Append(i.ToString("X4"));
				hex.Append(": ");
				
				for(int j=0; j<16; j++) 
				{
					if(j+i < dump.Length) 
					{
						byte val = dump[j+i];
						hex.Append(dump[j+i].ToString("X2"));
						hex.Append(" ");
						if (val>=32 && val<=127) 
						{
							text.Append((char)val);
						} 
						else 
						{
							text.Append(".");
						}
					}
					else 
					{
						hex.Append("   ");
						text.Append(" ");
					}
				}
				hex.Append("  ");
				hex.Append(text.ToString());
				hex.Append('\n');
				hexDump.Append(hex.ToString());
			}
			return hexDump.ToString();
		}
	}
}

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
using System;
using System.IO;
using System.Text;

namespace PacketLogConverter
{
	public enum ePacketDirection : byte
	{
		ServerToClient,
		ClientToServer,
	}

	public enum ePacketProtocol : byte
	{
		UDP,
		TCP,
	}

	/// <summary>
	/// Base class for all packets
	/// </summary>
	public class Packet : MemoryStream
	{
		public const int MAX_CODE = byte.MaxValue+1;

		private int                m_code;
		private ePacketDirection   m_direction;
		private ePacketProtocol    m_protocol;
		private TimeSpan           m_time;
		private LogPacketAttribute m_attribute;
		private bool               m_initialized;
		private bool               m_allowClassChange = true;
		private long               m_positionAfterInit;
		private int                m_logTextIndex = -1;
		private Exception          m_initException;

		public Packet(int capacity) : base(capacity)
		{
		}

		public int Code
		{
			get { return m_code; }
			set { m_code = value; }
		}

		public ePacketDirection Direction
		{
			get { return m_direction; }
			set { m_direction = value; }
		}

		public ePacketProtocol Protocol
		{
			get { return m_protocol; }
			set { m_protocol = value; }
		}

		public TimeSpan Time
		{
			get { return m_time; }
			set { m_time = value; }
		}

		public LogPacketAttribute Attribute
		{
			get { return m_attribute; }
			set { m_attribute = value; }
		}

		public bool Initialized
		{
			get { return m_initialized; }
			set { m_initialized = value; }
		}

		public bool AllowClassChange
		{
			get { return m_allowClassChange; }
			set { m_allowClassChange = value; }
		}

		public long PositionAfterInit
		{
			get { return m_positionAfterInit; }
			set { m_positionAfterInit = value; }
		}

		public int LogTextIndex
		{
			get { return m_logTextIndex; }
			set { m_logTextIndex = value; }
		}

		public Exception InitException
		{
			get { return m_initException; }
			set { m_initException = value; }
		}

		public byte this[long index]
		{
			get
			{
				if (index >= Length)
					throw new Exception("item[]: tried to read more data than contained in the packet (pos="+Position+", len="+Length+")");
				Position = index;
				return (byte)ReadByte();
			}
		}

		#region Stream read helpers

		public new byte ReadByte()
		{
			int b = base.ReadByte();
			if (b < 0)
				throw new Exception("ReadByte(): tried to read more data than contained in the packet (pos="+Position+", len="+Length+")");
			return (byte)b;
		}

		/// <summary>
		/// Reads in 2 bytes and converts it from network to host byte order
		/// </summary>
		/// <returns>A 2 byte (short) value</returns>
		public ushort ReadShort()
		{
			byte v1 = ReadByte();
			byte v2 = ReadByte();
			return Marshal.ConvertToUInt16(v1, v2);
		}

		/// <summary>
		/// Reads in 2 bytes
		/// </summary>
		/// <returns>A 2 byte (short) value in network byte order</returns>
		public ushort ReadShortLowEndian()
		{
			byte v1 = ReadByte();
			byte v2 = ReadByte();
			return Marshal.ConvertToUInt16(v2, v1);
		}

		/// <summary>
		/// Reads in 4 bytes and converts it from network to host byte order
		/// </summary>
		/// <returns>A 4 byte value</returns>
		public uint ReadInt()
		{
			byte v1 = ReadByte();
			byte v2 = ReadByte();
			byte v3 = ReadByte();
			byte v4 = ReadByte();
			return Marshal.ConvertToUInt32(v1, v2, v3, v4);
		}

		/// <summary>
		/// Reads in 4 bytes value in network byte order
		/// </summary>
		/// <returns>A 4 byte value</returns>
		public uint ReadIntLowEndian()
		{
			byte v1 = ReadByte();
			byte v2 = ReadByte();
			byte v3 = ReadByte();
			byte v4 = ReadByte();
			return Marshal.ConvertToUInt32(v4, v3, v2, v1);
		}

		/// <summary>
		/// Reads in 8 bytes and converts it from network to host byte order
		/// </summary>
		/// <returns>A 8 byte value</returns>
		public ulong ReadLong()
		{
			byte v1 = ReadByte();
			byte v2 = ReadByte();
			byte v3 = ReadByte();
			byte v4 = ReadByte();
			byte v5 = ReadByte();
			byte v6 = ReadByte();
			byte v7 = ReadByte();
			byte v8 = ReadByte();
			return Marshal.ConvertToUInt64(v1, v2, v3, v4, v5, v6, v7, v8);
		}

		/// <summary>
		/// Reads in 8 bytes value in network byte order
		/// </summary>
		/// <returns>A 8 byte value</returns>
		public ulong ReadLongLowEndian()
		{
			byte v1 = ReadByte();
			byte v2 = ReadByte();
			byte v3 = ReadByte();
			byte v4 = ReadByte();
			byte v5 = ReadByte();
			byte v6 = ReadByte();
			byte v7 = ReadByte();
			byte v8 = ReadByte();
			return Marshal.ConvertToUInt64(v8, v7, v6, v5, v4, v3, v2, v1);
		}

		/// <summary>
		/// Reads float value in network byte order.
		/// </summary>
		/// <returns></returns>
		public float ReadFloat()
		{
			uint data = ReadInt();
			return Util.GetFloat(data);
		}
		
		/// <summary>
		/// Reads float value in host byte order.
		/// </summary>
		/// <returns></returns>
		public float ReadFloatLowEndian()
		{
			uint data = ReadIntLowEndian();
			return Util.GetFloat(data);
		}

		/// <summary>
		/// Reads the time span.
		/// </summary>
		/// <returns>Timespan</returns>
		public TimeSpan ReadTimeSpan()
		{
			uint data = ReadInt();
			return Util.GetTimeSpan(data);
		}

		/// <summary>
		/// Reads the time span low endian.
		/// </summary>
		/// <returns>Timespan</returns>
		public TimeSpan ReadTimeSpanLowEndian()
		{
			uint data = ReadIntLowEndian();
			return Util.GetTimeSpan(data);
		}

		/// <summary>
		/// Skips 'num' bytes ahead in the stream
		/// </summary>
		/// <param name="num">Number of bytes to skip ahead</param>
		public void Skip(long num)
		{
			if (Position + num > Length)
				throw new Exception("Skip(long num): tried to skip more data than contained in the packet (pos="+Position+", len="+Length+")");
			Seek(num, SeekOrigin.Current);
		}

		/// <summary>
		/// Reads the zero-terminated string.
		/// </summary>
		/// <returns>Read string</returns>
		public string ReadString() 
		{
			byte[] buf = new byte[Length];
			for (int i = 0; i >= 0; i++)
			{
				byte b = ReadByte();
				if (b == 0) break;
				buf[i] = b;
			}
			return Marshal.ConvertToString(buf);
		}

		/// <summary>
		/// Reads a null-terminated string from the stream
		/// </summary>
		/// <param name="maxlen">Maximum number of bytes to read in</param>
		/// <returns>A string of maxlen or less</returns>
		public string ReadString(int maxlen)
		{
			byte[] buf = new byte[maxlen];
			Read(buf, 0, maxlen);
			return Marshal.ConvertToString(buf);
		}

		/// <summary>
		/// Reads in a pascal style string
		/// </summary>
		/// <returns>A string from the stream</returns>
		public string ReadPascalString()
		{
			int size = ReadByte();
			return ReadString(size);
		}

		public string ReadString(long stringOffset, int maxlen)
		{
			Position = stringOffset;
			return ReadString(maxlen);
		}

		public string ReadPascalString(long stringOffset)
		{
			Position = stringOffset;
			return ReadPascalString();
		}

		#endregion

		/// <summary>
		/// Gets the packet description
		/// </summary>
		public string Description
		{
			get
			{
				if (m_attribute == null || m_attribute.Description == null)
					return DefaultPacketDescriptions.GetDescription(Code, Direction);
				return m_attribute.Description;
			}
		}

		public virtual string ToHumanReadableString(TimeSpan baseTime, bool flagsDescription)
		{
			StringBuilder str = new StringBuilder(512);

			TimeSpan time = Time - baseTime;
			if (time.Ticks < 0)
			{
				str.Append('-');
				time = time.Negate();
			}
			else if (baseTime.Ticks != 0)
				str.Append('+');

			if (time.Days > 0)
				str.Append(time.Days).Append('d');
			str.Append(time.Hours).Append(':');
			str.Append(time.Minutes.ToString("D2")).Append(':');
			str.Append(time.Seconds.ToString("D2")).Append('.');
			str.Append(time.Milliseconds.ToString("D3"));
			str.Append(' ');
			switch (Direction)
			{
				case ePacketDirection.ClientToServer: str.Append("S<=C"); break;
				case ePacketDirection.ServerToClient: str.Append("S=>C"); break;
				default: str.Append(Direction); break;
			}
			if (PositionAfterInit > Length)
			{
				str.Append("(PositionAfterInit > PacketLength !)");
			}
			str.AppendFormat(" 0x{0:X2} {1}", Code, Description);

			string packetData = string.Empty;
			if (!Initialized)
			{
				packetData = "PACKET NOT INITIALIZED. Most likely errors during Init(). "+this.GetType().Name+"\n";
				if (InitException != null)
					packetData += InitException.ToString() + "\n";
			}
			try
			{
				packetData += GetPacketDataString(flagsDescription);
			}
			catch (Exception e)
			{
				packetData += string.Format("{0}: {1}", e.GetType().ToString(), e.Message);
			}

			if (packetData.Length > 0)
			{
				str.AppendFormat(" ({0})", packetData);
			}

			string notInitData = GetNotInitializedData();
			if (notInitData.Length > 0)
			{
				str.AppendFormat(" not initialized data from pos {0} ({1}): ({2})", PositionAfterInit, Length - PositionAfterInit, notInitData);
			}

			return str.ToString();
		}

		public virtual string GetNotInitializedData()
		{
			if (PositionAfterInit >= Length || GetType().Equals(typeof(Packet)))
				return "";

			StringBuilder str = new StringBuilder();

			Position = PositionAfterInit;
			str.Append("0x").Append(ReadByte().ToString("X2"));
			while (Position < Length)
			{
				str.Append(",0x").Append(ReadByte().ToString("X2"));
			}

			return str.ToString();
		}

		/// <summary>
		/// Gets all the packet data as a human readable string
		/// </summary>
		/// <param name="flagsDescription">Include flags description in output</param> 
		/// <returns></returns>
		public virtual string GetPacketDataString(bool flagsDescription)
		{
			return string.Empty;
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			switch (Direction)
			{
				case ePacketDirection.ClientToServer: str.Append("S<=C"); break;
				case ePacketDirection.ServerToClient: str.Append("S=>C"); break;
				default: str.Append(Direction); break;
			}
			str.AppendFormat(" 0x{0:X2} {1}", Code, Description);

			return str.ToString();
		}

		/// <summary>
		/// Copy all data from another packet and init private fields if any
		/// </summary>
		/// <param name="pak"></param>
		/// <returns>The target packet instance (this)</returns>
		public virtual Packet CopyFrom(Packet pak)
		{
			m_code = pak.m_code;
			m_direction = pak.m_direction;
			m_time = pak.m_time;
			m_protocol = pak.m_protocol;
			SetLength(0);
			Position = 0;
			pak.Position = 0;
			pak.WriteTo(this);
			return this;
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public virtual void Init()
		{
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public virtual void InitLog(PacketLog log)
		{
		}
	}
}
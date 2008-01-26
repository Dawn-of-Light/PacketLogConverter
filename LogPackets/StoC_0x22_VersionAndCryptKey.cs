using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x22, -1, ePacketDirection.ServerToClient, "Version And Crypt Key")]
	public class StoC_0x22_VersionAndCryptKey : Packet
	{
		protected byte encryption;
		protected byte isSI;
		protected byte majorVersion;
		protected byte minorVersion;
		protected byte build;
		protected ushort keyLenght;

		#region public access properties

		public byte Encryption { get { return encryption; } }
		public byte IsSi { get { return isSI; } }
		public byte MajorVersion { get { return majorVersion; } }
		public byte MinorVersion { get { return minorVersion; } }
		public byte Build { get { return build; } }
		public ushort KeyLenght { get { return keyLenght; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("encryption:{0} isSI:0x{1:X2} majorVersion:{2} minorVersion:{3} build:{4} keyLenght:{5}",
			                 encryption, isSI, majorVersion, minorVersion, build, keyLenght);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			encryption = ReadByte();
			isSI = ReadByte();
			majorVersion = ReadByte();
			minorVersion = ReadByte();
			build = ReadByte();
			if (encryption == 1)
			{
				keyLenght = ReadShort();
				Skip(keyLenght);
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x22_VersionAndCryptKey(int capacity) : base(capacity)
		{
		}
	}
}
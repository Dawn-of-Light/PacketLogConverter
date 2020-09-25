using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF4, 186, ePacketDirection.ClientToServer, "Crypt key request v186")]
	public class CtoS_0xF4_CryptKeyRequest_186 : CtoS_0xF4_CryptKeyRequest
	{
		protected ushort keyLength;

		#region public access properties

		public ushort KeyLenght { get { return keyLength; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keyLenght:");
			text.Write(keyLength.ToString("D"));
			text.Write(" ");
			base.GetPacketDataString(text, flagsDescription);
		}
		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			rc4Enabled = ReadByte();
			clientTypeAndAddons = ReadByte();
			clientVersionMajor = ReadByte();
			clientVersionMinor = ReadByte();
			clientVersionBuild = ReadByte();
			if(rc4Enabled==1)
			{
				keyLength = ReadByte();;
				Skip(keyLength);
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xF4_CryptKeyRequest_186(int capacity) : base(capacity)
		{
		}
	}
}
using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF4, 1126, ePacketDirection.ClientToServer, "Crypt key request v186")]
	public class CtoS_0xF4_CryptKeyRequest_1126 : CtoS_0xF4_CryptKeyRequest
	{
		protected uint keyLength = 0;

		#region public access properties

		public uint KeyLength { get { return keyLength; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keyLength:");
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

			if (Length <= 7)
			{
				clientTypeAndAddons = ReadByte();
				clientVersionMajor = ReadByte();
				clientVersionMinor = ReadByte();
				clientVersionBuild = ReadByte();
				clientVersionRev = (char)ReadByte();
			}
			else
			{
				rc4Enabled = 1;
				keyLength = ReadIntLowEndian();
				Skip(keyLength);
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xF4_CryptKeyRequest_1126(int capacity) : base(capacity)
		{
		}
	}
}
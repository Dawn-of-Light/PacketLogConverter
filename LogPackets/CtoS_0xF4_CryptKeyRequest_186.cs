namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF4, 186, ePacketDirection.ClientToServer, "Crypt key request v186")]
	public class CtoS_0xF4_CryptKeyRequest_186 : CtoS_0xF4_CryptKeyRequest
	{
		protected ushort keyLenght;

		#region public access properties

		public ushort KeyLenght { get { return keyLenght; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return "keyLenght:" + keyLenght.ToString("D") + " " + base.GetPacketDataString(flagsDescription);
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
				keyLenght = ReadByte();;
				Skip(keyLenght);
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
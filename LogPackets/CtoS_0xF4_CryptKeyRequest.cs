using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF4, -1, ePacketDirection.ClientToServer, "Crypt key request")]
	public class CtoS_0xF4_CryptKeyRequest : Packet
	{
		protected byte rc4Enabled;
		protected byte clientTypeAndAddons;
		protected byte clientVersionMajor;
		protected byte clientVersionMinor;
		protected byte clientVersionBuild;

		#region public access properties

		public byte Rc4Enabled { get { return rc4Enabled; } }
		public byte ClientTypeAndAddons { get { return clientTypeAndAddons; } }
		public byte ClientVersionMajor { get { return clientVersionMajor; } }
		public byte ClientVersionMinor { get { return clientVersionMinor; } }
		public byte ClientVersionBuild { get { return clientVersionBuild; } }

		#endregion

		public enum eClientType : byte
		{
			Classic = 1,
			ShroudedIsles = 2,
			TrialsOfAtlantis = 3,
			Catacombs = 4,
			DarknessRising = 5,
			Labyrinth = 6,
		}
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("rc4Enabled:{0} clientTypeAndAddons:0x{1:X2} clientVersion:{2}.{3}.{4}",
				rc4Enabled, clientTypeAndAddons, clientVersionMajor, clientVersionMinor, clientVersionBuild);
			if (flagsDescription)
			{
				text.Write("\n\tclient:{0}", (eClientType)(clientTypeAndAddons & 0x0F));
				text.Write(" expantions:");
				if ((clientTypeAndAddons & 0x80) == 0x80)
					text.Write(", NewFrontiers");
				if ((clientTypeAndAddons & 0x40) == 0x40)
					text.Write(", Foundations(Housing)");
			}
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
				Skip(256);
			}
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			if (log.SubversionReinit)
				return;
			Position = 2;
			int major = ReadByte();
			int minor = ReadByte();
			int build = ReadByte();
			int version = major*100 + minor*10 + build;
			if (version >= 200)
				version = version + 900;
			log.Version = (float)version;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xF4_CryptKeyRequest(int capacity) : base(capacity)
		{
		}
	}
}
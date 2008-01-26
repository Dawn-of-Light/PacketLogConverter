using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x05, -1, ePacketDirection.ServerToClient, "House permission")]
	public class StoC_0x05_HousePermission: Packet, IHouseIdPacket
	{
		protected byte count;
		protected byte unk1;
		protected ushort houseOid;
		protected Access[] m_permissions;

		#region public access properties

		public byte Count { get { return count; } }
		public byte Unk1 { get { return unk1; } }
		public ushort HouseId { get { return houseOid; } }
		public Access[] Permissions { get { return m_permissions; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("count:{0} unk1:0x{1:X2} houseOid:0x{2:X4}", count, unk1, houseOid);
			for (int i = 0; i < count; i++)
			{
				Access permission = (Access)m_permissions[i];
				text.Write("\n\tLevel:{0} Enter:{1} Vault1:{2} Vault2:{3} Vault3:{4} Vault4:{5} Appearance:{6} Interior:{7} Garden:{8} Banish:{9} useMerchant:{10} Tools:{11} Bind:{12} Merchant:0x{13:X2} payRent:{14} zero?:0x{15:X2}",
					permission.level, permission.enter, permission.vault1, permission.vault2, permission.vault3, permission.vault4, permission.appearance, permission.interior, permission.garden,
					permission.banish, permission.useMerchant, permission.tools, permission.bind, permission.merchant, permission.payRent, permission.unk1);
				if (flagsDescription)
				{
					text.Write("\n\tVault1 = View:({0,-6}) Add:({1,-6}) Remove:({2,-6})",
						((permission.vault1 & 4) == 4 ? "Enable" : "Disable"),
						((permission.vault1 & 2) == 2 ? "Enable" : "Disable"),
						((permission.vault1 & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tVault2 = View:({0,-6}) Add:({1,-6}) Remove:({2,-6})",
						((permission.vault2 & 4) == 4 ? "Enable" : "Disable"),
						((permission.vault2 & 2) == 2 ? "Enable" : "Disable"),
						((permission.vault2 & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tVault3 = View:({0,-6}) Add:({1,-6}) Remove:({2,-6})",
						((permission.vault3 & 4) == 4 ? "Enable" : "Disable"),
						((permission.vault3 & 2) == 2 ? "Enable" : "Disable"),
						((permission.vault3 & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tVault4 = View:({0,-6}) Add:({1,-6}) Remove:({2,-6})",
						((permission.vault4 & 4) == 4 ? "Enable" : "Disable"),
						((permission.vault4 & 2) == 2 ? "Enable" : "Disable"),
						((permission.vault4 & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tDecorations-> Interior = Add:({0,-6}) Remove:({1,-6})",
						((permission.interior & 2) == 2 ? "Enable" : "Disable"),
						((permission.interior & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tDecoration-> Garden = Add:({0,-6}) Remove:({1,-6})",
						((permission.garden & 2) == 2 ? "Enable" : "Disable"),
						((permission.garden & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Tools:({0})", ((permission.tools & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Use Merchant:({0})", ((permission.useMerchant & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Enter House:({0})", ((permission.enter & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Ablility to Banish:({0})", ((permission.banish & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Use Bind Stones:({0})", ((permission.bind & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Change external Appearance:({0})", ((permission.appearance & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Pay rent:({0})", ((permission.payRent & 1) == 1 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Add/Remove to/from Consign.Merchant:({0})", ((permission.merchant & 3) == 3 ? "Enable" : "Disable"));
					text.Write("\n\tAccess-> Withdraw from Consignment Merchant:({0})", ((permission.merchant & 0x10) == 0x10 ? "Enable" : "Disable"));
				}
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			count = ReadByte();
			unk1 = ReadByte();
			houseOid = ReadShort();
			m_permissions = new Access[count];
			for (int i = 0; i < count; i++)
			{
				Access permission = new Access();
				permission.level = ReadByte();
				permission.enter = ReadByte();
				permission.vault1 = ReadByte();
				permission.vault2 = ReadByte();
				permission.vault3 = ReadByte();
				permission.vault4 = ReadByte();
				permission.appearance = ReadByte();
				permission.interior = ReadByte();
				permission.garden = ReadByte();
				permission.banish = ReadByte();
				permission.useMerchant = ReadByte();
				permission.tools = ReadByte();
				permission.bind = ReadByte();
				permission.merchant = ReadByte();
				permission.payRent = ReadByte();
				permission.unk1 = ReadByte();
				m_permissions[i] = permission;
			}
		}

		public struct Access
		{
			public byte level;
			public byte enter;
			public byte vault1;
			public byte vault2;
			public byte vault3;
			public byte vault4;
			public byte appearance;
			public byte interior;
			public byte garden;
			public byte banish;
			public byte useMerchant;
			public byte tools;
			public byte bind;
			public byte merchant;
			public byte payRent;
			public byte unk1;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x05_HousePermission(int capacity) : base(capacity)
		{
		}
	}
}
using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDC, -1, ePacketDirection.ClientToServer, "System message filter")]
	public class CtoS_0xDC_MessageFilter: Packet
	{
		public byte[] filter;

		public enum filterName: byte
		{
			Spell = 0,
			You_hit = 1,
			You_are_hit = 2,
			Skill = 3,
			Merchant = 4,
			Your_death = 5,
			Others_death = 6,
			Others_combat = 7,
			Resist_changed = 8,
			Spell_expires = 9,
			Loot = 10,
			Spell_resisted = 11,
			Impotant = 12,
			Damaged = 13,
			Missed = 14,
			Spell_pulse = 15
		}

		#region public access properties

		public byte[] Filter{ get { return filter; } }
		
		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			for (byte i=0; i<16 ; i++)
				str.AppendFormat("\n\t{0}:{1}",(filterName)i, filter[i]);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			ArrayList tmp = new ArrayList(16);
			for (byte i=0; i < 16; i++)
				tmp.Add(ReadByte());
			filter = (byte[])tmp.ToArray(typeof(byte));
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xDC_MessageFilter(int capacity) : base(capacity)
		{
		}
	}
}
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0E, -1, ePacketDirection.ClientToServer, "House decoration change angle")]
	public class CtoS_0x0E_HouseDecorationChangeAngle: Packet, IHouseIdPacket
	{
		protected ushort index;
		protected ushort houseOid;
		protected ushort rotateAngle;
		protected byte place;
		protected byte unk1; // unused

		#region public access properties

		public ushort Index { get { return index; } }
		public ushort HouseId { get { return houseOid; } }
		public ushort RotateAngle { get { return rotateAngle; } }
		public byte Place { get { return place; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("index:{0,-3} houseOid:0x{1:X4} rotateAngle:{2,-3} place:{3}({5}) unk1:0x{4:X2}",
				index, houseOid, rotateAngle, place, unk1, (CtoS_0x0C_HouseItemPlacementRequest.ePlaceType)place);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			index = ReadShort();      // 0x00
			houseOid = ReadShort();   // 0x02
			rotateAngle = ReadShort();// 0x04
			place = ReadByte();       // 0x06
			unk1 = ReadByte();        // 0x07
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x0E_HouseDecorationChangeAngle(int capacity) : base(capacity)
		{
		}
	}
}
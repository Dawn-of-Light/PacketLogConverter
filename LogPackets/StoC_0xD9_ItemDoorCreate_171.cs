using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD9, 171, ePacketDirection.ServerToClient, "Item/door create v171")]
	public class StoC_0xD9_ItemDoorCreate_171 : StoC_0xD9_ItemDoorCreate
	{
		protected uint unk1_171;

		#region public access properties

		public uint Unk1_171 { get { return unk1_171; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} emblem:0x{1:X4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} model:0x{6:X4} health:{7,3}% flags:0x{8:X2}(realm:{12}) extraBytes:{9} unk1_171:0x{10:X8} name:\"{11}\"",
				oid, emblem, heading, x, y, z, model, hp, flags, extraBytes, unk1_171, name, (flags & 0x30)>>4);
			if (extraBytes == 4)
				str.AppendFormat(" internalId:0x{0:X4}", internalId);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			emblem = ReadShort();
			heading = ReadShort();
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			model = ReadShort();
			hp = ReadByte();
			flags = ReadByte();
			unk1_171 = ReadInt();
			name = ReadPascalString();
			extraBytes = ReadByte();

			if (extraBytes == 4)
				internalId = ReadInt();
			else if (extraBytes != 0)
				throw new Exception("unknown extra bytes count.");
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD9_ItemDoorCreate_171(int capacity) : base(capacity)
		{
		}
	}
}
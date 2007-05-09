using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x67, -1, ePacketDirection.ServerToClient, "Keep/Tower update")]
	public class StoC_0x67_KeepUpdate : Packet, IObjectIdPacket
	{
		protected ushort keepId;
		protected byte realm;
		protected byte level;
		protected byte count;
		protected byte[] components;
		protected byte unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { keepId }; }
		}

		public enum eAction: byte
		{
			Update = 0,
			Capture = 6,
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public byte Realm  { get { return realm; } }
		public byte Level { get { return level; } }
		public byte Count { get { return count; } }
		public byte[] Components { get { return components; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} realm:{1} level:{2} count:{3}",
				keepId, realm, level, count);
			if (count > 0)
			{
				str.Append("  component flags:(");
				for (int i = 0; i < count; i++)
				{
					byte component = components[i];
					if (i > 0)
						str.Append(',');
					str.AppendFormat("0x{0:X2}", component);
				}
				str.Append(")");
			}
			str.AppendFormat(" unk1:{0:X2}", unk1);
//			if (flagsDescription)
//				str.AppendFormat("({0})", (eAction)unk1);
			return str.ToString();
		}

		public override void Init()
		{
			Position = 0;
			keepId = ReadShort();
			realm = ReadByte();
			level = ReadByte();
			count = ReadByte();
			components = new byte[count];
			for(int i=0;i<count;i++)
			{
				components[i] = ReadByte();
			}
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x67_KeepUpdate(int capacity) : base(capacity)
		{
		}
	}
}
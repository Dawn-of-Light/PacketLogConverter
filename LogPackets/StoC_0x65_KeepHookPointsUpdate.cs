using System.Text;
using System.Collections;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x65, -1, ePacketDirection.ServerToClient, "Keep/Tower hook points update")]
	public class StoC_0x65_KeepHookPointsUpdate: Packet
	{
		protected int keepId;
		protected int count;
		protected ArrayList components;
		protected int unk1;

		#region public access properties

		public int KeepId { get { return keepId; } }
		public int Count { get { return count; } }
		public ArrayList Components { get { return components; } }
		public int Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} count:{1}",
				keepId, count);
			if (components.Count > 0)
			{
				str.Append("  component flags:(");
				for (int i = 0; i < components.Count; i++)
				{
					Component component = (Component)components[i];
					if (i > 0)
						str.Append(',');
					str.AppendFormat("0x{0:X2}", component.flag);
				}
				str.Append(")");
			}
			return str.ToString();
		}

		public override void Init()
		{
			Position = 0;
			keepId = ReadShort();
			count = ReadByte();
			components = new ArrayList();
			for(int i=0;i<count;i++)
			{
				Component component = new Component();
				component.flag = ReadByte();
				components.Add(component);
			}
			unk1 = ReadByte();
		}

		public class Component
		{
			public int flag;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x65_KeepHookPointsUpdate(int capacity) : base(capacity)
		{
		}
	}
}
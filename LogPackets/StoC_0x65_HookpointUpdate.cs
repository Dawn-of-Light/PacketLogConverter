using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x65, -1, ePacketDirection.ServerToClient, "Hookpoint update")]
	public class StoC_0x65_HookpointUpdate : Packet, IKeepIdPacket
	{
		/*0x02 bytes = keep ID
0x02 bytes = component ID
0x01 byte = hook point count
   0x01 byte = selected hookPt ID
   foreach GameKeepHookPoint
	  0x01 byte = hookPt.ID
if hook point count > 1
   0x01 byte = 0x41 //unk2 same byte in Keep Component Interact : 0x81 or 0x41 or 0x61 or 00
   */
		protected ushort keepId;
		protected ushort componentId;
		protected byte hookPointCount;
		protected byte unk1;
		protected byte[] hookpoint;

		/// <summary>
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		public ushort[] KeepIds
		{
			get { return new ushort[] { keepId }; }
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public ushort ComponentId { get { return componentId; } }
		public byte HookPointCount { get { return hookPointCount; } }
		public byte Unk1 { get { return unk1; } }
		public byte[] Hookpoint { get { return hookpoint; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keepId:0x{0:X4} componentId:{1,-2} HookPointCount:{2,-2} selectedHookPointIndex:0x{3:X2}",
				keepId, componentId, hookPointCount, unk1);
			string type;
			if (hookPointCount != 0)
			{
				text.Write(" HookPoints:");
				for (int i=0;i<hookPointCount;i++)
				{
					text.Write(" [{0}]=0x{1:X2}", i, hookpoint[i]);
					switch(hookpoint[i])
					{
						case 0x41:
							type = "ballista";
							break;
						case 0x61:
							type = "trebuchet";
							break;
						case 0x81:
							type = "cauldron";
							break;
						default:
							type = "";
							break;
					}
					if (type != "")
						text.Write("({0})", type);
				}
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			keepId = ReadShort();
			componentId = ReadShort();
			hookPointCount = ReadByte();
			unk1 = ReadByte();
			if (hookPointCount != 0)
			{
				hookpoint = new byte[hookPointCount];
				for (int i=0;i<hookPointCount;i++)
					hookpoint[i] = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x65_HookpointUpdate(int capacity) : base(capacity)
		{
		}
	}
}
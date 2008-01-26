using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x92, -1, ePacketDirection.ServerToClient, "Set weather")]
	public class StoC_0x92_SetWeather : Packet
	{
		protected uint x;
		protected uint width;
		protected ushort fogDiffusion;
		protected ushort speed;
		protected ushort intensity;
		protected ushort unk1;

		#region public access properties

		public uint X { get { return x; } }
		public uint Width { get { return width; } }
		public ushort FogDiffusion { get { return fogDiffusion; } }
		public ushort Speed { get { return speed; } }
		public ushort Intensity { get { return intensity; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("x:{0,-7} width:{1,-6} fogDiffusion:{2,-5} speed:{3,-4} intensity:{4} unk1:0x{5:X4}", x, width, fogDiffusion, speed, intensity, unk1);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			x = ReadInt();
			width = ReadInt();
			fogDiffusion = ReadShort();
			speed = ReadShort();
			intensity = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x92_SetWeather(int capacity) : base(capacity)
		{
		}
	}
}
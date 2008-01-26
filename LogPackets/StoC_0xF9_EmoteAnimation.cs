using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF9, -1, ePacketDirection.ServerToClient, "Emote animation")]
	public class StoC_0xF9_EmoteAnimation : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected byte emote;
		protected byte unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public byte Emote { get { return emote; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		#region emote enum

		public enum eEmote : byte
		{
			Beckon = 0x1,
			Blush = 0x2,
			Bow = 0x3,
			Cheer = 0x4,
			Clap = 0x5,
			Cry = 0x6,
			Curtsey = 0x7,
			Flex = 0x8,
			BlowKiss = 0x9,
			Dance = 0xa,
			Laugh = 0xb,
			Point = 0xc,
			Salute = 0xd,
			BangOnShield = 0xe,
			Victory = 0xf,
			Wave = 0x10,
			Distract = 0x11,
			MidgardFrenzy = 0x12,
			ThrowDirt = 0x13,
			StagFrenzy = 0x14,
			Roar = 0x15,
			Drink = 0x16,
			Ponder = 0x17,
			Military = 0x18,
			Present = 0x19,
			Taunt = 0x1a,
			Rude = 0x1b,
			Chicken = 0x1c,
			Hug = 0x1d,
			Charge = 0x1e,
			Meditate = 0x1f,
			No = 0x20,
			Raise = 0x21,
			Shrug = 0x22,
			Slap = 0x23,
			Slit = 0x24,
			Surrender = 0x25,
			Yes = 0x26,
			Beg = 0x27,
			Induct = 0x28,
			Mercy = 0x29,
			LvlUp = 0x2a,
			Pray = 0x2b,
			Bind = 0x2c,
			SpellGoBoom = 0x2d,
			Knock = 0x2e,
			Smile = 0x2f,
			Angry = 0x30,
			LookFar = 0x31,
			Stench = 0x32,
			Horse_halt = 0x33,
			Horse_pet = 0x34,
			Horse_courbette = 0x35,
			Horse_startle = 0x36,
			Horse_nod = 0x37,
			Horse_graze = 0x38,
			Horse_rear = 0x39,
			Sweat = 0x3a,
			Stagger = 0x3b,
			Rider_Trick = 0x3c,
			Yawn = 0x3d,
			Doh = 0x3e,
			Confused = 0x3f,
			Shiver = 0x40,
			Rofl = 0x41,
			Mememe = 0x42,
			Horse_whistle = 0x43,
			Worship = 0x44,
			PlayerPrepare = 0x45,
			PlayerPickup = 0x46,
			PlayerListen = 0x47,
			BindOnEquip = 72,
			AlbionBind = 73,
			MidgardBind = 74,
			HibernianBind = 75,
		};

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("oid:0x{0:X4} unk1:0x{1:X2} emote:{2}({3})", oid, unk1, emote, (eEmote)emote);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			emote = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF9_EmoteAnimation(int capacity) : base(capacity)
		{
		}
	}
}
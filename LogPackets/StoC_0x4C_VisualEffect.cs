using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4C, -1, ePacketDirection.ServerToClient, "Visual effect")]
	public class StoC_0x4C_VisualEffect: Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected byte subCode;
		protected ASubData subData;

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
		public byte SubCode { get { return subCode; } }
		public ASubData SubData { get { return subData; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} subcode:{1}", oid, subCode);
			if (subData == null)
				str.AppendFormat(" UNKNOWN SUBCODE");
			else
				subData.MakeString(str);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			subCode = ReadByte();
			InitSubcode(subCode);
		}

		private void InitSubcode(byte code)
		{
			switch (code)
			{
				case 3: InitHexEffectsUpdate(); break;
				case 4: InitUndergoundRaceFlyUpdate(); break;// on high speed look like flyed
				case 5: InitColorNameUpdate(); break;
				case 6: InitMobStealthEffectUpdate(); break; // Work only on Mob
				case 7: InitQuestEffectUpdate(); break;
				case 8: InitBlinkPanelUpdate(); break; // Self effect
				case 9: InitFreeLevelUpdate(); break; // Self effect
				case 11: InitTitleUpdate(); break; // Work only on others players
				case 12: InitBannerUpdate(); break; // Work only on players
				case 13: InitMinoRelicBeginUpdate(); break; // Work only on players
				case 14: InitMinoRelicTimerUpdate(); break; // Work only on players
				case 15: InitMinoRelicSetTimerUpdate(); break; // Work only on players
				default: InitDefaultUpdate(); break;
			}
			return;
		}

		public abstract class ASubData
		{
			abstract public void Init(StoC_0x4C_VisualEffect pak);
			abstract public void MakeString(StringBuilder str);
		}

		protected virtual void InitDefaultUpdate()
		{
			subData = new DefaultUpdate();
			subData.Init(this);
		}

		public class DefaultUpdate : ASubData
		{
			public byte flag;
			public ushort unk1;
			public ushort unk2;

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				unk1 = pak.ReadShort();
				unk2 = pak.ReadShort();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(UNKNOWN) flag:{0} unk1:0x{1:X4} unk2:0x{2:X4}", flag, unk1, unk2);
			}
		}

		protected virtual void InitHexEffectsUpdate()
		{
			subData = new HexEffectsUpdate();
			subData.Init(this);
		}

		public class HexEffectsUpdate: ASubData
		{
			public byte effect1;
			public byte effect2;
			public byte effect3;
			public byte effect4;
			public byte effect5;

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				effect1 = pak.ReadByte();
				effect2 = pak.ReadByte();
				effect3 = pak.ReadByte();
				effect4 = pak.ReadByte();
				effect5 = pak.ReadByte();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(HexEffects) effect1:0x{0:X2} effect2:{1:X2} effect3:{2:X2} effect4:{3:X2} effect5:{4:X2}",
					effect1, effect2, effect3, effect4, effect5);
			}
		}

		protected virtual void InitUndergoundRaceFlyUpdate()
		{
			subData = new UndergoundRaceFlyUpdate();
			subData.Init(this);
		}

		public class UndergoundRaceFlyUpdate: ASubData
		{
			public byte flag; // unused
			public uint unk1; // unused

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				unk1 = pak.ReadInt();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(Vampiire) flag:{0} unk1:{1}", flag, unk1);
			}
		}

		protected virtual void InitColorNameUpdate()
		{
			subData = new ColorNameUpdate();
			subData.Init(this);
		}

		public class ColorNameUpdate: ASubData
		{
			public byte flag;
			public uint unk1; // unused

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				unk1 = pak.ReadInt();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(ColorName) flag:{0}({1}) unk1:{2}",
					flag, (flag == 1 ? "PvP" : "RvR"), unk1);
			}
		}

		protected virtual void InitMobStealthEffectUpdate()
		{
			subData = new MobStealthEffectUpdate();
			subData.Init(this);
		}

		public class MobStealthEffectUpdate: ASubData
		{
			public byte flag;
			public uint unk1; // unused

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				unk1 = pak.ReadInt();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(MobStealth) flag:{0}({1}) unk1:{2}",
					flag, (flag == 0 ? "Disable" : "Enable"), unk1);
			}
		}

		protected virtual void InitQuestEffectUpdate()
		{
			subData = new QuestEffectUpdate();
			subData.Init(this);
		}

		public class QuestEffectUpdate: ASubData
		{
			public byte flag;
			public uint unk1; // unused

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				unk1 = pak.ReadInt();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(QuestEffect) flag:{0}({1}) unk1:{2}",
					flag, (flag == 0 ? "Disable" : "Enable"), unk1);
			}
		}

		protected virtual void InitBlinkPanelUpdate()
		{
			subData = new BlinkPanelUpdate();
			subData.Init(this);
		}

		public class BlinkPanelUpdate: ASubData
		{
			public enum ePanel : byte
			{
				Command_Window = 0,
				Journal_Button = 1,
				Map_Button = 2,
				Sit_Button = 3,
				Stats_Index_Window = 4,
				Attributes_Button = 5,
				Inventory_Button = 6,
				Specializations_Button = 7,
				CombatStyles_Button = 8,
				MagicSpells_Button = 9,
				Group_Button = 0x0A,
				MiniInfo_Window = 0x0B,
				CommandEnter_Window = 0x0C,
				QuickBar1_Window = 0x0D,
				QBar1_Bank1Button = 0x0E,
				QBar1_Bank2Button = 0x0F,
				QBar1_Bank3Button = 0x10,
				QBar1_Bank4Button = 0x11,
				QBar1_Bank5Button = 0x12,
				QBar1_Bank6Button = 0x13,
				QBar1_Bank7Button = 0x14,
				QBar1_Bank8Button = 0x15,
				QBar1_Bank9Button = 0x16,
				QBar1_Bank10Button = 0x17,
			}

			public byte flag;
			public uint unk1; // unused

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				unk1 = pak.ReadInt();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(BlinkPanel) flag:{0}({1}) unk1:{2}",
					flag, (ePanel)flag, unk1);
			}
		}

		protected virtual void InitFreeLevelUpdate()
		{
			subData = new FreeLevelUpdate();
			subData.Init(this);
		}

		public class FreeLevelUpdate: ASubData
		{
			public byte flag;
			public ushort unk1; // unused
			public short time;

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				unk1 = pak.ReadShort();
				time = (short)pak.ReadShort();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(FreeLevel) flag:{0}", flag);
				TimeSpan t = new TimeSpan(0, time-1, 0);
				switch(flag)
				{
					case 1:
						str.Append("(\"Above the max level to abtain a free level\")");
						break;
					case 2:
						str.Append("(\"Now aligible for a free level\")");
						break;
					case 3:
						str.AppendFormat("(\"{0} days {1} hours {2} minutes until a free level\") time:0x{3:X4}",
							t.Days, t.Hours, t.Minutes, time);
						break;
					case 4:
						str.AppendFormat("(\"One level and {0} days {1} hours {2} minutes until a free level\") time:0x{3:X4}",
							t.Days, t.Hours, t.Minutes, time);
						break;
					case 5:
						str.Append("(\"One level until a free level\")");
						break;
					default:
						str.Append("(Disable)");
						break;
				}
			}
		}

		protected virtual void InitTitleUpdate()
		{
			subData = new TitleUpdate();
			subData.Init(this);
		}

		public class TitleUpdate: ASubData
		{
			public byte flag;
			public ushort titleLength;
			public ushort unk1; // unused
			public string title;

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				titleLength = pak.ReadShort();
				unk1 = pak.ReadShort();
				if (flag == 1)
					title = pak.ReadString(titleLength);
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(Title) flag:{0}({1}) titleLength:{2} unk1:{3}{4}",
					flag, (flag == 0 ? "Clear" : "Set"), titleLength, unk1, (flag == 0 ? "" : " title:\"" + title + '\"'));
			}
		}

		protected virtual void InitBannerUpdate()
		{
			subData = new BannerUpdate();
			subData.Init(this);
		}

		public class BannerUpdate: ASubData
		{
			public byte flag;
			public ushort unk1;
			public ushort emblem;

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				unk1 = pak.ReadShort(); // Flag use new guildEmblem
				emblem = pak.ReadShort();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(Banner) flag:{0}({1}) newEmblem:0x{2:X4} guildEmblem:{3}",
					flag, (flag == 1 ? "Disable" : "Enable"), unk1, emblem);
//				if (flagsDescription && emblem != 0)
//				  str.AppendFormat(" (guildLogo:{0,-3} pattern:{1} primaryColor:{2,-2} secondaryColor:{3})", (unk1 << 7) | (emblem >> 9), (emblem >> 7) & 2, (emblem >> 3) & 0x0F, emblem & 7);
			}
		}

		protected virtual void InitMinoRelicBeginUpdate()
		{
			subData = new MinoRelicBeginUpdate();
			subData.Init(this);
		}

		public class MinoRelicBeginUpdate: ASubData
		{
			public byte flag;
			public uint effect;

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				effect = pak.ReadInt();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(MinoRelic) flag:{0}({1}) effect:0x{2:X8}",
					flag, (flag == 1 ? "Disable" : "Enable"), effect);
			}
		}

		protected virtual void InitMinoRelicSetTimerUpdate()
		{
			subData = new MinoRelicSetTimerUpdate();
			subData.Init(this);
		}

		public class MinoRelicSetTimerUpdate: ASubData
		{
			public byte flag; // unused
			public uint timer;

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				timer = pak.ReadInt();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(MinoRelicTimerSet) flag:{0} timer:{1}", flag, timer);
			}
		}

		protected virtual void InitMinoRelicTimerUpdate()
		{
			subData = new MinoRelicTimerUpdate();
			subData.Init(this);
		}

		public class MinoRelicTimerUpdate: ASubData
		{
			public byte flag; // unused
			public uint timer;

			public override void Init(StoC_0x4C_VisualEffect pak)
			{
				flag = pak.ReadByte();
				timer = pak.ReadInt();
			}

			public override void MakeString(StringBuilder str)
			{
				str.AppendFormat("(MinoRelicTimer) flag:{0} timer:{1}", flag, timer);
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x4C_VisualEffect(int capacity) : base(capacity)
		{
		}
	}
}
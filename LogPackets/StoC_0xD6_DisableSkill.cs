using System;
using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD6, -1, ePacketDirection.ServerToClient, "Disable skill")]
	public class StoC_0xD6_DisableSkill: Packet
	{
		protected object update;
		protected byte subCode;

		#region public access properties

		public object Update { get { return update; } }
		public byte SubCode { get { return subCode; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("subcode:0x{0:X2}  ", subCode);
			str.Append(update.ToString());

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			subCode = this[3];
			Position = 0;

			switch (subCode)
			{
				case 0: update = ReadDisableSkill(); break;
				case 1: update = ReadDisableHybrid(); break;
				case 2: update = ReadDisableCasterList(); break;
				default: throw new Exception("UNKNOWN SUBCODE " + subCode);
			}
		}

		#region ReadDisableSkill

		protected virtual object ReadDisableSkill()
		{
			DisableSkillData data = new DisableSkillData();

			data.time = ReadShort();
			data.skill = ReadByte();
			ReadByte(); // subtype

			return data;
		}

		public class DisableSkillData
		{
			public ushort time;
			public byte skill;

			public override string ToString()
			{
				return string.Format("DISABLE SKILL: time:{0} skill:{1}", time, skill);
			}
		}

		#endregion

		#region ReadDisableHybrid

		protected virtual object ReadDisableHybrid()
		{
			DisableHybridData data = new DisableHybridData();

			data.time = ReadShort();
			data.count = ReadByte();
			ReadByte(); // subtype

			AbilityData[] abilities = data.abilities = new AbilityData[data.count];
			for(int i=0;i<data.count;i++)
			{
				abilities[i].index = ReadShort();
				abilities[i].unk1 = ReadShort();
			}

			return data;
		}

		public class DisableHybridData
		{
			public ushort time;
			public byte count;
			public AbilityData[] abilities;

			public override string ToString()
			{
				StringBuilder str = new StringBuilder(32);

				str.AppendFormat("DISABLE HYBRID SPELL: count:{0}, time:{1}", count, time);

				if (count == 1)
					str.AppendFormat(" index:{0} unk1:{1}", abilities[0].index, abilities[0].unk1);
				else
				{
					for (int i = 0; i < count; i++)
						str.AppendFormat(" ; index:{0} unk1:{1}", abilities[i].index, abilities[i].unk1);
				}

				return str.ToString();
			}
		}
		
		public struct AbilityData
		{
			public ushort index, unk1;
		}

		#endregion

		#region ReadDisableCasterList

		protected virtual object ReadDisableCasterList()
		{
			DisableListData data = new DisableListData();

			data.time = ReadShort();
			data.count = ReadByte();
			ReadByte(); // subtype

			ListSpell[] spells = data.spells = new ListSpell[data.count];
			for(int i=0;i<data.count;i++)
			{
				spells[i].lineIndex = ReadByte();
				spells[i].spellIndex = ReadByte();
			}
			
			return data;
		}

		public class DisableListData
		{
			public ushort time;
			public byte count;
			public ListSpell[] spells;

			public override string ToString()
			{
				StringBuilder str = new StringBuilder(512);

				str.AppendFormat("\nDISABLE PURE CASTER SPELLS: time:{0} count:{1}", time, count);
				for (int i = 0; i < count; i++)
				{
					str.AppendFormat("\n\tline:{0,-2} spell:{1}", spells[i].lineIndex, spells[i].spellIndex);
				}

				return str.ToString();
			}
		}

		public struct ListSpell
		{
			public byte lineIndex, spellIndex;
		}

		#endregion

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD6_DisableSkill(int capacity) : base(capacity)
		{
		}
	}
}
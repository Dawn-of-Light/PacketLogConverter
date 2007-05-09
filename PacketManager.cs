using System;
using System.Collections;
using System.Reflection;

namespace PacketLogConverter
{
	/// <summary>
	/// Manage packets
	/// </summary>
	public sealed class PacketManager
	{
		private static readonly SortedList[] m_ctosConstrsByCode = new SortedList[Packet.MAX_CODE];
		private static readonly SortedList[] m_stocConstrsByCode = new SortedList[Packet.MAX_CODE];
		private static readonly Hashtable m_packetTypeByAttribute = new Hashtable(512);
		private static readonly Hashtable m_attributeByType = new Hashtable(512);

		static PacketManager()
		{
			Init();
		}
		
		public static LogPacketAttribute GetPacketTypeAttribute(Type t)
		{
			return (LogPacketAttribute) m_attributeByType[t];
		}

		public static Packet CreatePacket(int version, int code, ePacketDirection dir)
		{
			return CreatePacket(version, code, dir, 0);
		}

		public static Packet CreatePacket(int version, int code, ePacketDirection dir, int capacity)
		{
			LogPacketData packetData = GetPacketData(version, code, dir);
			return CreatePacket(packetData, capacity);
		}

		public static Packet ChangePacketClass(Packet pak, int newVersion)
		{
			LogPacketData packetData = GetPacketData(newVersion, pak.Code, pak.Direction);
			if (packetData != null && pak.GetType().Equals(packetData.Type))
			{
				pak.Attribute = packetData.Attribute;
				return pak;
			}

			Packet newPacket = CreatePacket(packetData, (int)pak.Length);
			newPacket.CopyFrom(pak);
			return newPacket;
		}

		private static Packet CreatePacket(LogPacketData packetData, int capacity)
		{
			if (packetData == null)
				return new Packet(capacity);
	
			try
			{
				Packet pak = (Packet)packetData.CapacityConstructor.Invoke(new object[] {capacity});
				pak.Attribute = packetData.Attribute;
				return pak;
			}
			catch(Exception)
			{
				return new Packet(capacity);
			}
		}

		private static LogPacketData GetPacketData(int version, int code, ePacketDirection dir)
		{
			SortedList typesByVersion =
				dir == ePacketDirection.ClientToServer
					? m_ctosConstrsByCode[code]
					: m_stocConstrsByCode[code];
	
			LogPacketData packetData = null;
			foreach (DictionaryEntry entry in typesByVersion)
			{
				int ver = (int)entry.Key;
				if (ver > version)
					break;
				packetData = (LogPacketData)entry.Value;
			}
			return packetData;
		}

		private sealed class LogPacketData
		{
			public readonly ConstructorInfo CapacityConstructor;
			public readonly LogPacketAttribute Attribute;
			public readonly Type Type;

			public LogPacketData(ConstructorInfo constructor, LogPacketAttribute attribute, Type type)
			{
				this.CapacityConstructor = constructor;
				this.Attribute = attribute;
				this.Type = type;
			}
		}

		public static int GetPacketTypesCount(int code, ePacketDirection dir)
		{
			return dir == ePacketDirection.ClientToServer
				? m_ctosConstrsByCode[code].Count
				: m_stocConstrsByCode[code].Count;
		}

		public static string GetPacketDescription(int version, int code, ePacketDirection dir)
		{
			SortedList packets =
				dir == ePacketDirection.ClientToServer
					? m_ctosConstrsByCode[code]
					: m_stocConstrsByCode[code];

			LogPacketData c = (LogPacketData)packets[version];
			if (c == null)
				 return null;
			return c.Attribute.Description;
		}

		public static Hashtable GetPacketTypesByAttribute()
		{
			return (Hashtable)m_packetTypeByAttribute.Clone();
		}

		public static void Init()
		{
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				m_ctosConstrsByCode[i] = new SortedList(4);
				m_stocConstrsByCode[i] = new SortedList(4);
			}

			Type[] constrParams = new Type[] {typeof(int)};
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in asm.GetTypes())
				{
					try
					{
						if (!type.IsClass) continue;
						if (type.IsAbstract) continue;
						if (typeof(Packet).IsAssignableFrom(type))
						{
							ConstructorInfo constr = type.GetConstructor(constrParams);
							if (constr == null)
								continue;
							foreach (LogPacketAttribute attr in type.GetCustomAttributes(typeof(LogPacketAttribute), false))
							{
								SortedList constrsByVersion =
									attr.Direction == ePacketDirection.ClientToServer
										? m_ctosConstrsByCode[attr.Code]
										: m_stocConstrsByCode[attr.Code];
								
								constrsByVersion.Add(attr.Version, new LogPacketData(constr, attr, type));
								m_packetTypeByAttribute.Add(attr, type);
								m_attributeByType[type] = attr;
								break;
							}
						}
					}
					catch (Exception e)
					{
						throw new Exception("Error parsing type "+type.FullName, e);
					}
				}
			}
		}

	}
}

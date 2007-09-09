using System;

namespace PacketLogConverter
{
	/// <summary>
	/// Denotes a class as a packet
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public class LogPacketAttribute : Attribute
	{
		private int m_packetCode;
		private float m_version;
		private ePacketDirection m_direction;
		private string m_description;

		public LogPacketAttribute(int packetCode, float version, ePacketDirection direction) : this (packetCode,  version, direction, null)
		{
		}

		public LogPacketAttribute(int packetCode, float version, ePacketDirection direction, string packetDescription)
		{
			m_packetCode = packetCode;
			m_version = version;
			m_direction = direction;
			m_description = packetDescription.ToLower();
		}

		public int Code
		{
			get { return m_packetCode; }
		}

		public float Version
		{
			get { return m_version; }
		}

		public ePacketDirection Direction
		{
			get { return m_direction; }
		}

		public string Description
		{
			get { return m_description; }
		}
	}
}

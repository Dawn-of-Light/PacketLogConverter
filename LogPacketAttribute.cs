using System;

namespace PacketLogConverter
{
	/// <summary>
	/// Denotes a class as a packet
	/// </summary>
	[Serializable, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
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

		///<summary>
		///Returns a value that indicates whether this instance is equal to a specified object.
		///</summary>
		///
		///<returns>
		///true if obj equals the type and value of this instance; otherwise, false.
		///</returns>
		///
		///<param name="obj">An <see cref="T:System.Object"></see> to compare with this instance or null. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			bool ret = false;
			LogPacketAttribute attr = obj as LogPacketAttribute;
			if (attr != null)
			{
				// Check all fields
				ret = attr.m_direction == m_direction;
				ret &= attr.m_packetCode == m_packetCode;
				ret &= attr.m_version == m_version;
			}

			return ret;
		}

		///<summary>
		///Returns the hash code for this instance.
		///</summary>
		///
		///<returns>
		///A 32-bit signed integer hash code.
		///</returns>
		///<filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			int ret = (int) m_direction + m_packetCode + (int) m_version;
			return ret;
		}
	}
}

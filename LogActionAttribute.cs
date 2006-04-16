using System;

namespace PacketLogConverter
{
	/// <summary>
	/// Denotes a class as a log action
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public sealed class LogActionAttribute : Attribute
	{
		private string m_name;
		private int m_priority;

		public LogActionAttribute(string name)
		{
			m_name = name;
		}

		public string Name
		{
			get { return m_name; }
		}

		public int Priority
		{
			get { return m_priority; }
			set { m_priority = value; }
		}
	}
}

using System;

namespace PacketLogConverter
{
	/// <summary>
	/// Denotes a class as a log writer
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	public sealed class LogWriterAttribute : Attribute
	{
		private readonly string m_description;
		private readonly string m_fileMask;
		private int m_priority = 100;

		public LogWriterAttribute(string description, string fileMask)
		{
			m_description = description;
			m_fileMask = fileMask;
		}

		public string Description
		{
			get { return m_description; }
		}

		public string FileMask
		{
			get { return m_fileMask; }
		}

		public int Priority
		{
			get { return m_priority; }
			set { m_priority = value; }
		}
	}
}

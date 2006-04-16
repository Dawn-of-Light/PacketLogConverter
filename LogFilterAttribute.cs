using System;
using System.Windows.Forms;

namespace PacketLogConverter
{
	/// <summary>
	/// Denotes a class as a log filter
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public sealed class LogFilterAttribute : Attribute
	{
		private readonly string m_filterName;
		private readonly Shortcut m_shortcutKey;
		private int m_priority;

		public LogFilterAttribute(string filterName) : this(filterName, Shortcut.None)
		{
		}

		public LogFilterAttribute(string filterName, Shortcut shortcutKey)
		{
			m_filterName = filterName;
			m_shortcutKey = shortcutKey;
		}

		public string FilterName
		{
			get { return m_filterName; }
		}

		public Shortcut ShortcutKey
		{
			get { return m_shortcutKey; }
		}

		public int Priority
		{
			get { return m_priority; }
			set { m_priority = value; }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Instances of this class hold references to packets a variable is linked to.
	/// </summary>
	[Serializable]
	internal class PacketPropertyVariableLink
	{
		private PacketPropertyValueFilterForm.PacketClass packetClass;
		private PacketPropertyValueFilterForm.ClassMemberPath packetClassMemberPath;

		/// <summary>
		/// Gets or sets the packet class.
		/// </summary>
		/// <value>The packet class.</value>
		public PacketPropertyValueFilterForm.PacketClass PacketClass
		{
			get { return packetClass; }
			set { packetClass = value; }
		}

		/// <summary>
		/// Gets or sets the packet class member path.
		/// </summary>
		/// <value>The packet class member path.</value>
		public PacketPropertyValueFilterForm.ClassMemberPath PacketClassMemberPath
		{
			get { return packetClassMemberPath; }
			set { packetClassMemberPath = value; }
		}

		#region Equals/Hashcode

		/// <summary>
		/// Compares this link to another link.
		/// </summary>
		/// <param name="packetPropertyVariableLink">The packet property variable link.</param>
		/// <returns><code>true</code> if all fields are equal, <code><false/code> otherwise.</returns>
		protected bool Equals(PacketPropertyVariableLink packetPropertyVariableLink)
		{
			if (packetPropertyVariableLink == null) return false;
			return Equals(packetClass, packetPropertyVariableLink.packetClass) && Equals(packetClassMemberPath, packetPropertyVariableLink.packetClassMemberPath);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as PacketPropertyVariableLink);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			return packetClass.GetHashCode() + 29*packetClassMemberPath.GetHashCode();
		}

		#endregion

		///<summary>
		///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		///</summary>
		///
		///<returns>
		///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		///</returns>
		///<filterpriority>2</filterpriority>
		public override string ToString()
		{
			string ret = "[" + packetClass.ToString() + "]." + packetClassMemberPath.ToString();
			return ret;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PacketPropertyVariableLink"/> class.
		/// </summary>
		/// <param name="packetClass">The packet class.</param>
		/// <param name="packetClassMemberPath">The packet class member path.</param>
		public PacketPropertyVariableLink(PacketPropertyValueFilterForm.PacketClass packetClass,
												PacketPropertyValueFilterForm.ClassMemberPath packetClassMemberPath)
		{
			// Param checks
			if (null == packetClass)
			{
				throw new ArgumentNullException("packetClass");
			}
			if (null == packetClassMemberPath)
			{
				throw new ArgumentNullException("packetClassMemberPath");
			}

			// Fields initialization
			this.packetClass = packetClass;
			this.packetClassMemberPath = packetClassMemberPath;
		}
	}
}

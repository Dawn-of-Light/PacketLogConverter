using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// This class contains all information about single variable.
	/// </summary>
	[Serializable]
	internal class PacketPropertyVariable
	{
		private readonly IList<PacketPropertyVariableLink>	links = new List<PacketPropertyVariableLink>(2);
		private string										name;


		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				if (null == name)
				{
					name = string.Empty;
				}
			}
		}

		/// <summary>
		/// Gets the links.
		/// </summary>
		/// <value>The links.</value>
		public IList<PacketPropertyVariableLink> Links
		{
			get { return links; }
		}

		#region Equals/Hashcode

		/// <summary>
		/// Compares this variable to another one.
		/// </summary>
		/// <param name="packetPropertyVariable">The packet property variable.</param>
		/// <returns><code>true</code> if all fields are equal, <code>false</code> otherwise.</returns>
		protected bool Equals(PacketPropertyVariable packetPropertyVariable)
		{
			if (packetPropertyVariable == null) return false;
			return Equals(links, packetPropertyVariable.links) && Equals(name, packetPropertyVariable.name);
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
			return Equals(obj as PacketPropertyVariable);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			return links.GetHashCode() + 29*name.GetHashCode();
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
			return "{var: " + name + "}";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PacketPropertyVariable"/> class.
		/// </summary>
		/// <param name="packetClass">The packet class.</param>
		/// <param name="packetClassMemberPath">The packet class member path.</param>
		/// <param name="name">The name.</param>
		public PacketPropertyVariable(PacketPropertyValueFilterForm.PacketClass packetClass,
										PacketPropertyValueFilterForm.ClassMemberPath packetClassMemberPath,
										string name)
		{
			PacketPropertyVariableLink loc = new PacketPropertyVariableLink(packetClass, packetClassMemberPath);
			this.links.Add(loc);

			if (null == name)
			{
				name = string.Empty;
			}

			this.name = name;
		}
	}
}

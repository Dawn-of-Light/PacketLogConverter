using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// This class contains helper methods for <see cref="PacketPropertyValueFilterForm"/>.
	/// </summary>
	public partial class PacketPropertyValueFilterForm
	{
		/// <summary>
		/// This class is used in combo box to show path to single member of a class (packet).
		/// </summary>
		[Serializable]
		internal class ClassMemberPath
		{
			public readonly List<MemberInfo> members;
			public readonly bool isRecursive;
			private readonly string fullName;

			/// <summary>
			/// Initializes a new instance of the <see cref="T:ClassMemberPath"/> class.
			/// </summary>
			/// <param name="members">Path to single member.</param>
			/// <param name="isRecursive"><code>true</code> if recursive flag is set</param>
			public ClassMemberPath(List<MemberInfo> members, bool isRecursive)
			{
				this.members = members;
				this.isRecursive = isRecursive;

				if (members == null)
				{
					if (isRecursive)
					{
						fullName = "(any property, recursive)";
					}
					else
					{
						fullName = "(any property)";
					}
				}
				else
				{
					// Construct full path to property
					StringBuilder str = new StringBuilder(members.Count * 16);
					foreach (MemberInfo info in members)
					{
						if (str.Length > 0)
						{
							str.Append('.');
						}
						str.Append(info.Name);

						// Check if member is an array
						if ((info is PropertyInfo && ((PropertyInfo)info).PropertyType.IsArray)
							|| (info is FieldInfo && ((FieldInfo)info).FieldType.IsArray))
						{
							str.Append("[]");
						}
					}
					fullName = str.ToString();
				}
			}

			/// <summary>
			/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
			/// </summary>
			/// <returns>
			/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
			/// </returns>
			public override string ToString()
			{
				return fullName;
			}

			///<summary>
			///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
			///</summary>
			///
			///<returns>
			///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
			///</returns>
			///
			///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
			public override bool Equals(object obj)
			{
				// Needed for proper list updates
				ClassMemberPath prop = obj as ClassMemberPath;
				bool bRet = prop != null;
				if (bRet)
				{
					if (prop.isRecursive != isRecursive)
					{
						bRet = false;
					}
					else
					{
						if (prop.members == members)
						{
							bRet = true;
						}
						else if (prop.members != null && members != null && prop.members.Count == members.Count)
						{
							// Check every element of collections
							for (int i = members.Count - 1; i >= 0; i--)
							{
								if (!members[i].Equals(prop.members[i]))
								{
									bRet = false;
									break;
								}
							}
						}
						else
						{
							bRet = false;
						}
					}
				}
				return bRet;
			}

			///<summary>
			///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
			///</summary>
			///
			///<returns>
			///A hash code for the current <see cref="T:System.Object"></see>.
			///</returns>
			///<filterpriority>2</filterpriority>
			public override int GetHashCode()
			{
				if (members == null)
					return 0;
				return members.Count;
			}
		}
	}
}

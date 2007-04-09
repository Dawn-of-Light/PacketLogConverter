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
		internal static readonly int RECURSIVE_SEARCH_DEPTH = 6;

		/// <summary>
		/// Gets paths to every property and field a class.
		/// </summary>
		/// <param name="clazz">The class to read properties for.</param>
		/// <param name="depth">How deep to read properties/fields. Zero means only properties/fields of specified class.</param>
		/// <param name="flags">The flags of property and field to read.</param>
		/// <param name="result">List with all paths.</param>
		/// <param name="curList">Path to current node.</param>
		internal static void GetPropertiesAndFields(Type clazz, int depth, BindingFlags flags, List<List<MemberInfo>> result, List<MemberInfo> curList)
		{
			if (depth < 0)
			{
				return;
			}
			if (IsClassIgnored(clazz))
			{
				return;
			}
			
			// Get properties
			PropertyInfo[] properties = clazz.GetProperties(flags);
			foreach (PropertyInfo info in properties)
			{
				// Clone current path, add this node if property is readable and is not indexer ("item[..]")
				if (!IsClassIgnored(info.PropertyType) && info.CanRead && info.GetIndexParameters().GetLength(0) == 0)
				{
					List<MemberInfo> newList = new List<MemberInfo>(curList);
					newList.Add(info);

					// Get sub-element type; can be array
					Type propType = info.PropertyType;
					string name = info.Name;
					name.Clone();
					if (propType.HasElementType)
					{
						propType = propType.GetElementType();
					}
					result.Add(newList);

					// Special case - no sub-properties of String types
					if (propType != typeof(string))
					{
						GetPropertiesAndFields(propType, depth - 1, flags, result, newList);
					}
				}
			}

			// Get fields
			FieldInfo[] fields = clazz.GetFields(flags);
			foreach (FieldInfo info in fields)
			{
				// Clone current path, add this node
				if (!IsClassIgnored(info.FieldType))
				{
					List<MemberInfo> newList = new List<MemberInfo>(curList);
					newList.Add(info);

					// Get sub-element type; can be array
					Type fieldType = info.FieldType;
					if (fieldType.HasElementType)
					{
						fieldType = fieldType.GetElementType();
					}
					result.Add(newList);

					// Special case - no sub-properties of String types
					if (fieldType != typeof(string))
					{
						GetPropertiesAndFields(fieldType, depth - 1, flags, result, newList);
					}
				}
			}
		}

		/// <summary>
		/// Determines whether class is ignored.
		/// </summary>
		/// <param name="classType">Type of the class.</param>
		/// <returns>
		/// 	<c>true</c> if class is ignored; otherwise, <c>false</c>.
		/// </returns>
		internal static bool IsClassIgnored(Type classType)
		{
			return classType == typeof(TimeSpan)
				|| classType == typeof(LogPacketAttribute)
//				|| valType == typeof(String)
				|| classType == typeof(ePacketProtocol)
				|| classType == typeof(ePacketDirection)
				|| typeof(Exception).IsAssignableFrom(classType)
				|| typeof(IDictionary).IsAssignableFrom(classType);
//				|| typeof(IList).IsAssignableFrom(classType);
		}

		#region data containers

		/// <summary>
		/// This class shows packet class in combo box.
		/// </summary>
		internal class PacketClass
		{
			public readonly Type type;
			public readonly LogPacketAttribute attr;

			public PacketClass(Type type, LogPacketAttribute attr)
			{
				this.type = type;
				this.attr = attr;
			}

			public override string ToString()
			{
				if (type == null || attr == null)
					return "(any packet)";
				string dir = (attr.Direction == ePacketDirection.ServerToClient ? "S=>C" : "S<=C");
				string desc = (attr.Description != null ? attr.Description : DefaultPacketDescriptions.GetDescription(attr.Code, attr.Direction));
				return string.Format("{0} 0x{1:X2}: \"{2}\"", dir, attr.Code, desc);
			}
		}

		/// <summary>
		/// This class is used in combo box to show path to single member of a class (packet).
		/// </summary>
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

		/// <summary>
		/// This class shows filter in listbox and does filtering.
		/// </summary>
		internal class FilterListEntry
		{
			public readonly PacketClass packetClass;
			public readonly ClassMemberPath classProperty;
			public readonly string valueToFind;
			public readonly bool isRecursive;
			public readonly string relation;
			public readonly string condition;
			public static readonly ArrayList m_ignoredProperties = new ArrayList();

			static FilterListEntry()
			{
				Type ignoredType = typeof(MemoryStream);
				foreach (PropertyInfo property in ignoredType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					m_ignoredProperties.Add(property);
				}
			}

			public FilterListEntry(PacketClass packetClass, ClassMemberPath classProperty, string valueToFind, bool isRecursive, string relation, string condition)
			{
				this.packetClass = packetClass;
				this.classProperty = classProperty;
				this.valueToFind = valueToFind.ToLower();
				this.isRecursive = isRecursive;
				this.relation = relation;
				this.condition = condition;
			}

			public override string ToString()
			{
				return string.Format("{4,-3} {0} [{1}] {3} \"{2}\"", packetClass, classProperty, valueToFind, relation, condition);
			}

			public bool IsPacketIgnored(Packet packet)
			{
				bool isIgnored = true;
				
				// Filter by packet type
				if (packetClass.type != null && !packetClass.type.IsAssignableFrom(packet.GetType()))
				{
					isIgnored = true;
				}
				else
				{
					int depth = (isRecursive ? RECURSIVE_SEARCH_DEPTH : 1);
					isIgnored = IsValueIgnored(packet, classProperty.members, depth);
				}

				return isIgnored;
			}

			private bool IsValueIgnored(object fieldValue, List<MemberInfo> path, int depth)
			{
				// Limit depth
				if (depth < 0)
				{
					return true;
				}

				bool isIgnored = true;
				
				if (fieldValue != null)
				{
					// Ignore certain types for better performance
					Type valType = fieldValue.GetType();
					if (IsClassIgnored(valType))
					{
						isIgnored = true;
					}
					else if (valType.IsPublic || valType.IsNestedPublic)
					{
						if (path != null)
						{
							// Make sure that depth is same as path length
							if (depth >= path.Count)
							{
								depth = path.Count - 1;
							}
							
							// Get data from property or field
							MemberInfo node = path[path.Count - 1 - depth];
							object data = null;
							if (node is PropertyInfo)
							{
								PropertyInfo propInfo = (PropertyInfo) node;
								data = propInfo.GetValue(fieldValue, null);
							}
							else if (node is FieldInfo)
							{
								FieldInfo fieldInfo = (FieldInfo) node;
								data = fieldInfo.GetValue(fieldValue);
							}
							
							// Check every element of collection
							if (data != null)
							{
								if (!(data is string) && data.GetType().IsArray)
								{
									foreach (object o in (IEnumerable)data)
									{
										isIgnored = IsValueIgnored(o, path, depth - 1);
										if (!isIgnored)
										{
											break;
										}
									}
								}
								else
								{
									// Check read value
									fieldValue = data;
									isIgnored = IsValueIgnored(fieldValue, path, depth - 1);
								}
							}
						}
						else
						{
							// Recursively check all properties of collection
							if (!(fieldValue is string) && fieldValue is IEnumerable)
							{
								foreach (object o in (IEnumerable)fieldValue)
								{
									isIgnored = IsValueIgnored(o, path, depth - 1);

									// Property is not ignored - break the loop
									if (!isIgnored)
									{
										break;
									}
								}
							}

							// Check all object's properties/fields
							else
							{
								// Check all properties
								foreach (PropertyInfo property in valType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
								{
									if (!property.CanRead) continue;
									// Ignore MemoryStream properties because they throw exceptions
//									if (m_ignoredProperties.Contains(property)) continue;
									if (property.DeclaringType.Equals(typeof(MemoryStream))) continue;
									if (property.DeclaringType.Equals(typeof(Stream))) continue;
									if (property.DeclaringType.Equals(typeof(Type))) continue;
									if (property.DeclaringType.Equals(typeof(Object))) continue;
									if (property.GetIndexParameters().GetLength(0) != 0) continue;

									object objPropVal = property.GetValue(fieldValue, null);
									isIgnored = IsValueIgnored(objPropVal, path, depth - 1);

									// Property is not ignored - break the loop
									if (!isIgnored)
									{
										break;
									}
								}

								// Check all fields
								if (isIgnored)
								{
									foreach (FieldInfo field in valType.GetFields(BindingFlags.Instance | BindingFlags.Public))
									{
										if (!field.IsPublic) continue;

										object objPropVal = field.GetValue(fieldValue);
										isIgnored = IsValueIgnored(objPropVal, path, depth - 1);

										// Field is not ignored - break the loop
										if (!isIgnored)
										{
											break;
										}
									}
								}
							}
						}

						// Compare strings
						if (isIgnored && (path == null || depth == 0))
						{
							if (fieldValue is string && valueToFind != ""
							    && ((relation == "==" && ((string)fieldValue).ToLower().IndexOf(valueToFind) != -1)
								   || (relation == "!=" && ((string)fieldValue).ToLower().IndexOf(valueToFind) == -1)))
							{
								// Filter string is a sub-string of packet property
								isIgnored = false;
							}
							else if (fieldValue != null)
							{
								// Packet property string equals filter value
								bool rc = fieldValue.ToString().ToLower().Equals(valueToFind);
								if (rc && relation == "==")
									isIgnored = false;
								else if (!rc && relation == "!=")
									isIgnored = false;
							}
						}
					}
				}
				
				return isIgnored;
			}
		}

		#endregion
	}
}

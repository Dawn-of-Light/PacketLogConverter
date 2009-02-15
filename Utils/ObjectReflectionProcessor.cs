using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PacketLogConverter.Utils
{
	/// <summary>
	/// This class does various processing on objects using reflection.
	/// </summary>
	public struct ObjectReflectionProcessor
	{
		private readonly Predicate<object> action;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ObjectReflectionProcessor"/> class.
		/// </summary>
		/// <param name="action">The action to be executed by methods which support callbacks.</param>
		public ObjectReflectionProcessor(Predicate<object> action)
		{
			this.action = action;
		}

		/// <summary>
		/// Gets the object member value.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="member">The member.</param>
		/// <returns>The value of object member.</returns>
		/// <exception cref="ArgumentException">If <paramref name="member"/> is of unknown type.</exception>
		public object GetObjectMemberValue(object data, MemberInfo member)
		{
			object ret;
			if (member is PropertyInfo)
			{
				PropertyInfo propInfo = (PropertyInfo)member;
				ret = propInfo.GetValue(data, null);
			}
			else if (member is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				ret = fieldInfo.GetValue(data);
			}
			else
			{
				// Unknown member type - throw it away
				string message = "Unknown type of member: " + (null == member ? "(null)" : member.GetType().FullName);
				Log.Error(message);
				throw new ArgumentException(message, "member");
			}

			return ret;
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
			bool ret = classType == typeof(TimeSpan)
				|| classType == typeof(LogPacketAttribute)
//				|| valType == typeof(String)
				|| classType == typeof(ePacketProtocol)
				|| classType == typeof(ePacketDirection)
				|| typeof(Exception).IsAssignableFrom(classType)
				|| typeof(IDictionary).IsAssignableFrom(classType);
//				|| typeof(IList).IsAssignableFrom(classType);

			return ret;
		}

		/// <summary>
		/// Iterates the object tree path values.
		/// </summary>
		/// <remarks>
		/// The <see cref="action"/> is called to process each object of a path (if stored in arrays or collections);
		/// return value indicates if iteration should stop (return <code>true</code>).
		/// </remarks>
		/// <param name="data">The data.</param>
		/// <param name="path">The path.</param>
		/// <param name="maxDepth">The max depth.</param>
		/// <returns><code>true</code> if iteration is stopped by callback.</returns>
		public bool IterateObjectTreePathValues(object data, IList<MemberInfo> path, int maxDepth)
		{
			bool ret = IterateObjectTreePathValuesImpl(data, path, maxDepth, true);
			return ret;
		}

		/// <summary>
		/// Iterates the object tree path values.
		/// </summary>
		/// <remarks>
		/// The <see cref="action"/> is called to process each object of a path (if stored in arrays or collections);
		/// return value indicates if iteration should stop (return <code>true</code>).
		/// </remarks>
		/// <param name="data">The data.</param>
		/// <param name="path">The path.</param>
		/// <param name="depth">The depth.</param>
		/// <param name="isFirstCall"><code>true</code> if first call, <code>false</code> otherwise.</param>
		/// <returns><code>true</code> if iteration is stopped by callback.</returns>
		private bool IterateObjectTreePathValuesImpl(object data, IList<MemberInfo> path, int depth, bool isFirstCall)
		{
			// No path if last element
			if (depth < 0)
			{
				path = null;
			}

			bool stopProcessing = false;

			if (data != null)
			{
				// Ignore certain types for better performance
				Type valType = data.GetType();
				if (!IsClassIgnored(valType) && (valType.IsPublic || valType.IsNestedPublic))
				{
					// depth < 0 if last path element is an array
					if (depth >= 0)
					{
						if (path != null)
						{
							// Make sure that depth is the same as path length
							if (depth >= path.Count)
							{
								depth = path.Count - 1;
							}

							// Get data from property or field
							MemberInfo node = path[path.Count - 1 - depth];
							object fieldValue = GetObjectMemberValue(data, node);

							// Check every element of collection
							if (fieldValue != null)
							{
								if (!(fieldValue is string) && fieldValue.GetType().IsArray)
								{
									foreach (object o in (IEnumerable) fieldValue)
									{
										stopProcessing = IterateObjectTreePathValuesImpl(o, path, depth - 1, false);
										if (stopProcessing)
										{
											break;
										}
									}

									// Don't check .ToString() of arrays - it makes no sense
									if (depth == 0)
									{
										depth = -1;
									}
								}
								else
								{
									// Check read value
									stopProcessing = IterateObjectTreePathValuesImpl(fieldValue, path, depth - 1, false);
								}
							}
						}
						else
						{
							// Recursively check all properties of collection
							if (data is IEnumerable && !(data is string))
							{
								foreach (object o in (IEnumerable) data)
								{
									stopProcessing = IterateObjectTreePathValuesImpl(o, path, depth - 1, false);

									// Property is not ignored - break the loop
									if (stopProcessing)
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
									if (property.DeclaringType.Equals(typeof (MemoryStream))) continue;
									if (property.DeclaringType.Equals(typeof (Stream))) continue;
									if (property.DeclaringType.Equals(typeof (Type))) continue;
									if (property.DeclaringType.Equals(typeof (Object))) continue;
									if (property.GetIndexParameters().GetLength(0) != 0) continue;

									object objPropVal = property.GetValue(data, null);
									stopProcessing = IterateObjectTreePathValuesImpl(objPropVal, path, depth - 1, false);

									// Property is not ignored - break the loop
									if (stopProcessing)
									{
										break;
									}
								}

								// Check all fields
								if (!stopProcessing)
								{
									foreach (FieldInfo field in valType.GetFields(BindingFlags.Instance | BindingFlags.Public))
									{
										if (!field.IsPublic) continue;

										object objPropVal = field.GetValue(data);
										stopProcessing = IterateObjectTreePathValuesImpl(objPropVal, path, depth - 1, false);

										// Field is not ignored - break the loop
										if (stopProcessing)
										{
											break;
										}
									}
								}
							}
						}
					}
				}
			}

			// No path if last element
			if ((depth == 0 || path == null) && !stopProcessing && !isFirstCall)
			{
				stopProcessing = action(data);
			}

			return stopProcessing;
		}
	}
}

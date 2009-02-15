using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using PacketLogConverter.Utils;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// This class contains helper methods for <see cref="PacketPropertyValueFilterForm"/>.
	/// </summary>
	public partial class PacketPropertyValueFilterForm
	{
		/// <summary>
		/// This class shows filter in listbox and does filtering.
		/// </summary>
		[Serializable]
		internal class FilterListEntry : ISerializable
		{
			public readonly PacketPropertyValueFilterForm.PacketClass packetClass;
			public readonly PacketPropertyValueFilterForm.ClassMemberPath classProperty;
			public readonly object searchValue;
			public readonly bool isRecursive;
			public readonly string relation;
			public readonly string condition;

			public FilterListEntry(PacketPropertyValueFilterForm.PacketClass packetClass,
									PacketPropertyValueFilterForm.ClassMemberPath classProperty,
									object searchValue,
									bool isRecursive,
									string relation,
									string condition)
			{
				this.packetClass = packetClass;
				this.classProperty = classProperty;
				this.searchValue = searchValue;
				this.isRecursive = isRecursive;
				this.relation = relation;
				this.condition = condition;
			}

			/// <summary>
			/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
			/// </summary>
			/// <returns>
			/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
			/// </returns>
			public override string ToString()
			{
				string valueToFindDescription = (searchValue is string ? '"' + (string)searchValue + '"' : searchValue.ToString());
				string ret = string.Format("{4,-3} {0} [{1}] {3} {2}", packetClass, classProperty, valueToFindDescription, relation, condition);
				return ret;
			}

			/// <summary>
			/// Deserialization constructor.
			/// </summary>
			/// <param name="info">The info.</param>
			/// <param name="context">The context.</param>
			protected FilterListEntry(SerializationInfo info, StreamingContext context)
			{
				try
				{
					// Support for filters saved before variables were added
					string val = info.GetString("valueToFind");
					long valLong;
					if (Util.ParseLong(val, out valLong))
					{
						searchValue = valLong;
					}
					else
					{
						searchValue = val;
					}
				}
				catch
				{
					// Old field doesn't exist, must be loading new filter
				}

				try
				{
					this.searchValue = info.GetValue("searchValue", typeof(object));
				}
				catch
				{
					// New field doesn't exist - must be loading old filter
				}

				// De-serialize data
				this.packetClass	= (PacketClass)info.GetValue("packetClass", typeof(PacketClass));
				this.classProperty	= (ClassMemberPath)info.GetValue("classProperty", typeof(ClassMemberPath));
				this.isRecursive	= info.GetBoolean("isRecursive");
				this.relation		= info.GetString("relation");
				this.condition		= info.GetString("condition");
			}

			///<summary>
			///Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with the data needed to serialize the target object.
			///</summary>
			///
			///<param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"></see>) for this serialization. </param>
			///<param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> to populate with data. </param>
			///<exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("packetClass",		packetClass,	typeof(PacketClass));
				info.AddValue("classProperty",		classProperty,	typeof(ClassMemberPath));
				info.AddValue("searchValue",		searchValue,	typeof(object));
				info.AddValue("isRecursive",		isRecursive);
				info.AddValue("relation",			relation);
				info.AddValue("condition",			condition);
			}
		}
	}
}

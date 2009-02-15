using System;
using System.Collections;
using System.IO;
using System.Reflection;
using PacketLogConverter.Utils;

namespace PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers
{
	/// <summary>
	/// Common logic for typical value checker.
	/// </summary>
	internal abstract class ObjectValueChecker : IValueChecker<object>
	{
		[NonSerialized]
		private object oldSearchValue;
		[NonSerialized]
		private double valueToFindDouble;
		[NonSerialized]
		private bool canCompareAsDouble;
		[NonSerialized]
		private uint valueToFindInt = 0;
		[NonSerialized]
		private bool canCompareAsInt;

		protected object value;
		protected readonly	PacketPropertyValueFilterForm.FilterListEntry	entry;

		protected static readonly ArrayList m_ignoredProperties = new ArrayList();

		static ObjectValueChecker()
		{
			Type ignoredType = typeof(MemoryStream);
			foreach (PropertyInfo property in ignoredType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				m_ignoredProperties.Add(property);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ObjectValueChecker"/> class.
		/// </summary>
		/// <param name="entry">The entry.</param>
		public ObjectValueChecker(PacketPropertyValueFilterForm.FilterListEntry entry)
		{
			this.entry = entry;
		}

		/// <summary>
		/// Gets the entry.
		/// </summary>
		/// <value>The entry.</value>
		public PacketPropertyValueFilterForm.FilterListEntry Entry
		{
			get { return entry; }
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public abstract void Dispose();

		/// <summary>
		/// Determines whether a packet is ignored by this instance of checker. (Generic interface implementation.)
		/// </summary>
		/// <param name="packetToCheck">The value to check.</param>
		/// <returns>
		/// 	<c>true</c> if value is ignored; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPacketIgnored(Packet packetToCheck)
		{
			bool isIgnored;

			// Filter by packet type
			if (entry.packetClass.type != null && !entry.packetClass.type.IsAssignableFrom(packetToCheck.GetType()))
			{
				isIgnored = true;
			}
			else
			{
				int depth = (entry.isRecursive ? PacketPropertyValueFilterForm.RECURSIVE_SEARCH_DEPTH : 1);
//					isIgnored = IsPacketIgnored(packet, classProperty.members, depth);
				// Check every reachable value
				ObjectReflectionProcessor processor = new ObjectReflectionProcessor(delegate(object o)
					{
						// Continue the search while we find at least single value which is not ignored
						bool ret = !IsPacketIgnoredImpl(o, value);
						return ret;
					});
				isIgnored = !processor.IterateObjectTreePathValues(packetToCheck, entry.classProperty.members, depth);
			}

			return isIgnored;
		}

		/// <summary>
		/// Determines whether single value is ignored.
		/// </summary>
		/// <param name="fieldValue">The field value.</param>
		/// <param name="valueToFind">The value to find.</param>
		/// <returns>
		/// 	<c>true</c> if [is value ignored impl] [the specified field value]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsPacketIgnoredImpl(object fieldValue, object valueToFind)
		{
			bool isIgnored = true;


			bool isEqual;
			IConvertible fieldValueConvertible = fieldValue as IConvertible;
			IConvertible valueToFindConvertible = valueToFind as IConvertible;
//			if (null != fieldValueConvertible && null != valueToFindConvertible)
			{
//				double val1 = fieldValueConvertible.ToDouble(null);
//				double val2 = valueToFindConvertible.ToDouble(null);
//				isEqual = val1 == val2;
			}
//			else
			{
				isEqual = (null == fieldValue ? null == valueToFind : fieldValue.ToString().ToLowerInvariant().Equals(null == valueToFind ? string.Empty : valueToFind.ToString().ToLowerInvariant()));
			}
			// For "!=" condition we ignore only equal field values
			isIgnored = (entry.relation == "!=" ? isEqual : !isEqual);


#warning TODO: Implement decent solution
			//string valueToFindStr = (valueToFind as string ?? string.Empty);
			//if (fieldValue is string && valueToFindStr != ""
			//    && ((entry.relation == "==" && ((string)fieldValue).ToLower().IndexOf(valueToFindStr) != -1)
			//        || (entry.relation == "!=" && ((string)fieldValue).ToLower().IndexOf(valueToFindStr) == -1)))
			//{
			//    // Filter string is a sub-string of packet property
			//    isIgnored = false;
			//}
			//else if (fieldValue != null)
			//{
			//    // Packet property string equals filter value
			//    if (relation == "&&" && canCompareAsInt && (fieldValue is sbyte || fieldValue is short || fieldValue is int || fieldValue is long || fieldValue is byte || fieldValue is ushort || fieldValue is uint || fieldValue is ulong))
			//    {
			//        uint fieldValue2 = Convert.ToUInt32(fieldValue);
			//        isIgnored = ((fieldValue2 & valueToFindInt) == 0); // same as !((a & b) != 0)
			//    }
			//    // Packet property string equals filter value
			//    else if (relation == "!&" && canCompareAsInt && (fieldValue is sbyte || fieldValue is short || fieldValue is int || fieldValue is long || fieldValue is byte || fieldValue is ushort || fieldValue is uint || fieldValue is ulong))
			//    {
			//        uint fieldValue2 = Convert.ToUInt32(fieldValue);
			//        isIgnored = ((fieldValue2 & valueToFindInt) != 0); // same as !((a & b) == 0)
			//    }
			//    else if (relation == "&=" && canCompareAsInt && (fieldValue is sbyte || fieldValue is short || fieldValue is int || fieldValue is long || fieldValue is byte || fieldValue is ushort || fieldValue is uint || fieldValue is ulong))
			//    {
			//        uint fieldValue2 = Convert.ToUInt32(fieldValue);
			//        isIgnored = (fieldValue2 & valueToFindInt) != valueToFindInt; // same as !(a & b) == b
			//    }
			//    else if (canCompareAsDouble && (fieldValue is sbyte || fieldValue is short || fieldValue is int || fieldValue is long || fieldValue is byte || fieldValue is ushort || fieldValue is uint || fieldValue is ulong || fieldValue is double || fieldValue is float))
			//    {
			//        double fieldValue2 = Convert.ToDouble(fieldValue);
			//        switch (relation)
			//        {
			//            case "==":
			//                isIgnored = fieldValue2 != valueToFindDouble; // same as !(a == b)
			//                break;
			//            case ">":
			//                isIgnored = fieldValue2 <= valueToFindDouble; // same as !(a > b)
			//                break;
			//            case "<":
			//                isIgnored = fieldValue2 >= valueToFindDouble; // same as !(a < b)
			//                break;
			//            case "!=":
			//                isIgnored = fieldValue2 == valueToFindDouble; // same as !(a != b)
			//                break;
			//            default:
			//                break;
			//        }
			//    }
			//    else
			//    {
			//        bool rc = fieldValue.ToString().ToLower().Equals(valueToFind);
			//        if (rc && relation == "==")
			//            isIgnored = false;
			//        else if (!rc && relation == "!=")
			//            isIgnored = false;
			//    }
			//}

			return isIgnored;
		}

		protected void InitNonSerialized()
		{
			object valueToFind = value;
			if (valueToFind != null)
			{
				try
				{
					this.valueToFindDouble = double.Parse(valueToFind.ToString());
					this.canCompareAsDouble = true;
				}
				catch { }
				try
				{
					this.valueToFindInt = uint.Parse(valueToFind.ToString());
					this.canCompareAsInt = true;
				}
				catch { }
			}
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PacketLogConverter.Utils.Serialization
{
	/// <summary>
	/// This class contains helper methods for deserialization.
	/// </summary>
	public struct DeserializationHelper
	{
		private readonly BinaryReader reader;
		private readonly BinaryFormatter serializer;

		public DeserializationHelper(BinaryReader reader)
		{
			this.reader = reader;
			this.serializer = new BinaryFormatter();
		}

		/// <summary>
		/// Deserializes the list.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <returns><code>true</code> if data existed in stream.</returns>
		public bool DeserializeList(IList list)
		{
			// Check if all data is read
			bool ret = reader.BaseStream.Position < reader.BaseStream.Length;

			if (ret)
			{
				for (int i = reader.ReadInt32(); i > 0; i--)
				{
					object o = serializer.Deserialize(reader.BaseStream);
					list.Add(o);
				}
			}

			return ret;
		}
	}
}

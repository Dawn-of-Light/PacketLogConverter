using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using PacketLogConverter.LogFilters;

namespace PacketLogConverter.Utils.Serialization
{
	/// <summary>
	/// This class contains helper methods for serialization.
	/// </summary>
	public struct SerializationHelper
	{
		private readonly BinaryFormatter	serializer;
		private readonly BinaryWriter		writer;
		private readonly MemoryStream		stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SerializationHelper"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="writer">The writer.</param>
		public SerializationHelper(MemoryStream stream, BinaryWriter writer)
		{
			// Check params
			if (null == stream)
			{
				throw new ArgumentNullException("stream", "Output stream is required");
			}

			this.stream = stream;
			this.writer = writer;
			this.serializer = new BinaryFormatter();
		}

		/// <summary>
		/// Serializes the collection.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public void SerializeCollection(ICollection collection)
		{
			// Count of entries
			writer.Write(collection.Count);
			writer.Flush();

			// Content of entries
			foreach (object o in collection)
			{
				// Seralize each object
				serializer.Serialize(stream, o);
			}
		}
	}
}
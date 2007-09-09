using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PacketLogConverter.Utils
{
	public class FilterReader : FilterStreamBase<FileStream>
	{
		protected BinaryReader	m_Reader;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FilterReader"/> class.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public FilterReader(string filePath)
		{
			m_Stream = new FileStream(filePath, FileMode.Open);
			m_Reader = new BinaryReader(m_Stream);
		}

		#region Read methods
		
		/// <summary>
		/// Processes the header. Must be called before stream is closed and after everything is saved.
		/// </summary>
		public override void ProcessHeader()
		{
			base.ProcessHeader();

			// Check file version
			int version = m_Reader.ReadInt32();
			if (s_Version != version)
			{
				throw new InvalidOperationException("Wrong file version.");
			}
			
			// Get static data
			CombineFilters	= m_Reader.ReadBoolean();
			InvertCheck		= m_Reader.ReadBoolean();
		}

		/// <summary>
		/// Reads the filters.
		/// </summary>
		/// <param name="availableFilters">The available filters.</param>
		/// <returns>List with loaded filters.</returns>
		public List<ILogFilter> ReadFilters(ICollection availableFilters)
		{
			List<ILogFilter> ret = new List<ILogFilter>(availableFilters.Count);

			// Read all filters
			for (int i = m_Reader.ReadInt32(); 0 < i; i--)
			{
				// Find filter instance
				string typeName = m_Reader.ReadString();
				ILogFilter filter = FindLogFilter(typeName, availableFilters);

				try
				{
					// Read filter data from file
					int size = m_Reader.ReadInt32();
					byte[] data = new byte[size];
					m_Reader.Read(data, 0, size);
					
					// Create data wrapper
					MemoryStream filterData = new MemoryStream(data);

					// Try to deserialize data
					if (null == filter || !filter.Deserialize(filterData))
					{
						// Skip filter data
						m_Reader.BaseStream.Seek(size, SeekOrigin.Current);
					}
					else
					{
						// Read successfully
						ret.Add(filter);
					}
				}
				catch (Exception e)
				{
					throw new Exception("Error loading filter '" + (typeName ?? "(NULL)") + "'", e);
				}
			}
			
			return ret;
		}

		/// <summary>
		/// Finds the log filter in a collection.
		/// </summary>
		/// <param name="typeName">Name of the type.</param>
		/// <param name="availableFilters">The available filters.</param>
		/// <returns></returns>
		private ILogFilter FindLogFilter(string typeName, ICollection availableFilters)
		{
			ILogFilter ret = null;
			
			foreach (ILogFilter filter in availableFilters)
			{
				if (filter.GetType().FullName == typeName)
				{
					ret = filter;
					break;
				}
			}
			
			return ret;
		}

		#endregion
	}
}

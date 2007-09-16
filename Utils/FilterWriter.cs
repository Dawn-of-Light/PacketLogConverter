using System;
using System.Collections;
using System.IO;
using PacketLogConverter;

namespace PacketLogConverter.Utils
{
	/// <summary>
	/// This class writes filters to a file. This class is not thread safe.
	/// </summary>
	public class FilterWriter : FilterStreamBase<FileStream>
	{
		protected BinaryWriter	m_Writer;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:StreamWrap"/> class.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public FilterWriter(string filePath)
		{
			m_Stream = new FileStream(filePath, FileMode.Create);
			m_Writer = new BinaryWriter(m_Stream);
		}
		
		#region Write methods
		
		/// <summary>
		/// Writes the header. Must be called before any filters are saved.
		/// </summary>
		public override void ProcessHeader()
		{
			base.ProcessHeader();

			// Write version
			m_Writer.Write(s_Version);
			
			// Write static data
			m_Writer.Write(CombineFilters);
			m_Writer.Write(InvertCheck);
		}

		/// <summary>
		/// Truncates the file. Must be called before stream is closed and after everything is saved.
		/// </summary>
		public override void ProcessEpilogue()
		{
			// Truncate file to writen data
			m_Writer.Flush();
			m_Writer.BaseStream.SetLength(m_Writer.BaseStream.Position);

			base.ProcessEpilogue();
		}

		/// <summary>
		/// Writes the filters to stream.
		/// </summary>
		/// <param name="filters">Collection of <see cref="ILogFilter"/> objects to write.</param>
		public virtual void WriteFilters(ICollection filters)
		{
			// Write count of filters
			m_Writer.Write(filters.Count);
			
			foreach (ILogFilter filter in filters)
			{
				try
				{
					// Create temp space for filter data
					MemoryStream filterData = new MemoryStream(0x100);

					// Serialize filter data
					if (filter.Serialize(filterData))
					{
						// Save filter type. Assume that all filters are in the same assembly.
						m_Writer.Write(filter.GetType().FullName);

						// Save filter data
						byte[] data = filterData.GetBuffer();
						m_Writer.Write(data.Length);
						m_Writer.Write(data, 0, data.Length);
					}
				}
				catch (Exception e)
				{
					Log.Error("Error writing filter '" + filter.GetType().FullName + "'", e);
				}
			}
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Disposes an instance of this class. Called only once.
		/// </summary>
		/// <param name="isDisposing">if set to <c>true</c> if disposing.</param>
		protected override void Dispose(bool isDisposing)
		{
			m_Writer.Flush();
			
			base.Dispose(isDisposing);
		}

		#endregion
	}
}

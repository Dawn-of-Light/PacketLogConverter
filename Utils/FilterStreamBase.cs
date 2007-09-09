using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacketLogConverter.Utils
{
	/// <summary>
	/// Base class for filter reader and writer. This class is not thread safe.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class FilterStreamBase<T> : StreamWrap<T>
		where T : Stream
	{
		/// <summary>
		/// Version of filter reader/writer.
		/// </summary>
		public static readonly int s_Version = 1;

		protected bool	m_HeaderProcessed;
		protected bool	m_EpilogueProcessed;

		// Static data
		bool	m_CombineFilters;
		bool	m_InvertCheck;

		#region Public access properties

		/// <summary>
		/// Gets or sets a value indicating whether to combine filters.
		/// </summary>
		/// <value><c>true</c> if [combine filters]; otherwise, <c>false</c>.</value>
		public bool CombineFilters
		{
			get { return m_CombineFilters; }
			set { m_CombineFilters = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether invert filter check.
		/// </summary>
		/// <value><c>true</c> if [invert check]; otherwise, <c>false</c>.</value>
		public bool InvertCheck
		{
			get { return m_InvertCheck; }
			set { m_InvertCheck = value; }
		}

		#endregion

		#region Processing methods
		
		/// <summary>
		/// Processes the header. Must be called before stream is closed and after everything is saved.
		/// </summary>
		public virtual void ProcessHeader()
		{
			// Safety checks
			if (0 != m_Stream.Position)
			{
				throw new InvalidOperationException("Stream is not at the very beginning (position != 0)");
			}
			
			m_HeaderProcessed = true;
		}

		/// <summary>
		/// Processes the epilogue. Must be called before stream is closed and after everything is saved.
		/// </summary>
		public virtual void ProcessEpilogue()
		{
			m_EpilogueProcessed = true;
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Disposes an instance of this class. Called only once.
		/// </summary>
		/// <param name="isDisposing">if set to <c>true</c> if disposing.</param>
		protected override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);

			// Safety checks
			if (!m_HeaderProcessed || !m_EpilogueProcessed)
			{
				throw new InvalidOperationException("Either header (" + m_HeaderProcessed
													+ ") or epilogue (" + m_EpilogueProcessed
													+ ") is not processed before stream is closed.");
			}
		}

		#endregion
	}
}

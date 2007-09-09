using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace PacketLogConverter.Utils
{
	/// <summary>
	/// Bse class for all stream wrappers.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class StreamWrap<T> : IDisposable
		where T : Stream
	{
		protected T			m_Stream;
		protected object	m_Disposed;

		#region Dispose

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="T:PacketLogConverter.Utils.StreamWrap"/> is reclaimed by garbage collection.
		/// </summary>
		~StreamWrap()
		{
			DisposeInt(false);
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public virtual void Dispose()
		{
			DisposeInt(true);
		}

		/// <summary>
		/// Disposes an instance of this class. Helper method to avoid code duplication.
		/// </summary>
		/// <param name="isDisposing">if set to <c>true</c> if disposing.</param>
		private void DisposeInt(bool isDisposing)
		{
			// If not disposed already
			if (null == Interlocked.Exchange(ref m_Disposed, this))
			{
				try
				{
					Dispose(isDisposing);
				}
				catch (Exception e)
				{
					Log.Error("Disposing stream wrap", e);
				}
			}
		}

		/// <summary>
		/// Disposes an instance of this class. Called only once.
		/// </summary>
		/// <param name="isDisposing">if set to <c>true</c> if disposing.</param>
		protected virtual void Dispose(bool isDisposing)
		{
			// Release resources
			m_Stream.Flush();
			m_Stream.Dispose();
			
			// Don't need finalizer anymore
			GC.SuppressFinalize(this);
		}
		
		#endregion
	}
}

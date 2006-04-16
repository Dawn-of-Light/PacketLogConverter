using System;
using System.IO;
using System.Threading;

namespace PacketLogConverter
{
	public class BufferedStringReader : IDisposable
	{
		private volatile Thread m_thread;
		private volatile int m_firstUncached;
		private volatile int m_currentLine;
		private readonly Stream m_stream;
		private readonly string[] m_lines;
		private readonly int m_indexMask;
		private readonly AutoResetEvent m_event;

		public BufferedStringReader(Stream stream, int bufferSizeBits)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			m_currentLine = -1;
			m_stream = stream;
			m_indexMask = (1 << bufferSizeBits) - 1;
			m_lines = new string[1 << bufferSizeBits];
			m_event = new AutoResetEvent(false);
			StartThread();
		}

		private void StartThread()
		{
			m_thread = new Thread(new ThreadStart(BufferingThread));
			m_thread.IsBackground = true;
			m_thread.Name = "StringCache";
			m_thread.Start();
		}

		private void BufferingThread()
		{
			try
			{
				StreamReader s = new StreamReader(m_stream);
				{
					do
					{
						int lastLine = m_firstUncached;
						int count = m_lines.Length - (lastLine - m_currentLine);
						if (count <= 0)
						{
							Thread.Sleep(1);
//							m_event.WaitOne();
							continue;
						}

						for (int i = 0; i < count; i++)
						{
							string line = s.ReadLine();
							if (line == null)
							{
								m_thread = null;
								m_firstUncached = lastLine + i;
								return;
							}
							m_lines[(lastLine + i) & m_indexMask] = line;
//							m_event.Set();
						}

						m_firstUncached = lastLine + count;
					} while (true);
				}
			}
			catch (ThreadAbortException)
			{
			}
		}

		public static int moveNextSpin;
		public bool MoveNext()
		{
			int cur = m_currentLine + 1;
			while (cur >= m_firstUncached)
			{
				if (m_thread == null)
					return false;
				
				int start = Environment.TickCount;
//				m_event.Set();
//				m_event.WaitOne();
				Thread.Sleep(1);
//				Thread.SpinWait(1);
				moveNextSpin += Environment.TickCount - start;
			}
			m_currentLine = cur;
			return true;
		}

		public string Current
		{
			get { return m_lines[m_currentLine & m_indexMask]; }
		}

		protected virtual void Dispose(bool disposing)
		{
			if (m_thread != null)
				m_thread.Abort();
			if (disposing)
				GC.SuppressFinalize(this);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		~BufferedStringReader()
		{
			Dispose(false);
		}
	}
}

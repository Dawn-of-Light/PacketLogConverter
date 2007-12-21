using System.IO;

namespace PacketLogConverter
{
	/// <summary>
	/// Writes a log to a stream.
	/// </summary>
	public interface ILogWriter
	{
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback);
	}
}

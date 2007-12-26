using System.Collections.Generic;
using System.IO;

namespace PacketLogConverter
{
	/// <summary>
	/// The interface for all log parsers
	/// </summary>
	public interface ILogReader
	{
		/// <summary>
		/// Reads the log.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback.</param>
		/// <returns>Found packets.</returns>
		ICollection<Packet> ReadLog(Stream stream, ProgressCallback callback);
	}
}

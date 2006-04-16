using System.Collections;
using System.IO;

namespace PacketLogConverter
{
	/// <summary>
	/// The interface for all log parsers
	/// </summary>
	public interface ILogReader
	{
		ICollection ReadLog(Stream stream, ProgressCallback callback);
	}
}

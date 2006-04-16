using System.IO;

namespace PacketLogConverter
{
	public interface ILogWriter
	{
		void WriteLog(PacketLog log, Stream stream, ProgressCallback callback);
	}
}

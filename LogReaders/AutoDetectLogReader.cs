using System;
using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogReaders
{
	/// <summary>
	/// Use detected log reader, if any.
	/// </summary>
	[LogReader("autodetect log type", "*.log;*.plc", Priority=100000)]
	public class AutoDetectLogReader : ILogReader
	{
		public ICollection ReadLog(Stream stream, ProgressCallback callback)
		{
			string line;
			ILogReader reader = null;
			long orgPos = stream.Position;
			
			using (StreamReader text = new StreamReader(stream, Encoding.ASCII))
			using (BinaryReader bin = new BinaryReader(stream, Encoding.ASCII))
			{
				int i = 0;
				while ((line = text.ReadLine()) != null && i++ < 100)
				{
					if (callback != null && (i & 0xF) == 0)
						callback(i, 100);
					
					if (line.Length <= 0)
						continue;
					
					if (line[0] == '<' && (line = text.ReadLine()) != null)
					{
						if (line.Length < 16*3)
							continue;
						
						for (int x = 2; x < 15*3-1; x+=3)
						{
							if (line[x] != ' ')
							{
								reader = new DaocLoggerV3BinaryLogReader();
								break;
							}
						}
						if (reader == null)
							reader = new DaocLoggerV3TextLogReader();
						break;
					}
				}
				
				stream.Position = orgPos;
				
				if (reader == null)
				{
					line = bin.ReadString();
					if (line != null
						&& line.Length > 0
						&& line[line.Length - 1] == ']'
						&& line.StartsWith("[PacketLogConverter v"))
					{
						int startLen = "[PacketLogConverter v".Length;
						string ver = line.Substring(startLen, line.Length - startLen - 1);
						switch (ver)
						{
							case "1":
								reader = new PacketLogConverterV1LogReader();
								break;
								
							default:
								throw new Exception("Unknown \"PLC\" log version: " + ver);
						}
					}
				}
				
				stream.Position = orgPos;
			
				if (reader != null)
					return reader.ReadLog(stream, callback);
				
				return new DolServerV17LogReader().ReadLog(stream, callback);
			}
		}
	}
}

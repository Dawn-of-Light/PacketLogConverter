using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

namespace PacketLogConverter.LogReaders
{
	/// <summary>
	/// Reads own format logs.
	/// </summary>
	[LogReader("PacketLogConverter v1", "*.plc", Priority=5000)]
	public class PacketLogConverterV1LogReader : ILogReader
	{
		public ICollection ReadLog(Stream stream, ProgressCallback callback)
		{
			Type[] constrType = new Type[] {typeof (int)};
			object[] param = new object[1];
			Assembly asm = typeof (Packet).Assembly;
			ArrayList packets = new ArrayList();
			ArrayList ignored = new ArrayList();
			Hashtable pakByName = new Hashtable(512);
			int counter = 0;
			
			using (BinaryReader s = new BinaryReader(stream, Encoding.ASCII))
			{
				if (callback != null)
					callback(0, (int) stream.Length);
				
				string header = s.ReadString();
				if (header != "[PacketLogConverter v1]")
					throw new Exception("Wrong log format: " + header);
				
				while (stream.Position < stream.Length)
				{
					if (callback != null && (counter++ & 0xFFF) == 0) // update progress every 4k packets
						callback((int)(stream.Position>>10),
							(int)(stream.Length>>10));
					
					Packet p;
					int size = s.ReadUInt16();
					string typeName = s.ReadString();
					
					try
					{
						PacketData d = (PacketData) pakByName[typeName];
						if (d == null)
						{
							Type t = asm.GetType(typeName);
							d = new PacketData();
							d.constructor = t.GetConstructor(constrType);
							d.attr = PacketManager.GetPacketTypeAttribute(t);
							pakByName[typeName] = d;
						}
						param[0] = size;
						p = (Packet) d.constructor.Invoke(param);
						p.Attribute = d.attr;
					}
					catch (Exception e)
					{
						Log.Error("instantiate: " + typeName, e);
						ignored.Add(typeName + ": " + e.Message);
						stream.Position += size + 12;
						continue;
					}
					
					p.Code = s.ReadUInt16(); // 2 bytes
					p.Direction = (ePacketDirection) s.ReadByte(); // 1 byte
					p.Protocol = (ePacketProtocol) s.ReadByte(); // 1 byte
					p.Time = new TimeSpan(s.ReadInt64()); // 8 bytes
					p.Write(s.ReadBytes(size), 0, size);
					p.AllowClassChange = false;
					
					packets.Add(p);
				}
				
				if (ignored.Count > 0)
				{
					StringBuilder str = new StringBuilder("Ignored packets (count " + ignored.Count.ToString() + "):\n\n");
					for (int i = 0;; i++)
					{
						str.Append(ignored[i]).Append('\n');
						if (i > 16)
						{
							str.Append("...\n");
							str.Append("and ").Append(ignored.Count - i + 1).Append(" more.");
							break;
						}
					}
					Log.Info(str.ToString());
				}
				
				return packets;
			}
		}
		
		private class PacketData
		{
			public ConstructorInfo constructor;
			public LogPacketAttribute attr;
		}
	}
}

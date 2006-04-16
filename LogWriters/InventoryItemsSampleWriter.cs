using System.IO;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("Inventory items writer sample", "*.txt")]
	public class InventoryItemsSampleWriter : ILogWriter
	{
		public void WriteLog(PacketLog log, Stream stream, ProgressCallback callback)
		{
			using (StreamWriter s = new StreamWriter(stream))
			{
				for (int i = 0; i < log.Count; i++)
				{
					if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
						callback(i, log.Count-1);

					StoC_0x02_InventoryUpdate invUpdate = log[i] as StoC_0x02_InventoryUpdate;
					if (invUpdate == null) continue;

					foreach (StoC_0x02_InventoryUpdate.Item item in invUpdate.Items)
					{
						s.WriteLine("level={0,-2} model={1,-5} name={2}", item.level, item.model, item.name);
					}
				}
			}
		}
	}
}

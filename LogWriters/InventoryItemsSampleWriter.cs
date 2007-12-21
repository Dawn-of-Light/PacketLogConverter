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
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback)
		{
			using (StreamWriter s = new StreamWriter(stream))
			{
				foreach (PacketLog log in context.LogManager.Logs)
				{
					for (int i = 0; i < log.Count; i++)
					{
						if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
							callback(i, log.Count - 1);

						StoC_0x02_InventoryUpdate invUpdate = log[i] as StoC_0x02_InventoryUpdate;
						if (invUpdate == null) continue;

						foreach (StoC_0x02_InventoryUpdate.Item item in invUpdate.Items)
						{
							if (item.name != null && item.name != "")
								s.WriteLine(
									"level={0,-2} value1:{1,-3} value2:{2,-3} damageType:{3} objectType:{4,-2} weight:{5,-3} model={6,-5} color:{7,-3} effect:{8,-3} name={9}",
									item.level, item.value1, item.value2, item.damageType, item.objectType, item.weight, item.model, item.color,
									item.effect, item.name);
						}
					}
				}
			}
		}
	}
}

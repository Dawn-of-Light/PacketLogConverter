namespace PacketLogConverter
{
	/// <summary>
	/// Packet with OID data
	/// </summary>
	public interface IOidPacket
	{
		int Oid1 { get; }
		int Oid2 { get; }
	}
}

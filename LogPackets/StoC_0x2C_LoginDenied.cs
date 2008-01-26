using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2C, -1, ePacketDirection.ServerToClient, "Login denied")]
	public class StoC_0x2C_LoginDenied: Packet
	{
		protected byte errorCode;
		protected byte isSI;
		protected byte majorVersion;
		protected byte minorVersion;
		protected byte build;

		#region public access properties

		public byte ErrorCode { get { return errorCode; } }
		public byte IsSi { get { return isSI; } }
		public byte MajorVersion { get { return majorVersion; } }
		public byte MinorVersion { get { return minorVersion; } }
		public byte Build { get { return build; } }

		public enum eLoginError : byte
		{
			WrongPassword = 0x01,
			AccountInvalid = 0x02,
			AutheorizationServerUnavailable = 0x03,
			ClientVersionTooLow = 0x05,
			CannotAccessUserAccount = 0x06,
			AccountNotFound = 0x07,
			AccountNoAccessAnyGame = 0x08,
			AccountNoAccessThisGame = 0x09,
			AccountClosed = 0x0a,
			AccountAlreadyLoggedIn = 0x0b,
			TooManyPlayersLoggedIn = 0x0c,
			GameCurrentlyClosed = 0x0d,
			AccountAlreadyLoggedIntoOtherServer = 0x10,
			AccountIsInLogoutProcedure = 0x11,
			ExpansionPacketNotAllowed = 0x12,
			AccountIsBannedFromThisServerType = 0x13,
			CafeIsOutOfPlayingTime = 0x14,
			PersonalAccountIsOutOfTime = 0x15,
			CafesAccountIsSuspended = 0x16,
			NotAuthorizedToUseExpansionVersion = 0x17,
			ServiceNotAvailable = 0xaa
		};
		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("errorCode:{0}({5}) isSI:0x{1:X2} majorVersion:{2} minorVersion:{3} build:{4}",
			                 errorCode, isSI, majorVersion, minorVersion, build, (eLoginError)errorCode);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			errorCode = ReadByte();
			isSI = ReadByte();
			majorVersion = ReadByte();
			minorVersion = ReadByte();
			build = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x2C_LoginDenied(int capacity) : base(capacity)
		{
		}
	}
}
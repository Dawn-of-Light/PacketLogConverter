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
			WrongPassword = 0x01, // "Your Password is Incorrect!"
			AccountInvalid = 0x02, // "Your Account is Invalid!"
			AutheorizationServerUnavailable = 0x03, // "The Authorization Server is Unavailable!"
			ClientVersionTooLow = 0x05,
			CannotAccessUserAccount = 0x06, // "Error Accessing User Account!"
			AccountNotFound = 0x07, // "No Record for User Found!"
			AccountNoAccessAnyGame = 0x08, // "Your account has no access to any games!"
			AccountNoAccessThisGame = 0x09, // "Your account has no access to this game!"
			AccountClosed = 0x0a, // "Your account has been closed!"
			AccountAlreadyLoggedIn = 0x0b, // "Your account is already logged in!"
			TooManyPlayersLoggedIn = 0x0c, // "Too many Players are logged in. Please try later."
			GameCurrentlyClosed = 0x0d, // "The game is currently closed. Please try later."
			AccountAlreadyLoggedIntoOtherServer = 0x10, // "Your account is already logged in to a different server!"
			AccountIsInLogoutProcedure = 0x11, // "Your account is being logged out! Try back in 1 minute."
			ExpansionPacketNotAllowed = 0x12, // "You have not been invited to join this server type."
			AccountIsBannedFromThisServerType = 0x13, // "Your account is banned from this server type."
			CafeIsOutOfPlayingTime = 0x14, // "The Cafe you are connecting through is out of playing time."
			PersonalAccountIsOutOfTime = 0x15, // "Your personal account is out of time."
			CafesAccountIsSuspended = 0x16, // "The Cafe's account is suspended."
			NotAuthorizedToUseExpansionVersion = 0x17, // "You are not authorized to use the expansion version!"
			ServiceNotAvailable = 0xaa // all other
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
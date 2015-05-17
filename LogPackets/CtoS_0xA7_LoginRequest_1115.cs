/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */

using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 1115, ePacketDirection.ClientToServer, "Login request v1115")]
	public class CtoS_0xA7_LoginRequest_1115 : CtoS_0xA7_LoginRequest_1104
	{
		protected byte revision;
		protected ushort build;

		public byte Revision { get { return revision; } }
		public ushort Build { get { return build; } }

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" revision:{0} build:{1}", Revision, Build);
		}

		public override void Init()
		{
			clientType = ReadByte();
			clientVersionMajor = ReadByte();
			clientVersionMinor = ReadByte();
			clientVersionBuild = ReadByte();
			revision = ReadByte();
			build = ReadShort();
			clientAccountName = ReadString(ReadShortLowEndian());
			clientAccountPassword = ReadString(ReadShortLowEndian());
			AunkI = new uint[0];
		}
		
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_1115(int capacity) : base(capacity)
		{
		}
	}
}
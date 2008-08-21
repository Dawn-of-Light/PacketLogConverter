using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.Utils
{
	/// <summary>
	/// Keeps track of packet description location in text, locates packet by text index.
	/// </summary>
	public class PacketTextLocationManager
	{
		private PacketInfo[]	m_packetInfos = new PacketInfo[0];
		private int					visiblePacketsCount;

		/// <summary>
		/// Gets or sets the capacity.
		/// </summary>
		/// <value>The capacity.</value>
		public int Capacity
		{
			get
			{
				return m_packetInfos.Length;
			}
			set
			{
				if (m_packetInfos.Length != value)
				{
					// Create new pairs, copy existing data
					PacketInfo[] newPairs = new PacketInfo[value];
					Array.Copy(m_packetInfos, newPairs, Math.Min(m_packetInfos.Length, newPairs.Length));
					m_packetInfos = newPairs;

					// Correct visible packets count
					visiblePacketsCount = Math.Min(visiblePacketsCount, m_packetInfos.Length);
				}
			}
		}

		/// <summary>
		/// Gets or sets the visible packets count. Should be less or equal to capacity.
		/// </summary>
		/// <value>The visible packets count.</value>
		public int VisiblePacketsCount
		{
			get { return visiblePacketsCount; }
			set
			{
				if (m_packetInfos.Length < value)
				{
					throw new ArgumentException("Count of visible packets should be less or equal to capacity");
				}
				visiblePacketsCount = value;
			}
		}

		/// <summary>
		/// Sets the visible packet.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="packetInfo">The packet info.</param>
		public void SetVisiblePacket(int index, PacketInfo packetInfo)
		{
			if (index >= visiblePacketsCount)
			{
				throw new ArgumentOutOfRangeException("index", index, "Index of visible packets should be less that count of visible packets");
			}
			if (index != 0 && packetInfo.TextEndIndex <= m_packetInfos[index - 1].TextEndIndex)
			{
				throw new ArgumentException("New text index should be greater than previous");
			}

			// Store packet info
			m_packetInfos[index] = packetInfo;
		}

		/// <summary>
		/// Finds the text index by packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>-1 if nothing found, text index if packet found.</returns>
		public int FindTextIndexByPacket(Packet packet)
		{
			int ret = -1;

			int count = 0;
			foreach (PacketInfo pair in m_packetInfos)
			{
				// Is packet found?
				if (pair.Packet == packet)
				{
					ret = count > 0 ? m_packetInfos[count - 1].TextEndIndex : 0;
//					ret = pair.TextEndIndex;
					break;
				}

				// Limit by visible packets count
				if (visiblePacketsCount <= ++count)
				{
					break;
				}
			}

			return ret;
		}

		/// <summary>
		/// Finds the index of the packet by text.
		/// </summary>
		/// <param name="textIndex">Index of the text.</param>
		/// <returns>Found <see cref="PacketInfo"/> or <see cref="PacketInfo.UNKNOWN"/>.</returns>
		public PacketInfo FindPacketInfoByTextIndex(int textIndex)
		{
//			return FindPacketInfoByTextIndexLinear(textIndex);
			return FindPacketInfoByTextIndexBinarySearch(textIndex);
		}

		/// <summary>
		/// Finds the packet by text index. Linear implementation.
		/// </summary>
		/// <param name="textIndex">Index of the text.</param>
		/// <returns>Found <see cref="PacketInfo"/> or <see cref="PacketInfo.UNKNOWN"/>.</returns>
		private PacketInfo FindPacketInfoByTextIndexLinear(int textIndex)
		{
			int index = 0;
			bool flagFound = false;
			// Limit by visible packets count
			for (; visiblePacketsCount > index && m_packetInfos.Length > index; index++)
			{
				// Is text index of current packet greater than requested?
				if (m_packetInfos[index].TextEndIndex > textIndex)
				{
					flagFound = true;
					break;
				}
			}

			PacketInfo ret = (flagFound ? m_packetInfos[index] : PacketInfo.UNKNOWN);
			return ret;
		}

		internal class TextIndexComparer: IComparer
		{
			public int Compare(object x, object y)
			{
				return ((PacketInfo)x).TextEndIndex - (int)y;
			}
		}

		/// <summary>
		/// Finds the packet by text index binary search method.
		/// </summary>
		/// <param name="textIndex">Index of the text.</param>
		/// <returns>Found <see cref="PacketInfo"/> or <see cref="PacketInfo.UNKNOWN"/>.</returns>
		private PacketInfo FindPacketInfoByTextIndexBinarySearch(int textIndex)
		{
            int lastIndex = Math.Min(m_packetInfos.Length, visiblePacketsCount);
            if (lastIndex <= 0 || textIndex >= m_packetInfos[lastIndex - 1].TextEndIndex)
			{
				return PacketInfo.UNKNOWN;
			}
			int index = Array.BinarySearch(m_packetInfos, 0, lastIndex, textIndex + 1, new TextIndexComparer()); // textIndex + 1 becouse last symbol is '\n' and it showed on next line
			return (index >= 0 ? m_packetInfos[index] : m_packetInfos[~index]);
		}

		/// <summary>
		/// Finds the packet by text index binary. Binary search implementation.
		/// </summary>
		/// <param name="textIndex">Index of the text.</param>
		/// <returns>Found <see cref="PacketInfo"/> or <see cref="PacketInfo.UNKNOWN"/>.</returns>
		private PacketInfo FindPacketInfoByTextIndexBinary(int textIndex)
		{
			// TODO: Finish
			int index = visiblePacketsCount/2, prevIndex = int.MinValue;
			bool smallerFound = false, biggerFound = false;

			// Do binary search - find closest (smaller) or equal value
			for (;;)
			{
				if (m_packetInfos[index].TextEndIndex <= textIndex)
				{
					smallerFound = true;
					if (biggerFound)
					{
						// Move left if bigger is already found - need to find smaller value
						index = BinaryMoveLeft(index);
					}
					else
					{
						// Move right if smaller value is found - need to find bigger value
						index = BinaryMoveRight(index);
					}
				}
				else
				{
					biggerFound = true;
				}

				// Safety check
				if (index == prevIndex)
				{
					throw new InvalidOperationException("index is equal to prev index - infinite loop");
				}

				prevIndex = index;
			}

			return PacketInfo.UNKNOWN;
		}

		/// <summary>
		/// Moves current position to the left.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>New index.</returns>
		private int BinaryMoveLeft(int index)
		{
			index = index / 2;
			return index;
		}

		/// <summary>
		/// Move current position to the right.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>New index.</returns>
		private int BinaryMoveRight(int index)
		{
			index = index + (visiblePacketsCount - index) / 2;
			return index;
		}
	}
}

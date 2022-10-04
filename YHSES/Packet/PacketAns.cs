using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YRCC.Packet
{
    class PacketAns
    {
        readonly PacketHeader header;
        public readonly byte service;
        public readonly byte status;
        public readonly byte added_status_size;
        public readonly ushort added_status;
        public readonly byte[] data;

        public PacketAns(byte[] _packet)
        {
            header = new PacketHeader(_packet);
            service = _packet[24];
            status = _packet[25];
            added_status_size = _packet[26];
            added_status = BitConverter.ToUInt16(_packet, 28);
            data = _packet.Skip(PacketHeader.HEADER_SIZE).Take(header.data_size).ToArray();
        }

        /// <summary>
        /// For debug purpose.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return header.ToBytes()
                .Concat(new byte[] { service, status, added_status_size, PacketHeader.HEADER_PADDING_U8, })
                .Concat(BitConverter.GetBytes(added_status))
                .Concat(BitConverter.GetBytes(PacketHeader.HEADER_PADDING_U16))
                .Concat(data)
                .ToArray();
        }
    }
}

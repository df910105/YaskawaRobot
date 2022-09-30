using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YHSES.Packet
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
            IEnumerable<byte> h = header.ToBytes();
            h.Append(service);
            h.Append(status);
            h.Append(added_status_size);
            h.Append(PacketHeader.HEADER_PADDING_U8);
            h.Concat(BitConverter.GetBytes(added_status));
            h.Concat(BitConverter.GetBytes(PacketHeader.HEADER_PADDING_U16));
            h.Concat(data);
            return h.ToArray();
        }
    }
}

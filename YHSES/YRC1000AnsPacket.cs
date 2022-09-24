using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YHSES
{
    class YRC1000AnsPacket
    {
        readonly YRC1000PacketHeader header;
        readonly byte service;
        readonly byte status;
        readonly byte added_status_size;
        readonly ushort added_status;
        readonly byte[] data;

        public YRC1000AnsPacket(byte[] _packet)
        {
            header = new YRC1000PacketHeader(_packet);
            service = _packet[24];
            status = _packet[25];
            added_status_size = _packet[26];
            added_status = BitConverter.ToUInt16(_packet, 28);
            data = _packet.Skip(YRC1000PacketHeader.HEADER_SIZE).Take(header.data_size).ToArray();
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
            h.Append(YRC1000PacketHeader.HEADER_PADDING_U8);
            h.Concat(BitConverter.GetBytes(added_status));
            h.Concat(BitConverter.GetBytes(YRC1000PacketHeader.HEADER_PADDING_U16));
            h.Concat(data);
            return h.ToArray();
        }
    }
}

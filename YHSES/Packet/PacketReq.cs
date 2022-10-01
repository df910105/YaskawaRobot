using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YHSES.Packet
{
    class PacketReq
    {
        readonly PacketHeader header;
        readonly ushort cmd_no;
        readonly ushort inst;
        readonly byte attr;
        readonly byte service;
        readonly byte[] data;

        public PacketReq(byte division, byte req_id, ushort cmd_no, ushort inst, byte attr, byte service, byte[] data, ushort data_size)
        {
            header = new PacketHeader(data_size, division,
                PacketHeader.HEADER_ACK_REQUEST, req_id, PacketHeader.HEADER_BLOCK_NUMBER_REQ);
            this.cmd_no = cmd_no;
            this.inst = inst;
            this.attr = attr;
            this.service = service;
            this.data = data;
        }

        public byte[] ToBytes()
        {
            IEnumerable<byte> h = header.ToBytes()
                .Concat(BitConverter.GetBytes(cmd_no))
                .Concat(BitConverter.GetBytes(inst))
                .Concat(new byte[] { attr, service })
                .Concat(BitConverter.GetBytes(PacketHeader.HEADER_PADDING_U16))
                .Concat(data);
            return h.ToArray();
        }

        public PacketReq Clone(byte[] data = null)
        {
            if (data == null)
            {
                return new PacketReq(header.division, header.req_id, cmd_no, inst, attr, service, this.data, (ushort)this.data.Length);
            }
            else
            {
                return new PacketReq(header.division, header.req_id, cmd_no, inst, attr, service, data, (ushort)data.Length);
            }
        }
    }
}

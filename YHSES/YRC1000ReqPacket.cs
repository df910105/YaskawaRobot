using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YHSES
{
    class YRC1000ReqPacket
    {
        readonly YRC1000PacketHeader header;
        readonly ushort cmd_no;
        readonly ushort inst;
        readonly byte attr;
        readonly byte service;
        readonly byte[] data;

        public YRC1000ReqPacket(byte division, byte req_id, ushort cmd_no, ushort inst, byte attr, byte service, byte[] data, ushort data_size)
        {
            header = new YRC1000PacketHeader(data_size, division,
                YRC1000PacketHeader.HEADER_ACK_REQUEST, req_id, YRC1000PacketHeader.HEADER_BLOCK_NUMBER_REQ);
            this.cmd_no = cmd_no;
            this.inst = inst;
            this.attr = attr;
            this.service = service;
            this.data = data;
        }

        public byte[] ToBytes()
        {
            IEnumerable<byte> h = header.ToBytes();
            h.Concat(BitConverter.GetBytes(cmd_no));
            h.Concat(BitConverter.GetBytes(inst));
            h.Append(attr);
            h.Append(service);
            h.Concat(BitConverter.GetBytes(YRC1000PacketHeader.HEADER_PADDING_U16));
            h.Concat(data);
            return h.ToArray();
        }

        public YRC1000ReqPacket Clone(byte[] data = null)
        {
            if (data == null)
            {
                return new YRC1000ReqPacket(header.division, header.req_id, cmd_no, inst, attr, service, this.data, (ushort)this.data.Length);
            }
            else
            {
                return new YRC1000ReqPacket(header.division, header.req_id, cmd_no, inst, attr, service, data, (ushort)data.Length);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

/**
* YASKAWA High Speed Ethernet Server Functions for YRC1000
*
* @date - $time$ 
*/

namespace YHSES
{
    public class YRC
    {
        readonly UdpClient client = new UdpClient();
        readonly IPEndPoint ipep;

        public YRC(string host, int port)
        {
            ipep = new IPEndPoint(IPAddress.Parse(host), port);
        }

        public int Switch()
        {
            string i = YRC1000PacketHeader.HEADER_IDENTIFIER;
            return 0;
        }
    }

    internal class YRC1000PacketHeader
    {
        public const string HEADER_IDENTIFIER = "YERC";
        public const ushort HEADER_SIZE = 0x20;
        public const byte HEADER_RESERVED_1 = 3;
        public const byte HEADER_DIVISION_ROBOT_CONTROL = 1;
        public const byte HEADER_DIVISION_FILE_CONTROL = 2;
        public const byte HEADER_ACK_REQUEST = 0;
        public const byte HEADER_ACK_NOT_REQUEST = 1;
        public const uint HEADER_BLOCK_NUMBER_REQ = 0;
        public const string HEADER_RESERVED_2 = "99999999";
        public const ushort HEADER_PADDING_USHORT = 0;
        public const byte HEADER_PADDING_BYTE = 0;

        private readonly ushort data_size;
        private readonly byte division;
        private readonly byte ack;
        private readonly byte req_id;
        private readonly uint block_no;

        public YRC1000PacketHeader(ushort data_size, byte division, byte ack, byte req_id, uint block_no, byte[] packet = null)
        {
            if (packet != null)
            {
                this.data_size = BitConverter.ToUInt16(packet, 6);
                this.division = packet[9];
                this.ack = packet[10];
                this.req_id = packet[11];
                this.block_no = BitConverter.ToUInt32(packet, 12);
            }
            else
            {
                this.data_size = data_size;
                this.division = division;
                this.ack = ack;
                this.req_id = req_id;
                this.block_no = block_no;
            }
        }

        public byte[] ToBytes()
        {
            IEnumerable<byte> h = Encoding.ASCII.GetBytes(HEADER_IDENTIFIER);
            h.Concat(BitConverter.GetBytes(HEADER_SIZE));
            h.Concat(BitConverter.GetBytes(data_size));
            h.Append(HEADER_RESERVED_1);
            h.Append(division);
            h.Append(ack);
            h.Append(req_id);
            h.Concat(BitConverter.GetBytes(block_no));
            h.Concat(Encoding.ASCII.GetBytes(HEADER_RESERVED_2));
            return h.ToArray();
        }
    }
}

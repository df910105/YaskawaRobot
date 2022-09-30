using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YHSES.Packet
{
    internal class PacketHeader
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
        public const ushort HEADER_PADDING_U16 = 0;
        public const byte HEADER_PADDING_U8 = 0;

        public readonly ushort data_size;
        public readonly byte division;
        public readonly byte ack;
        public readonly byte req_id;
        public readonly uint block_no;

        public PacketHeader(ushort _data_size, byte _division, byte _ack, byte _req_id, uint _block_no)
        {
            data_size = _data_size;
            division = _division;
            ack = _ack;
            req_id = _req_id;
            block_no = _block_no;
        }

        public PacketHeader(byte[] packet)
        {
            data_size = BitConverter.ToUInt16(packet, 6);
            division = packet[9];
            ack = packet[10];
            req_id = packet[11];
            block_no = BitConverter.ToUInt32(packet, 12);
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

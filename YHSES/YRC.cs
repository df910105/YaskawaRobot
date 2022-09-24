using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

/**
 * 
 * YASKAWA High Speed Ethernet Server Functions (C#)
 * 
 * This C# Library is implement from hsinkoyu/fs100 on GitHub (ref: https://github.com/hsinkoyu/fs100)
 *
 * Copyright (C) MIRDC 2022
 * 
 * Greatest Thanks To:
 *    Hsinko Yu <hsinkoyu@fih-foxconn.com>
 * 
 * Authors:
 *    Willy Wu <ycwu@mail.mirdc.org.tw>
 *    
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
        public const ushort HEADER_PADDING_U16 = 0;
        public const byte HEADER_PADDING_U8 = 0;

        public readonly ushort data_size;
        public readonly byte division;
        public readonly byte ack;
        public readonly byte req_id;
        public readonly uint block_no;

        public YRC1000PacketHeader(ushort _data_size, byte _division, byte _ack, byte _req_id, uint _block_no)
        {
            data_size = _data_size;
            division = _division;
            ack = _ack;
            req_id = _req_id;
            block_no = _block_no;
        }

        public YRC1000PacketHeader(byte[] packet)
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadStrData(ushort number, ref string data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x7D, number, 1, 0x0E,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data = utf_8.GetString(ans.data.Skip(0).Take(16).ToArray());
            }
            return ans.status;
        }

        public int WriteStrData(ushort number, string data, out ushort err_code)
        {
            var bytes = utf_8.GetBytes(data);
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x7D, number, 1, 0x10,
                bytes, (ushort)bytes.Length);
            var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }
}
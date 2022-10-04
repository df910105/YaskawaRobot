using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadIOData(ushort number, ref byte data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x78, number, 1, 0x0E,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data = ans.data[0];
            }
            return ans.status;
        }

        public int WriteIOData(ushort number, byte data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x78, number, 1, 0x10,
                new byte[1] { data }, 1);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }
    }
}

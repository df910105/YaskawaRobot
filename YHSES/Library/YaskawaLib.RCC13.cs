using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadDIntData(ushort number, ref int data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x7C, number, 1, 0x0E,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data = BitConverter.ToInt32(ans.data, 0);
            }
            return ans.status;
        }

        public int WriteDIntData(ushort number, int data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x7C, number, 1, 0x10,
                BitConverter.GetBytes(data), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }
    }
}

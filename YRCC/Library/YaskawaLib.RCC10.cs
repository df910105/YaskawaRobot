using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadRegData(ushort number, ref ushort data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x79, number, 1, 0x0E,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data = BitConverter.ToUInt16(ans.data, 0);
            }
            return ans.status;
        }

        public int WriteRegData(ushort number, ushort data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x79, number, 1, 0x10,
                BitConverter.GetBytes(data), 2);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }
    }
}

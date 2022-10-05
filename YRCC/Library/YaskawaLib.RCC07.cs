using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadPosErrorData(ushort robot_number, ref Axis data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x76, robot_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data.Axis_1 = BitConverter.ToInt32(ans.data, 0);
                data.Axis_2 = BitConverter.ToInt32(ans.data, 4);
                data.Axis_3 = BitConverter.ToInt32(ans.data, 8);
                data.Axis_4 = BitConverter.ToInt32(ans.data, 12);
                data.Axis_5 = BitConverter.ToInt32(ans.data, 16);
                data.Axis_6 = BitConverter.ToInt32(ans.data, 20);
                data.Axis_7 = BitConverter.ToInt32(ans.data, 24);
                data.Axis_8 = BitConverter.ToInt32(ans.data, 28);
            }
            return ans.status;
        }
    }
}

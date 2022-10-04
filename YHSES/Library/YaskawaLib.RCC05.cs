using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadAxisName(ushort robot_number, ref AxisName config, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x74, robot_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                config.Axis_1 = utf_8.GetString(ans.data.Skip(0).Take(4).ToArray());
                config.Axis_2 = utf_8.GetString(ans.data.Skip(4).Take(4).ToArray());
                config.Axis_3 = utf_8.GetString(ans.data.Skip(8).Take(4).ToArray());
                config.Axis_4 = utf_8.GetString(ans.data.Skip(12).Take(4).ToArray());
                config.Axis_5 = utf_8.GetString(ans.data.Skip(16).Take(4).ToArray());
                config.Axis_6 = utf_8.GetString(ans.data.Skip(20).Take(4).ToArray());
                config.Axis_7 = utf_8.GetString(ans.data.Skip(24).Take(4).ToArray());
                config.Axis_7 = utf_8.GetString(ans.data.Skip(28).Take(4).ToArray());
            }
            return ans.status;
        }
    }

    public class AxisName
    {
        public string Axis_1;
        public string Axis_2;
        public string Axis_3;
        public string Axis_4;
        public string Axis_5;
        public string Axis_6;
        public string Axis_7;
        public string Axis_8;
    }
}

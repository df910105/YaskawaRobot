using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YHSES.Packet;

namespace YHSES.Library
{
    partial class YaskawaLib
    {
        public int ReadTorqueData(ushort robot_number, ref AxisData data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x77, robot_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
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

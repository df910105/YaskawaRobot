using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YHSES.Packet;

namespace YHSES.Library
{
    partial class YaskawaLib
    {
        public int AlarmReset(out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x82, (int)RESET_TYPE.Alarm, 0x01, 0x10,
                BitConverter.GetBytes(1), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }

        public int ErrorCancel(out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x82, (int)RESET_TYPE.Error, 0x01, 0x10,
                BitConverter.GetBytes(1), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }
    }

    enum RESET_TYPE : int
    {
        Alarm = 1,
        Error = 2,
    }
}

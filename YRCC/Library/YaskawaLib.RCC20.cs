using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ServoSwitch(POWER_SWITCH on_off, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x83, (int)POWER_TYPE.SERVO, 0x01, 0x10,
                BitConverter.GetBytes((int)on_off), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }

        public int HLockSwitch(POWER_SWITCH on_off, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x83, (int)POWER_TYPE.HLOCK, 0x01, 0x10,
                BitConverter.GetBytes((int)on_off), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }

        public int HoldSwitch(POWER_SWITCH on_off, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x83, (int)POWER_TYPE.HOLD, 0x01, 0x10,
                BitConverter.GetBytes((int)on_off), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }

        enum POWER_TYPE : int
        {
            HOLD = 1,
            SERVO = 2,
            HLOCK = 3,
        }
    }

    public enum POWER_SWITCH : int
    {
        ON = 1,
        OFF = 2,
    }
}

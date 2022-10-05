using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ServoSwitch(SWITCH on_off, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x83, (int)POWER_TYPE.SERVO, 0x01, 0x10,
                    BitConverter.GetBytes((int)on_off), 4);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int HLockSwitch(SWITCH on_off, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x83, (int)POWER_TYPE.HLOCK, 0x01, 0x10,
                    BitConverter.GetBytes((int)on_off), 4);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int HoldSwitch(SWITCH on_off, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x83, (int)POWER_TYPE.HOLD, 0x01, 0x10,
                    BitConverter.GetBytes((int)on_off), 4);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }

        enum POWER_TYPE : int
        {
            HOLD = 1,
            SERVO = 2,
            HLOCK = 3,
        }
    }

    public enum SWITCH : int
    {
        ON = 1,
        OFF = 2,
    }
}

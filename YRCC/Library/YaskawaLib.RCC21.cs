using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int SwtichCycleType(CYCLE_TYPE type, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x84, 2, 0x01, 0x10,
                BitConverter.GetBytes((uint)type), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }
    }

    public enum CYCLE_TYPE : uint
    {
        Step = 1,
        Cycle = 2,
        AUTO = 3,
    }
}

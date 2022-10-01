using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YHSES.Packet;

namespace YHSES.Library
{
    partial class YaskawaLib
    {
        public int StartJob(out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x86, 1, 0x01, 0x10,
                BitConverter.GetBytes(1), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }
    }
}

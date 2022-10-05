using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int DisplayMessage(string message, out ushort err_code)
        {
            var bytes = utf_8.GetBytes(message);
            if (bytes.Length > 30)
            {
                bytes = bytes.Take(30).ToArray();
            }
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x85, 1, 0x01, 0x10,
                bytes, (ushort)bytes.Length);
            var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            return ans.status;
        }
    }
}

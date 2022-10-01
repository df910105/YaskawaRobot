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
        public int SelectJob(string job_name, uint line, out ushort err_code)
        {
            var bytes = utf_8.GetBytes(job_name);
            if (bytes.Length > 32)
            {
                bytes = bytes
                    .Take(32)
                    .Concat(BitConverter.GetBytes(line))
                    .ToArray();
            }
            else
            {
                bytes = bytes
                    .Concat(new byte[32 - bytes.Length])
                    .Concat(BitConverter.GetBytes(line))
                    .ToArray();
            }
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x87, 1, 0x00, 0x02,
                bytes, (ushort)bytes.Length);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }
    }
}

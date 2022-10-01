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
        public int ReadIOData(ushort job_number, ref byte data, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x78, job_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                job.JobName = utf_8.GetString(ans.data.Skip(0).Take(32).ToArray());
            }
            return ans.status;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int SelectJob(string job_name, uint line, out ushort err_code)
        {
            try
            {
                var bytes = utf_8.GetBytes(job_name);
                if (bytes.Length <= 32)
                {
                    bytes = bytes
                        .Concat(new byte[32 - bytes.Length])
                        .Concat(BitConverter.GetBytes(line))
                        .ToArray();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("NameLength", "Job name is over 32 bytes.");
                }
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x87, 1, 0x00, 0x02,
                    bytes, (ushort)bytes.Length);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

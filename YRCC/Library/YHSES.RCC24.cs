using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/31 by Willy

        /// <summary>
        /// [RCC24] 程式選擇 (0x87). 長度限制32位元組(byte)
        /// </summary>
        /// <param name="job_name"></param>
        /// <param name="line"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int SelectJob(string job_name, uint line, out ushort err_code)
        {
            try
            {
                var bytes = utf_8.GetBytes(job_name);
                if (bytes.Length <= 32)
                {
                    bytes = bytes.Concat(new byte[32 - bytes.Length]).ToArray();
                }
                bytes = bytes.Concat(BitConverter.GetBytes(line)).ToArray();
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

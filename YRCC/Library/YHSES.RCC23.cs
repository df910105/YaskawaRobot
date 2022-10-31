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
        /// [RCC23] 程式選擇 (0x86)
        /// </summary>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int StartJob(out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x86, 1, 0x01, 0x10,
                    BitConverter.GetBytes(1), 4);
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

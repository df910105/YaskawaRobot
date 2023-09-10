using System;
using YRCC.Packet;

namespace YRCC
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/27 by Willy

        /// <summary>
        /// [RCC22] 顯示訊息 (0x85). 長度限制30位元組(byte)。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int DisplayMessage(string message, out ushort err_code)
        {
            try
            {
                var bytes = big5.GetBytes(message);
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x85, 1, 0x01, 0x10,
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

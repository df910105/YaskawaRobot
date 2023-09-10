using System;
using YRCC.Packet;

namespace YRCC
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/27 by Willy

        /// <summary>
        /// [RCC02] 讀取歷史異常資訊 (0x71)
        /// </summary>
        /// <param name="alarm_number">Range: 請參考手冊</param>
        /// <param name="alarm"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadAlarmHistory(ushort alarm_number, ref AlarmData alarm, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x71, alarm_number, 0, 0x01,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    AlarmDataDecode(alarm, ans.data);
                }
                return ans.status;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

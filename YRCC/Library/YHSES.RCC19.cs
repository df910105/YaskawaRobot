using System;
using YRCC.Packet;

namespace YRCC
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/27 by Willy

        /// <summary>
        /// [RCC19] 重置警報 (0x82)
        /// </summary>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int AlarmReset(out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x82, (int)RESET_TYPE.Alarm, 0x01, 0x10,
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

        /// <summary>
        /// [RCC19] 取消錯誤 (0x82)
        /// </summary>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ErrorCancel(out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x82, (int)RESET_TYPE.Error, 0x01, 0x10,
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

    enum RESET_TYPE : int
    {
        Alarm = 1,
        Error = 2,
    }
}

using System;
using YRCC.Packet;

namespace YRCC
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/26 by Willy

        /// <summary>
        /// [RCC20] 伺服切換開關 (0x83)
        /// </summary>
        /// <param name="on_off"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ServoSwitch(SWITCH on_off, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x83, (int)POWER_TYPE.SERVO, 0x01, 0x10,
                    BitConverter.GetBytes((int)on_off), 4);
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
        /// [RCC20] 螢幕鎖定切換開關 (0x83)
        /// </summary>
        /// <param name="on_off"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int HLockSwitch(SWITCH on_off, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x83, (int)POWER_TYPE.HLOCK, 0x01, 0x10,
                    BitConverter.GetBytes((int)on_off), 4);
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
        /// [RCC20] 手臂暫停切換開關 (0x83)
        /// </summary>
        /// <param name="on_off"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int HoldSwitch(SWITCH on_off, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x83, (int)POWER_TYPE.HOLD, 0x01, 0x10,
                    BitConverter.GetBytes((int)on_off), 4);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }

        enum POWER_TYPE : int
        {
            HOLD = 1,
            SERVO = 2,
            HLOCK = 3,
        }
    }

    /// <summary>
    /// 開關切換
    /// </summary>
    public enum SWITCH : int
    {
        /// <summary>
        /// 啟用
        /// </summary>
        ON = 1,

        /// <summary>
        /// 關閉
        /// </summary>
        OFF = 2,
    }
}

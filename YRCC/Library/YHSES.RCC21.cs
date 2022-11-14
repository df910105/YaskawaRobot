using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/26 by Willy

        /// <summary>
        /// [RCC21] 循環動作指令 (0x84)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int SwitchCycleType(CYCLE_TYPE type, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x84, 2, 0x01, 0x10,
                    BitConverter.GetBytes((uint)type), 4);
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

    /// <summary>
    /// 模式
    /// </summary>
    public enum CYCLE_TYPE : uint
    {
        /// <summary>
        /// 單步
        /// </summary>
        Step = 1,

        /// <summary>
        /// 單次循環
        /// </summary>
        Cycle = 2,

        /// <summary>
        /// 連續循環
        /// </summary>
        AUTO = 3,
    }
}

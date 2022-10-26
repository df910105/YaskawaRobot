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
        /// [RCC13] 讀取倍精度型(int)資料 (0x7C)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadDIntData(ushort number, ref int data, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x7C, number, 1, 0x0E,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    data = BitConverter.ToInt32(ans.data, 0);
                }
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// [RCC13] 寫入倍精度型(int)資料 (0x7C)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="data"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int WriteDIntData(ushort number, int data, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x7C, number, 1, 0x10,
                    BitConverter.GetBytes(data), 4);
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

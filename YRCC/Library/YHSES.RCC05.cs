using System;
using System.Linq;
using YRCC.Packet;

namespace YRCC
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/11/14 by Willy

        /// <summary>
        /// [RCC05] 讀取各軸名稱 (0x74)
        /// </summary>
        /// <param name="robot_number"></param>
        /// <param name="config"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadAxisName(ushort robot_number, ref AxisName config, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x74, robot_number, 0, 0x01,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    config.Axis_1 = utf_8.GetString(ans.data.Skip(0).Take(4).ToArray());
                    config.Axis_2 = utf_8.GetString(ans.data.Skip(4).Take(4).ToArray());
                    config.Axis_3 = utf_8.GetString(ans.data.Skip(8).Take(4).ToArray());
                    config.Axis_4 = utf_8.GetString(ans.data.Skip(12).Take(4).ToArray());
                    config.Axis_5 = utf_8.GetString(ans.data.Skip(16).Take(4).ToArray());
                    config.Axis_6 = utf_8.GetString(ans.data.Skip(20).Take(4).ToArray());
                    config.Axis_7 = utf_8.GetString(ans.data.Skip(24).Take(4).ToArray());
                    config.Axis_7 = utf_8.GetString(ans.data.Skip(28).Take(4).ToArray());
                }
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    /// <summary>
    /// 軸名稱
    /// </summary>
    public class AxisName
    {
        /// <summary>
        /// 
        /// </summary>
        public string Axis_1;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_2;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_3;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_4;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_5;
        
        /// <summary>
        /// 
        /// </summary>
        public string Axis_6;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_7;

        /// <summary>
        /// 
        /// </summary>
        public string Axis_8;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/31 by Willy

        /// <summary>
        /// [RCC25] 讀取系統管理時間 (0x88)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="time"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadTimeData(ushort number, ref Time time, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x88, number, 0, 0x01,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    string dateString = ascii.GetString(ans.data.Skip(0).Take(16).ToArray());
                    time.DateStart = DateTime.ParseExact(dateString, DATE_PATTERN, null);

                    //PATTERN：%h:mm'ss
                    string timeString = ascii.GetString(ans.data.Skip(16).Take(12).ToArray()).TrimEnd('\0');
                    string[] temp0 = timeString.Split(':'); 
                    string[] temp1 = temp0.Last().Split('\'');
                    int hh = int.Parse(temp0.First());
                    int mm = int.Parse(temp1.First());
                    int ss = int.Parse(temp1.Last());
                    time.TimeElapsed = new TimeSpan(hh, mm, ss);
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
    /// 系統時間資訊
    /// </summary>
    public class Time
    {
        /// <summary>
        /// 始記時間
        /// </summary>
        public DateTime DateStart = new DateTime();

        /// <summary>
        /// 運轉時間
        /// </summary>
        public TimeSpan TimeElapsed = new TimeSpan();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Operation start time: {DateStart:g},\r\n" +
                $"Elapse time: {TimeElapsed}\r\n";
        }
    }
}

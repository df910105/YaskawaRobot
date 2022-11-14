using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        /// 本頁功能確認於 2022/10/27 by Willy

        /// <summary>
        /// [RCC01] 讀取目前異常資訊 (0x70)
        /// </summary>
        /// <param name="last_number">Range: 1st - 4th.</param>
        /// <param name="alarm"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadAlarmData(ushort last_number, ref AlarmData alarm, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x70, last_number, 0, 0x01,
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

        private void AlarmDataDecode(AlarmData alarm, byte[] packetData)
        {
            alarm.Code = BitConverter.ToUInt32(packetData, 0);
            alarm.Data = BitConverter.ToUInt32(packetData, 4);
            alarm.Type = BitConverter.ToUInt32(packetData, 8);
            string timeString = ascii.GetString(packetData.Skip(12).Take(16).ToArray());
            if (DateTime.TryParseExact(timeString, DATE_PATTERN, null,
                System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                alarm.Time = dateTime;
            }
            alarm.Name = big5.GetString(packetData.Skip(28).Take(32).ToArray()).TrimEnd('\0');
        }
    }

    /// <summary>
    /// 異常資訊
    /// </summary>
    public class AlarmData
    {
        /// <summary>
        /// 發生時間
        /// </summary>
        public DateTime Time = new DateTime();

        /// <summary>
        /// 描述
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// 代號
        /// </summary>
        public uint Code = 0;

        /// <summary>
        /// 次代號(sub code)
        /// </summary>
        public uint Data = 0;

        /// <summary>
        /// 異常分類
        /// </summary>
        public uint Type = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Time: {Time:g}\r\n" +
                $"Name: {Name}\r\n" +
                $"Code: {Code}, " +
                $"Data: {Data}, " +
                $"Type: {Type}";
        }
    }

    /*public enum AlarmType : uint
    {
        //待補, 或改成Dictionary結構
        NoAlarm = 0,

        SLURBT = 3,
    }*/
}

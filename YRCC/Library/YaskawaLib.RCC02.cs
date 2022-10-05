using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadAlarmHistory(ushort alarm_number, ref AlarmData alarm, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x71, alarm_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                alarm.Code = BitConverter.ToUInt32(ans.data, 0);
                alarm.Data = BitConverter.ToUInt32(ans.data, 4);
                alarm.Type = BitConverter.ToUInt32(ans.data, 8);
                string timeString = ascii.GetString(ans.data.Skip(12).Take(16).ToArray());
                alarm.Time = DateTime.ParseExact(timeString, "yyyy/MM/dd HH:mm", null);
                string nameString = utf_8.GetString(ans.data.Skip(28).Take(32).ToArray());
                alarm.Name = nameString;
            }
            return ans.status;
        }
    }
}

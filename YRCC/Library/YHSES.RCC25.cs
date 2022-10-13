﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int TimeDataR(ushort number, ref Time time, out ushort err_code)
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
                    string timeString = ascii.GetString(ans.data.Skip(16).Take(12).ToArray());
                    time.TimeElapsed = TimeSpan.ParseExact(dateString, TIME_PATTERN, null);
                }
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class Time
    {
        public DateTime DateStart = new DateTime();
        public TimeSpan TimeElapsed = new TimeSpan();

        public override string ToString()
        {
            return $"Operation start time: {DateStart:g},\r\n" +
                $"Elapse time: {TimeElapsed}\r\n";
        }
    }
}
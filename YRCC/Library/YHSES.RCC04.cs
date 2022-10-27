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
        /// [RCC04] 讀取目前執行程式 (0x73)
        /// </summary>
        /// <param name="job_number"></param>
        /// <param name="job"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int ReadExecutingJob(ushort job_number, ref JobInfo job, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x73, job_number, 0, 0x01,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    job.JobName = utf_8.GetString(ans.data.Skip(0).Take(32).ToArray());
                    job.Line = BitConverter.ToUInt32(ans.data, 32);
                    job.Step = BitConverter.ToUInt32(ans.data, 36);
                    job.SpeedOverride = BitConverter.ToUInt32(ans.data, 40);
                }
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class JobInfo
    {
        public string JobName = string.Empty;
        public uint Line = 0;
        public uint Step = 0;
        public uint SpeedOverride = 0;

        public override string ToString()
        {
            return $"JobName: {JobName}\r\n" +
                $"Line: {Line}, " +
                $"Step: {Step}, " +
                $"SpeedOverride: {SpeedOverride}";
        }
    }
}

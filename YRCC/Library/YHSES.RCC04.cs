using System;
using System.Linq;
using YRCC.Packet;

namespace YRCC
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

    /// <summary>
    /// 程式資訊
    /// </summary>
    public class JobInfo
    {
        /// <summary>
        /// Job name (32 letters)
        /// </summary>
        public string JobName = string.Empty;

        /// <summary>
        /// Line number (0 to 9999)
        /// </summary>
        public uint Line = 0;

        /// <summary>
        /// Step number (0 to 9998)
        /// </summary>
        public uint Step = 0;

        /// <summary>
        /// Speed override value.
        /// </summary>
        public uint SpeedOverride = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"JobName: {JobName}\r\n" +
                $"Line: {Line}, " +
                $"Step: {Step}, " +
                $"SpeedOverride: {SpeedOverride}";
        }
    }
}

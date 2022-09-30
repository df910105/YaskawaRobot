using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using YHSES.Packet;

/**
 * 
 * YASKAWA High Speed Ethernet Server Functions (C#)
 * 
 * This C# Library is implement from hsinkoyu/fs100 on GitHub (ref: https://github.com/hsinkoyu/fs100)
 *
 * Copyright (C) MIRDC 2022
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 * See theGNU General Public License for more details.
 * 
 * Greatest Thanks To:
 *    Hsinko Yu <hsinkoyu@fih-foxconn.com>
 * 
 * Authors:
 *    Willy Wu <ycwu@mail.mirdc.org.tw>
 *    
 */

namespace YHSES.Library
{
    public partial class YaskawaLib
    {
        #region -- Field --

        UdpClient client = new UdpClient();
        IPEndPoint endPoint;

        readonly Encoding ascii = Encoding.ASCII;
        readonly Encoding utf_8 = Encoding.UTF8;
        #endregion

        #region -- Constant --

        const int UDP_PORT_ROBOT_CONTROL = 10040;
        const int UDP_PORT_FILE_CONTROL = 10041;

        const int TRANSMISSION_SEND = 1;
        const int TRANSMISSION_SEND_AND_RECV = 2;

        const int ERROR_SUCCESS = 0;
        const int ERROR_CONNECTION = 1;
        const int ERROR_NO_SUCH_FILE_OR_DIRECTORY = 2;

        const double TRAVEL_STATUS_POLLING_DURATION = 0.1;
        const int TRAVEL_STATUS_START = 0;
        const uint TRAVEL_STATUS_END = 0xffffffff;
        const int TRAVEL_STATUS_ERROR = -1; //errno for details

        // cycle selection command
        const int CYCLE_TYPE_STEP = 1;
        const int CYCLE_TYPE_ONE_CYCLE = 2;
        const int CYCLE_TYPE_CONTINUOUS = 3;

        // move command
        const int MOVE_TYPE_JOINT_ABSOLUTE_POS = 1;
        const int MOVE_TYPE_LINEAR_ABSOLUTE_POS = 2;
        const int MOVE_TYPE_LINEAR_INCREMENTAL_POS = 3;
        const int MOVE_SPEED_CLASS_PERCENT = 0;  // for joint operation
        const int MOVE_SPEED_CLASS_MILLIMETER = 1;
        const int MOVE_SPEED_CLASS_DEGREE = 2;
        const int MOVE_COORDINATE_SYSTEM_BASE = 16;
        const int MOVE_COORDINATE_SYSTEM_ROBOT = 17;
        const int MOVE_COORDINATE_SYSTEM_USER = 18;
        const int MOVE_COORDINATE_SYSTEM_TOOL = 19;

        // reset alarm command
        const int RESET_ALARM_TYPE_ALARM = 1;
        const int RESET_ALARM_TYPE_ERROR = 2;

        #endregion

        #region -- Property --

        public string IP { get; private set; }

        public int TimeOut { get; private set; }

        public int ErrNo { get; private set; }

        #endregion

        public YaskawaLib(string ip, int timeout = 800)
        {
            IP = ip;
            TimeOut = timeout;
            client = null;
            ErrNo = 0;
        }

        private void Connect(int port = UDP_PORT_ROBOT_CONTROL)
        {
            endPoint = new IPEndPoint(IPAddress.Parse(IP), port);
            if (client == null)
            {
                client = new UdpClient(endPoint);
                client.Client.ReceiveTimeout = TimeOut;
            }
        }

        private void Disconnect()
        {
            if (client != null)
            {
                client.Close();
                client.Dispose();
            }
        }

        private byte[] GenerateErrorAnsPacket(byte result, ushort errno)
        {
            //when error, result and error number are what callers care about
            IEnumerable<byte> p = new byte[25];
            p.Append(result);
            p.Concat(new byte[2]);
            p.Concat(BitConverter.GetBytes(errno));
            p.Concat(new byte[2]);
            return p.ToArray();
        }

        private PacketAns Transmit(byte[] packet, int direction = TRANSMISSION_SEND_AND_RECV)
        {
            PacketAns ans = null;
            byte[] ans_packet = null;
            lock (client)
            {

                bool to_disc;
                if (client.Client.Connected)
                {
                    to_disc = false;
                }
                else
                {
                    Connect();
                    to_disc = true;
                }
                try
                {
                    client.Send(packet, packet.Length);
                    if (direction == TRANSMISSION_SEND_AND_RECV)
                    {
                        ans_packet = client.Receive(ref endPoint);
                    }
                }
                catch (SocketException ex)
                {
                    ans_packet = GenerateErrorAnsPacket(ERROR_CONNECTION, (ushort)ex.ErrorCode);
                }

                if (direction == TRANSMISSION_SEND_AND_RECV)
                {
                    ans = new PacketAns(ans_packet);
                }

                if (to_disc)
                {
                    Disconnect();
                }
            }
            return ans;
        }

        #region -- Switch Command --

        public int ServoSwitch(POWER_SWITCH on_off, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x83, (int)POWER_TYPE.SERVO, 0x01, 0x10, 
                BitConverter.GetBytes((int)on_off), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }

        public int HLockSwitch(POWER_SWITCH on_off, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x83, (int)POWER_TYPE.HLOCK, 0x01, 0x10, 
                BitConverter.GetBytes((int)on_off), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }

        public int HoldSwitch(POWER_SWITCH on_off, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x83, (int)POWER_TYPE.HOLD, 0x01, 0x10, 
                BitConverter.GetBytes((int)on_off), 4);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            return ans.status;
        }

        #endregion        

        #region -- Status Information --

        public int ReadStatusInfo(ref uint data_1, ref uint data_2, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x72, 1, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                data_1 = BitConverter.ToUInt32(ans.data, 0);
                data_2 = BitConverter.ToUInt32(ans.data, 4);
            }
            return ans.status;
        }

        public int ReadStatusInfo(ref StatusInfo status, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x72, 1, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                status.Data1 = BitConverter.ToUInt32(ans.data, 0);
                status.Data2 = BitConverter.ToUInt32(ans.data, 4);                
            }
            return ans.status;
        }

        #endregion

        #region -- Read Executing Job Infomation --

        public int ReadExecutingJob(ushort job_number, ref JobInfo job, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x73, job_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
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

        #endregion
    }
}

public enum POWER_SWITCH : int
{
    ON = 1,
    OFF = 2,
}

enum POWER_TYPE : int
{
    HOLD = 1,
    SERVO = 2,
    HLOCK = 3,
}

public class AlarmData
{
    public DateTime Time;
    public string Name;
    public uint Code;
    public uint Data;
    public uint Type;
}

public class StatusInfo
{
    public uint Data1 = 0;
    public uint Data2 = 0;

    #region Data 1

    public bool Step => (Data1 & 0x01) > 0;
    public bool OneCycle => (Data1 & 0x02) > 0;
    public bool AutoAndCont => (Data1 & 0x04) > 0;
    public bool Running => (Data1 & 0x08) > 0;
    public bool InGuardSafe => (Data1 & 0x10) > 0;
    public bool Teach => (Data1 & 0x20) > 0;
    public bool Play => (Data1 & 0x40) > 0;
    public bool CmdRemote => (Data1 & 0x80) > 0;

    #endregion

    #region Data 2

    public bool InHold_Pendant => (Data2 & 0x02) > 0;
    public bool InHold_Ext => (Data2 & 0x04) > 0;
    public bool InHold_Cmd => (Data2 & 0x08) > 0;
    public bool Alarming => (Data2 & 0x10) > 0;
    public bool ErrOccurring => (Data2 & 0x20) > 0;
    public bool ServoON => (Data2 & 0x40) > 0;
    #endregion
}

public class JobInfo
{
    public string JobName = string.Empty;
    public uint Line = 0;
    public uint Step = 0;
    public uint SpeedOverride = 0;
}

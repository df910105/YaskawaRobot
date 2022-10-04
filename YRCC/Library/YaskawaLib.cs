using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using YRCC.Packet;

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
 * See the GNU General Public License for more details.
 * 
 * Greatest Thanks To:
 *    Hsinko Yu <hsinkoyu@fih-foxconn.com>
 * 
 * Authors:
 *    Willy Wu <ycwu@mail.mirdc.org.tw>
 *    
 */

namespace YRCC.Library
{
    public sealed partial class YHSES
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

        #endregion

        #region -- Property --

        public string IP { get; private set; }

        public int TimeOut { get; private set; }

        public int ErrNo { get; private set; }

        #endregion

        public YHSES(string ip, int timeout = 800)
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
            return new byte[25]
                .Concat(new byte[1] { result })
                .Concat(new byte[2])
                .Concat(BitConverter.GetBytes(errno))
                .Concat(new byte[2])
                .ToArray();
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
    }
}

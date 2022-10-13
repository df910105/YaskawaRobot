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

        Socket socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        EndPoint endPoint;

        readonly Encoding ascii = Encoding.ASCII;
        readonly Encoding utf_8 = Encoding.UTF8;
        #endregion

        #region -- Constant --

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

        /// <summary>
        /// Ex:2011/10/10 15:49
        /// </summary>
        const string DATE_PATTERN = @"yyyy/MM/dd HH\:mm";

        /// <summary>
        /// Ex:000000:00'00
        /// </summary>
        const string TIME_PATTERN = @"%h:mm'ss";

        #endregion

        #region -- Property --

        public string IP { get; private set; }

        public int TimeOut { get; private set; }

        public int PORT_ROBOT_CONTROL { get; set; } = 10040;

        public int PORT_FILE_CONTROL { get; set; } = 10041;

        #endregion

        public YHSES(string ip, int timeout = 800)
        {
            try
            {
                IP = ip;
                TimeOut = timeout;
                socket.ReceiveTimeout = TimeOut;
                socket.SendTimeout = TimeOut;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Connect(int port)
        {
            try
            {
                if (socket == null)
                {
                    socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
                }
                endPoint = new IPEndPoint(IPAddress.Parse(IP), port);
                socket.Bind(endPoint);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Disconnect()
        {
            try
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
            catch (Exception)
            {
                throw;
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

        private PacketAns Transmit(byte[] packet, int port, int direction = TRANSMISSION_SEND_AND_RECV)
        {
            PacketAns ans = null;
            byte[] ans_packet = new byte[512];
            lock (socket)
            {
                bool to_disc = !socket.Connected;
                try
                {
                    if (!socket.Connected)
                    {
                        Connect(port);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                try
                {
                    socket.Send(packet);
                    if (direction == TRANSMISSION_SEND_AND_RECV)
                    {
                        int count = socket.Receive(ans_packet);
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

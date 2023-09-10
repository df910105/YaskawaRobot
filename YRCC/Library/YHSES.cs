using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using YRCC.Packet;

/***
 * 
 * YASKAWA High Speed Ethernet Server Functions (C#)
 * 
 * This C# Library is implement from YASKAWA High Speed Ethernet Server, 
 * which is a UDP base robot control protocal provide by YASKAWA.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 * See the GNU General Public License for more details.
 * 
 * Authors:
 *    Willy Wu <df910105@gmail.com>
 * 
 * Greatest Thanks To:
 *    Hsinko Yu <hsinkoyu@fih-foxconn.com>
 *    Ref: https://github.com/hsinkoyu/fs100
 *    
 ***/

namespace YRCC
{
    /// <summary>
    /// YASKAWA High Speed Ethernet Server Functions (C#)
    /// </summary>
    public sealed partial class YHSES
    {
        #region -- Field --

        Socket socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        EndPoint endPoint;

        private readonly object SocketLock = new object();
        readonly Encoding ascii = Encoding.ASCII;
        readonly Encoding utf_8 = Encoding.UTF8;
        readonly Encoding big5 = Encoding.GetEncoding("big5");
        #endregion

        #region -- Constant --

        const int TRANSMISSION_SEND = 1;
        const int TRANSMISSION_SEND_AND_RECV = 2;

        const int ERROR_SUCCESS = 0;
        const int ERROR_CONNECTION = 1;

        /// <summary>
        /// Ex:2011/10/10 15:49
        /// </summary>
        const string DATE_PATTERN = @"yyyy/MM/dd HH\:mm";

        #endregion

        #region -- Property --

        /// <summary>
        /// Robot IP
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        /// 逾時設定(ms)
        /// </summary>
        public int TimeOut { get; private set; }

        /// <summary>
        /// 手臂控制埠，預設10040
        /// </summary>
        public int PORT_ROBOT_CONTROL { get; set; } = 10040;

        /// <summary>
        /// 檔案傳輸埠，預設10041
        /// </summary>
        public int PORT_FILE_CONTROL { get; set; } = 10041;

        /// <summary>
        /// 連線是否正常? (測試中)
        /// </summary>
        public bool IsConnectOK { get; private set; } = false;

        #endregion

        /// <summary>
        /// YASKAWA High Speed Ethernet Server
        /// </summary>
        /// <param name="ip">IP位址 ex."192.168.255.1"</param>
        /// <param name="timeout">連線逾時</param>
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
                if (!socket.Connected)
                {
                    socket.Dispose();
                    socket = new Socket(SocketType.Dgram, ProtocolType.Udp)
                    {
                        ReceiveTimeout = TimeOut,
                        SendTimeout = TimeOut
                    };
                }
                endPoint = new IPEndPoint(IPAddress.Parse(IP), port);
                socket.Connect(endPoint);
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
                    socket.Dispose();
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
            lock (SocketLock)
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
                        IsConnectOK = true;
                    }
                }
                catch (SocketException ex)
                {
                    IsConnectOK = false;
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

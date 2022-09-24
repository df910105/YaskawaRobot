using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

/**
 * 
 * YASKAWA High Speed Ethernet Server Functions (C#)
 * 
 * This C# Library is implement from hsinkoyu/fs100 on GitHub (ref: https://github.com/hsinkoyu/fs100)
 *
 * Copyright (C) MIRDC 2022
 * 
 * Greatest Thanks To:
 *    Hsinko Yu <hsinkoyu@fih-foxconn.com>
 * 
 * Authors:
 *    Willy Wu <ycwu@mail.mirdc.org.tw>
 *    
 */

namespace YHSES
{
    public class YRC1000
    {
        readonly UdpClient client = new UdpClient();
        readonly IPEndPoint ipep;

        public YRC1000(string host, int port)
        {
            ipep = new IPEndPoint(IPAddress.Parse(host), port);
        }

        public int Switch()
        {
            string i = YRC1000PacketHeader.HEADER_IDENTIFIER;
            return 0;
        }
    }
}

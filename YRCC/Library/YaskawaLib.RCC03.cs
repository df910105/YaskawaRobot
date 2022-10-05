using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadStatusInfo(ref uint data_1, ref uint data_2, out ushort err_code)
        {
            try
            {

                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x72, 1, 0, 0x01,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    data_1 = BitConverter.ToUInt32(ans.data, 0);
                    data_2 = BitConverter.ToUInt32(ans.data, 4);
                }
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int ReadStatusInfo(ref StatusInfo status, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x72, 1, 0, 0x01,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    status.Data1 = BitConverter.ToUInt32(ans.data, 0);
                    status.Data2 = BitConverter.ToUInt32(ans.data, 4);
                }
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }
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

}

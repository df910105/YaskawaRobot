using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YHSES.Packet;

namespace YHSES.Library
{
    partial class YaskawaLib
    {
        public int ReadPositionData(ushort robot_number, ref PosData config, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x75, robot_number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes());
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                config.DataType = BitConverter.ToUInt32(ans.data, 0);
                config.Type = BitConverter.ToUInt32(ans.data, 4);
                config.ToolNumber = BitConverter.ToUInt32(ans.data, 8);
                config.UserCoordNumber = BitConverter.ToUInt32(ans.data, 12);
                config.ExtendedType = BitConverter.ToUInt32(ans.data, 16);
                config.AxisData.Axis_1 = BitConverter.ToInt32(ans.data, 20);
                config.AxisData.Axis_2 = BitConverter.ToInt32(ans.data, 24);
                config.AxisData.Axis_3 = BitConverter.ToInt32(ans.data, 28);
                config.AxisData.Axis_4 = BitConverter.ToInt32(ans.data, 32);
                config.AxisData.Axis_5 = BitConverter.ToInt32(ans.data, 36);
                config.AxisData.Axis_6 = BitConverter.ToInt32(ans.data, 40);
                config.AxisData.Axis_7 = BitConverter.ToInt32(ans.data, 44);
                config.AxisData.Axis_8 = BitConverter.ToInt32(ans.data, 48);
            }
            return ans.status;
        }
    }

    public class PosData
    {
        public uint DataType;
        public uint Type;
        public uint ToolNumber;
        public uint UserCoordNumber;
        public uint ExtendedType;
        public AxisData AxisData;
    }

    public class AxisData
    {
        public int Axis_1;
        public int Axis_2;
        public int Axis_3;
        public int Axis_4;
        public int Axis_5;
        public int Axis_6;
        public int Axis_7;
        public int Axis_8;
    }
}

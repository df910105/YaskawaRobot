using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int ReadPositionData(ushort number, ref Posistion config, out ushort err_code)
        {
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x7F, number, 0, 0x01,
                new byte[0], 0);
            var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                config.DataType = BitConverter.ToUInt32(ans.data, 0);
                config.Figure = BitConverter.ToUInt32(ans.data, 4);
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

        public int WritePositionData(ushort number, Posistion config, out ushort err_code)
        {
            var bytes = ParsePositionDataBytes(config);
            var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                0x7F, number, 0, 0x02,
                bytes, (ushort)bytes.Length);
            var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
            err_code = ans.added_status;
            if (ans.status == ERROR_SUCCESS)
            {
                config.DataType = BitConverter.ToUInt32(ans.data, 0);
                config.Figure = BitConverter.ToUInt32(ans.data, 4);
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

        private byte[] ParsePositionDataBytes(Posistion config)
        {
            return BitConverter.GetBytes(config.DataType)
                .Concat(BitConverter.GetBytes(config.Figure))
                .Concat(BitConverter.GetBytes(config.ToolNumber))
                .Concat(BitConverter.GetBytes(config.UserCoordNumber))
                .Concat(BitConverter.GetBytes(config.ExtendedType))
                .Concat(BitConverter.GetBytes(config.AxisData.Axis_1))
                .Concat(BitConverter.GetBytes(config.AxisData.Axis_2))
                .Concat(BitConverter.GetBytes(config.AxisData.Axis_3))
                .Concat(BitConverter.GetBytes(config.AxisData.Axis_4))
                .Concat(BitConverter.GetBytes(config.AxisData.Axis_5))
                .Concat(BitConverter.GetBytes(config.AxisData.Axis_6))
                .Concat(BitConverter.GetBytes(config.AxisData.Axis_7))
                .Concat(BitConverter.GetBytes(config.AxisData.Axis_8))
                .ToArray();
        }
    }
}

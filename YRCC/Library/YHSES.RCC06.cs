using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        /// <summary>
        /// [RCC06] 讀取手臂位置 (0x75)
        /// </summary>
        /// <param name="robot_number"></param>
        /// <param name="config"></param>
        /// <param name="err_code"></param>
        /// <returns></returns>
        public int RobotPosR(ushort robot_number, ref Posistion config, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x75, robot_number, 0, 0x01,
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
                    if (ans.data.Length >= 48)
                    {
                        config.AxisData.Axis_7 = BitConverter.ToInt32(ans.data, 44);
                    }
                    if (ans.data.Length >= 52)
                    {
                        config.AxisData.Axis_8 = BitConverter.ToInt32(ans.data, 48);
                    }
                }
                return ans.status;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class Posistion
    {
        public uint DataType = 0;
        public uint Figure = 0;
        public uint ToolNumber = 0;
        public uint UserCoordNumber = 0;
        public uint ExtendedType = 0;
        public Axis AxisData = new Axis();

        public override string ToString()
        {
            if (DataType == 0)
            {
                return $"DataType: {DataType} (Pulse),\r\n" +
                  $"Figure: {Figure},\r\n" +
                  $"ToolNumber: {ToolNumber},\r\n" +
                  $"UserCoordNumber: {UserCoordNumber},\r\n" +
                  $"ExtendedType: {ExtendedType},\r\n" +
                  $"AxisData:\r\n{AxisData}\r\n";
            }
            else if (DataType == 16)
            {
                return $"DataType: {DataType} (Coordinate),\r\n" +
                  $"Figure: {Figure},\r\n" +
                  $"ToolNumber: {ToolNumber},\r\n" +
                  $"UserCoordNumber: {UserCoordNumber},\r\n" +
                  $"ExtendedType: {ExtendedType},\r\n" +
                  $"AxisData:\r\n" +
                  $"X: {AxisData.Axis_1 / 1000.0:0.000}, " +
                  $"Y: {AxisData.Axis_2 / 1000.0:0.000}, " +
                  $"Z: {AxisData.Axis_3 / 1000.0:0.000}, " +
                  $"Rx: {AxisData.Axis_4 / 10000.0:0.0000}, " +
                  $"Ry: {AxisData.Axis_5 / 10000.0:0.0000}, " +
                  $"Rz: {AxisData.Axis_6 / 10000.0:0.0000}, " +
                  $"ax7: {AxisData.Axis_7}, " +
                  $"ax8: {AxisData.Axis_8}\r\n";
            }
            else
            {
                return "Undefined data type";
            }
        }
    }

    public class Axis
    {
        public int Axis_1 = 0;
        public int Axis_2 = 0;
        public int Axis_3 = 0;
        public int Axis_4 = 0;
        public int Axis_5 = 0;
        public int Axis_6 = 0;
        public int Axis_7 = 0;
        public int Axis_8 = 0;

        public override string ToString()
        {
            return 
                $"1: {Axis_1}, " +
                $"2: {Axis_2}, " +
                $"3: {Axis_3}, " +
                $"4: {Axis_4}, " +
                $"5: {Axis_5}, " +
                $"6: {Axis_6}, " +
                $"7: {Axis_7}, " +
                $"8: {Axis_8}";
        }
    }
}

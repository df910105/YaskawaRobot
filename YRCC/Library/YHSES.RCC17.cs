using System;
using System.Linq;
using YRCC.Packet;

namespace YRCC
{
    partial class YHSES
    {
        /*
        public int ReadBasePosData(ushort number, ref BasePosistion config, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x80, number, 0, 0x01,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    config.DataType = BitConverter.ToUInt32(ans.data, 0);
                    config.AxisData.Axis_1 = BitConverter.ToInt32(ans.data, 4);
                    config.AxisData.Axis_2 = BitConverter.ToInt32(ans.data, 8);
                    config.AxisData.Axis_3 = BitConverter.ToInt32(ans.data, 12);
                    config.AxisData.Axis_4 = BitConverter.ToInt32(ans.data, 16);
                    config.AxisData.Axis_5 = BitConverter.ToInt32(ans.data, 20);
                    config.AxisData.Axis_6 = BitConverter.ToInt32(ans.data, 24);
                    config.AxisData.Axis_7 = BitConverter.ToInt32(ans.data, 28);
                    config.AxisData.Axis_8 = BitConverter.ToInt32(ans.data, 32);
                }
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int WriteBasePosData(ushort number, BasePosistion config, out ushort err_code)
        {
            try
            {

                var bytes = ParsePositionDataBytes(config);
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x80, number, 0, 0x02,
                    bytes, (ushort)bytes.Length);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }*/

        private byte[] ParsePositionDataBytes(BasePosistion config)
        {
            try
            {
                return BitConverter.GetBytes(config.DataType)
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
            catch (Exception)
            {

                throw;
            }
        }
    }

    /// <summary>
    /// 軸資訊，需自行轉換單位
    /// </summary>
    public class BasePosistion
    {
        /// <summary>
        /// 軸型態
        /// </summary>
        public uint DataType = 0;

        /// <summary>
        /// 
        /// </summary>
        public Axis AxisData;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"DataType: {DataType},\r\n" +
                $"AxisData:\r\n{AxisData}\r\n";
        }
    }
}

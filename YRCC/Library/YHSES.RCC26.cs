using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YRCC.Packet;

namespace YRCC.Library
{
    partial class YHSES
    {
        public int SyStemInfoDataR(ushort number, ref SystemInfo info, out ushort err_code)
        {
            try
            {
                var req = new PacketReq(PacketHeader.HEADER_DIVISION_ROBOT_CONTROL, 0,
                    0x88, number, 0, 0x01,
                    new byte[0], 0);
                var ans = Transmit(req.ToBytes(), PORT_ROBOT_CONTROL);
                err_code = ans.added_status;
                if (ans.status == ERROR_SUCCESS)
                {
                    info.SysSoftwareVer = ascii.GetString(ans.data.Skip(0).Take(24).ToArray());
                    info.ModelName_App = ascii.GetString(ans.data.Skip(24).Take(16).ToArray());
                    info.ParameterVer = ascii.GetString(ans.data.Skip(40).Take(8).ToArray());
                }
                return ans.status;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class SystemInfo
    {
        public string SysSoftwareVer = string.Empty;
        public string ModelName_App = string.Empty;
        public string ParameterVer = string.Empty;

        public override string ToString()
        {
            return $"System software version: {SysSoftwareVer},\r\n" +
                $"Model name / application: {ModelName_App},\r\n" +
                $"Parameter version: {ParameterVer}\r\n";
        }
    }
}

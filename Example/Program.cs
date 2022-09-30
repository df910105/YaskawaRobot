using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YHSES.Library;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            YaskawaLib yrc1000 = new YaskawaLib("192.168.255.1");
            yrc1000.ServoSwitch(POWER_SWITCH.OFF, out _);
            var alarm = new AlarmData();
            yrc1000.ReadAlarmData(0, ref alarm, out _);
            Console.ReadLine();
        }
    }
}

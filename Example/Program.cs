using System;
using YRCC.Library;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            YHSES yrc1000 = new YHSES("192.168.255.1");
            yrc1000.ServoSwitch(POWER_SWITCH.OFF, out _);
            var alarm = new AlarmData();
            yrc1000.ReadAlarmData(0, ref alarm, out _);
            Console.ReadLine();
        }
    }
}

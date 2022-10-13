using System;
using System.Threading;
using YRCC.Library;

namespace Example
{
    class Program
    {

        static void Main(string[] args)
        {
            int rt;
            ushort err;

            //Initial.
            YHSES yrc1000 = new YHSES("192.168.255.1");
            Console.WriteLine("Robot safety check before press ENTER!!!");
            Console.ReadLine();

            try
            {
                //RCC01
                var alarm = new AlarmData();
                rt = yrc1000.ReadAlarmData(0, ref alarm, out err);
                ErrMsg(rt, err);

                //RCC06
                Posistion r = new Posistion();
                rt = yrc1000.RobotPosR(0, ref r, out err);
                ErrMsg(rt, err);
                Console.WriteLine(r);

                //RCC16
                Posistion p = new Posistion();
                rt = yrc1000.PosDataR(0, ref p, out err);
                ErrMsg(rt, err);
                Console.WriteLine(p);
                rt = yrc1000.PosDataR(1, ref p, out err);
                ErrMsg(rt, err);
                Console.WriteLine(p);
                rt = yrc1000.PosDataR(2, ref p, out err);
                Console.WriteLine(p);
                ErrMsg(rt, err);

                 //RCC20
                 rt = yrc1000.ServoSwitch(SWITCH.ON, out err);
                ErrMsg(rt, err);
                SpinWait.SpinUntil(() => false, 5000);
                rt = yrc1000.ServoSwitch(SWITCH.OFF, out err);
                ErrMsg(rt, err);

                rt = yrc1000.HoldSwitch(SWITCH.ON, out err);
                ErrMsg(rt, err);
                SpinWait.SpinUntil(() => false, 2000);
                rt = yrc1000.HoldSwitch(SWITCH.OFF, out err);
                ErrMsg(rt, err);

                //RCC21
                rt = yrc1000.SwitchCycleType(CYCLE_TYPE.Step, out err);
                ErrMsg(rt, err);
                SpinWait.SpinUntil(() => false, 2000);
                rt = yrc1000.SwitchCycleType(CYCLE_TYPE.Cycle, out err);
                ErrMsg(rt, err);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static void ErrMsg(int rt, ushort err)
        {
            Console.WriteLine($"result: {rt}, {err:X4}");
        }
    }
}

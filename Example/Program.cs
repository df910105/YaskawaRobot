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
                //RCC13
                int i32 = 0;
                rt = yrc1000.ReadDIntData(0, ref i32, out err);
                Console.WriteLine(i32);
                ErrMsg(rt, err);
                i32 = int.MinValue;
                Console.WriteLine(i32);
                rt = yrc1000.WriteDIntData(0, i32, out err);
                ErrMsg(rt, err);
                rt = yrc1000.ReadDIntData(0, ref i32, out err);
                Console.WriteLine(i32);
                ErrMsg(rt, err);

                #region -- Tested--
                Console.WriteLine("----------Tested----------\r\n");

                /*
                //RCC01
                var alarm = new AlarmData();
                rt = yrc1000.ReadAlarmData(0, ref alarm, out err);
                ErrMsg(rt, err);

                //RCC06
                Posistion r = new Posistion();
                rt = yrc1000.RobotPosR(1, ref r, out err);
                ErrMsg(rt, err);
                Console.WriteLine(r);
                //RCC06
                Posistion r1 = new Posistion();
                rt = yrc1000.RobotPosR(101, ref r1, out err);
                ErrMsg(rt, err);
                Console.WriteLine(r1);

                //RCC09
                byte b = 0;
                rt = yrc1000.ReadIOData(2701, ref b, out err);
                Console.WriteLine(b);
                ErrMsg(rt, err);
                b = 255;
                Console.WriteLine(b);
                rt = yrc1000.WriteIOData(2701, b, out err);
                ErrMsg(rt, err);
                rt = yrc1000.ReadIOData(2701, ref b, out err);
                Console.WriteLine(b);
                ErrMsg(rt, err);

                //RCC10
                ushort u16 = 0;
                rt = yrc1000.ReadRegData(1, ref u16, out err);
                Console.WriteLine(u16);
                ErrMsg(rt, err);
                u16 = 65535;
                Console.WriteLine(u16);
                rt = yrc1000.WriteRegData(1, u16, out err);
                ErrMsg(rt, err);
                rt = yrc1000.ReadRegData(1, ref u16, out err);
                Console.WriteLine(u16);
                ErrMsg(rt, err);

                //RCC11
                byte u8 = 0;
                rt = yrc1000.ReadByteData(0, ref u8, out err);
                Console.WriteLine(u8);
                ErrMsg(rt, err);
                u8 = 255;
                Console.WriteLine(u8);
                rt = yrc1000.WriteByteData(0, u8, out err);
                ErrMsg(rt, err);
                rt = yrc1000.ReadByteData(0, ref u8, out err);
                Console.WriteLine(u8);
                ErrMsg(rt, err);

                //RCC12
                short i16 = 0;
                rt = yrc1000.ReadIntData(0, ref i16, out err);
                Console.WriteLine(i16);
                ErrMsg(rt, err);
                i16 = -32767;
                Console.WriteLine(i16);
                rt = yrc1000.WriteIntData(0, i16, out err);
                ErrMsg(rt, err);
                rt = yrc1000.ReadIntData(0, ref i16, out err);
                Console.WriteLine(i16);
                ErrMsg(rt, err);

                //RCC13
                int i32 = 0;
                rt = yrc1000.ReadDIntData(0, ref i32, out err);
                Console.WriteLine(i32);
                ErrMsg(rt, err);
                i32 = int.MinValue;
                Console.WriteLine(i32);
                rt = yrc1000.WriteDIntData(0, i32, out err);
                ErrMsg(rt, err);
                rt = yrc1000.ReadDIntData(0, ref i32, out err);
                Console.WriteLine(i32);
                ErrMsg(rt, err);

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

                p.AxisData = new Axis
                {
                    Axis_1 = 0,
                    Axis_2 = 10,
                    Axis_3 = 20,
                    Axis_4 = 30,
                    Axis_5 = 50,
                    Axis_6 = 100,
                    Axis_7 = 0,
                    Axis_8 = 0,
                };
                p.DataType = 0;
                Console.WriteLine(p);
                rt = yrc1000.PosDataW(3, p, out err);
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
                ErrMsg(rt, err); */
                #endregion

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

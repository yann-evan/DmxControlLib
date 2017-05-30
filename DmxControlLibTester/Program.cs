using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DmxControlLib.Hardware;
using DmxControlLib.Utility;

namespace DmxControlLibTester
{
    class Program
    {
        static void Main(string[] args)
        {
            DmxController.Connect(); //connection du controlleur DMX
            LaunchPadControl.Connect(); //connection du launchpad Midi

            LaunchPadControl.LaunchPadInput += Inputevent;

            #region Test DmxControlleur
            DmxController.WriteValue(1, 255);
            DmxController.WriteValue(6, 255);
            DmxController.WriteValue(8, 128);
            #endregion

            #region Test LaunchPadControl

            //O
            LaunchPadControl.Led(33, ButtonColor.Green, false, false);

            LaunchPadControl.Led(48, ButtonColor.Green, false, false);
            LaunchPadControl.Led(64, ButtonColor.Green, false, false);

            LaunchPadControl.Led(81, ButtonColor.Green, false, false);

            LaunchPadControl.Led(50, ButtonColor.Green, false, false);
            LaunchPadControl.Led(66, ButtonColor.Green, false, false);

            //K
            LaunchPadControl.Led(36, ButtonColor.Green, false, false);
            LaunchPadControl.Led(52, ButtonColor.Green, false, false);
            LaunchPadControl.Led(68, ButtonColor.Green, false, false);
            LaunchPadControl.Led(84, ButtonColor.Green, false, false);

            LaunchPadControl.Led(53, ButtonColor.Green, false, false);
            LaunchPadControl.Led(38, ButtonColor.Green, false, false);

            LaunchPadControl.Led(69, ButtonColor.Green, false, false);
            LaunchPadControl.Led(86, ButtonColor.Green, false, false);

            Console.ReadKey(false);

            LaunchPadControl.Led(33, ButtonColor.None, false, false);

            LaunchPadControl.Led(48, ButtonColor.None, false, false);
            LaunchPadControl.Led(64, ButtonColor.None, false, false);

            LaunchPadControl.Led(81, ButtonColor.None, false, false);

            LaunchPadControl.Led(50, ButtonColor.None, false, false);
            LaunchPadControl.Led(66, ButtonColor.None, false, false);

            LaunchPadControl.Led(36, ButtonColor.None, false, false);
            LaunchPadControl.Led(52, ButtonColor.None, false, false);
            LaunchPadControl.Led(68, ButtonColor.None, false, false);
            LaunchPadControl.Led(84, ButtonColor.None, false, false);

            LaunchPadControl.Led(53, ButtonColor.None, false, false);
            LaunchPadControl.Led(38, ButtonColor.None, false, false);

            LaunchPadControl.Led(69, ButtonColor.None, false, false);
            LaunchPadControl.Led(86, ButtonColor.None, false, false);

            #endregion

        }

        public static void Inputevent(object sender ,LaunchPadInputEventArgs e)
        {
            LaunchPadControl.RunLedtest();

            DmxController.WriteValue(1, 0);
            DmxController.WriteValue(6, 0);
            DmxController.WriteValue(8, 0);

            DmxController.Close();
            LaunchPadControl.close();

            Environment.Exit(0);
        }
    }
}

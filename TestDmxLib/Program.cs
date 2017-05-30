using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DmxControlLib.Hardware;
using DmxControlLib.Utility;

namespace TestDmxLib
{
    class Program
    {
        static void Main(string[] args)
        {
            DmxController.Connect(); //connection du controlleur DMX
            LaunchPadControl.Connect(); //connection du launchpad Midi

            #region Test DmxControlleur
            DmxController.WriteValue(1, 255);
            DmxController.WriteValue(6, 255);
            DmxController.WriteValue(8, 128);
            #endregion

            #region Test LaunchPadControl

            //O
            LaunchPadControl.LedOn(33, "Green", false, false);

            LaunchPadControl.LedOn(48, "Green", false, false);
            LaunchPadControl.LedOn(64, "Green", false, false);

            LaunchPadControl.LedOn(81, "Green", false, false);

            LaunchPadControl.LedOn(50, "Green", false, false);
            LaunchPadControl.LedOn(66, "Green", false, false);

            //K
            LaunchPadControl.LedOn(36, "Green", false, false);
            LaunchPadControl.LedOn(52, "Green", false, false);
            LaunchPadControl.LedOn(68, "Green", false, false);
            LaunchPadControl.LedOn(84, "Green", false, false);

            LaunchPadControl.LedOn(53, "Green", false, false);
            LaunchPadControl.LedOn(38, "Green", false, false);

            LaunchPadControl.LedOn(69, "Green", false, false);
            LaunchPadControl.LedOn(86, "Green", false, false);
            #endregion

        }
    }
}

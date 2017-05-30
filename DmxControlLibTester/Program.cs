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
        public static LPMMap Mapping;

        static void Main(string[] args)
        {
            int ind = 0;

            DmxController.Connect(); //connection du controlleur DMX
            LaunchPadControl.Connect(); //connection du launchpad Midi

            Mapping = new LPMMap("Launchpad Mini");

            #region Test DmxControlleur
            DmxController.WriteValue(1, 255);
            DmxController.WriteValue(6, 255);
            DmxController.WriteValue(8, 128);
            #endregion

            #region Test Mapping
            ind = Mapping.BT.FindIndex(x => x.ID == 2);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Ambre;
            Mapping.BT[ind].onFlashing = true;
            Mapping.BT[ind].offColor = ButtonColor.Green;
            Mapping.BT[ind].offFlashing = false;


            ind = Mapping.BT.FindIndex(x => x.ID == 19);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Red;
            Mapping.BT[ind].onFlashing = true;
            Mapping.BT[ind].offColor = ButtonColor.Red;
            Mapping.BT[ind].offFlashing = false;


            ind = Mapping.BT.FindIndex(x => x.ID == 37);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.None;
            Mapping.BT[ind].onFlashing = false;
            Mapping.BT[ind].offColor = ButtonColor.Yellow;
            Mapping.BT[ind].offFlashing = false;


            ind = Mapping.BT.FindIndex(x => x.ID == 72);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Red;
            Mapping.BT[ind].onFlashing = false;
            Mapping.BT[ind].offColor = ButtonColor.Green;
            Mapping.BT[ind].offFlashing = true;


            ind = Mapping.BT.FindIndex(x => x.ID == 99);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Red;
            Mapping.BT[ind].onFlashing = false;
            Mapping.BT[ind].offColor = ButtonColor.Green;
            Mapping.BT[ind].offFlashing = true;


            ind = Mapping.BT.FindIndex(x => x.ID == 64);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Red;
            Mapping.BT[ind].onFlashing = false;
            Mapping.BT[ind].offColor = ButtonColor.Green;
            Mapping.BT[ind].offFlashing = true;


            ind = Mapping.BT.FindIndex(x => x.ID == 82);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Red;
            Mapping.BT[ind].onFlashing = false;
            Mapping.BT[ind].offColor = ButtonColor.Green;
            Mapping.BT[ind].offFlashing = true;


            ind = Mapping.BT.FindIndex(x => x.ID == 117);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Red;
            Mapping.BT[ind].onFlashing = false;
            Mapping.BT[ind].offColor = ButtonColor.Green;
            Mapping.BT[ind].offFlashing = true;


            ind = Mapping.BT.FindIndex(x => x.ID == 0);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Red;
            Mapping.BT[ind].onFlashing = false;
            Mapping.BT[ind].offColor = ButtonColor.Green;
            Mapping.BT[ind].offFlashing = true;


            ind = Mapping.BT.FindIndex(x => x.ID == 120);

            Mapping.BT[ind].Type = buttonType.Momentary;
            Mapping.BT[ind].onColor = ButtonColor.Red;
            Mapping.BT[ind].onFlashing = false;
            Mapping.BT[ind].offColor = ButtonColor.Green;
            Mapping.BT[ind].offFlashing = true;

            ind = Mapping.SysBT.FindIndex(x => x.ID == 1);

            Mapping.SysBT[ind].Type = buttonType.Momentary;
            Mapping.SysBT[ind].onColor = ButtonColor.Red;
            Mapping.SysBT[ind].onFlashing = false;
            Mapping.SysBT[ind].offColor = ButtonColor.Green;
            Mapping.SysBT[ind].offFlashing = true;

            #endregion

            LaunchPadControl.LinkMapping(Mapping);

            Console.ReadKey(true);


            DmxController.WriteValue(1, 0);
            DmxController.WriteValue(6, 0);
            DmxController.WriteValue(8, 0);
            LaunchPadControl.RunLedtest();
            Environment.Exit(0);

        }

    }
}

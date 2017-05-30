using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DmxControlLib.Utility
{
    public class LPMMap
    {

        string name;

        public List<Button> SysBT;
        public List<Button> BT;

        public LPMMap(string N)
        {
            Trace.Listeners.Add(new TextWriterTraceListener("LaunchpadMappingInit.log", "Launchpad Mapping Init"));
            Trace.TraceInformation("Launch date: " + DateTime.Today.ToString());
            name = N;

            SysBT = new List<Button>();
            BT = new List<Button>();

            for (int i = 0; i < 8; i++)
            {
                SysBT.Add(new Button(i));
                Trace.TraceInformation("new SystemButton - ID -> " + i);
                Trace.Flush();
            }

            for (int y = 0; y < 120; y += 16)
            {
                for (int i = 0; i < 9; i++)
                {
                    BT.Add(new Button(y + i));
                    Trace.TraceInformation("new Button - ID -> " + (i + y));
                    Trace.Flush();
                }
            }
        }

    }

    public class Button
    {
        public int ID;
        public buttonType Type;

        //off
        public ButtonColor offColor;
        public bool offFlashing;

        //on
        public ButtonColor onColor;
        public bool onFlashing;

        public Button(int bID)
        {
            ID = bID;
            Type = buttonType.Momentary;

            offColor = ButtonColor.None;
            offFlashing = false;

            onColor = ButtonColor.None;
            onFlashing = false;
        }
    }
}

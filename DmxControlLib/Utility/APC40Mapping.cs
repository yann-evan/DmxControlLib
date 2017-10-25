using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxControlLib.Utility
{
    [Serializable]
    public class APC40Mapping
    {

        public string name;

        public List<RGBButton> RGBBT;

        public APC40Mapping(string N)
        {
            name = N;

            RGBBT = new List<RGBButton>();

            for (int i = 0; i < 40; i++)
            {
                RGBBT.Add(new RGBButton(i));
            }

            for (int i = 82; i < 87; i++)
            {
                RGBBT.Add(new RGBButton(i));
            }
        }

    }

    [Serializable]
    public class RGBButton
    {
        public int ID;
        public buttonType Type;
        public int Groupe; //-1 pour none
        public bool IsOnToogle;

        //off
        public int offColor;
        public bool offFlashing;

        //on
        public int onColor;
        public bool onFlashing;

        public RGBButton(int bID)
        {
            ID = bID;

            IsOnToogle = false;

            Type = buttonType.Momentary;

            offColor = 0;
            offFlashing = false;

            onColor = 0;
            onFlashing = false;

            Groupe = -1;
        }
    }
}

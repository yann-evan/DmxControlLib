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
        /// <summary>
        /// Nom du mapping
        /// </summary>
        public string name;

        /// <summary>
        /// Liste des boutons RGB
        /// </summary>
        public List<RGBButton> RGBBT;

        /// <summary>
        /// Initialisation du Mapping
        /// </summary>
        /// <param name="N"></param>
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
        public int offprimaryColor;
        public int offsecondaryColor;
        public BlinkingType offFlashingtype;
        public BlinkingSpeed offFlashingspeed;

        //on
        public int onprimaryColor;
        public int onsecondaryColor;
        public BlinkingType onFlashingtype;
        public BlinkingSpeed onFlashingspeed;

        public RGBButton(int bID)
        {
            ID = bID;

            IsOnToogle = false;

            Type = buttonType.Momentary;

            offprimaryColor = 0;
            offsecondaryColor = 0;
            offFlashingtype = BlinkingType.OneShot;
            offFlashingspeed = BlinkingSpeed._1_2;

            onprimaryColor = 0;
            onsecondaryColor = 0;
            onFlashingtype = BlinkingType.OneShot;
            onFlashingspeed = BlinkingSpeed._1_2;

            Groupe = -1;
        }
    }
}

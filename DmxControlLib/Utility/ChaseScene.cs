using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxControlLib.Utility
{
    public class Scene
    {
        public string name;
        public int delay;
        public int timeAfter;

        private int[] dmxValue = new int[512];

        public Scene(String name)
        {
            this.name = name;

            for (int i = 0; i < dmxValue.Length; i++)
            {
                dmxValue[i] = 0;
            }
        }

        public int[] getDmxValueArray()
        {
            return dmxValue;
        }

        public bool setDmxValueArray(int[] values)
        {
            if (values.Length != 512)
            {
                return false;
            }

            foreach (int val in dmxValue)
            {
                if (val > 255 || val < 0)
                {
                    return false;
                }
            }

            dmxValue = values;

            return true;
        }

        public int getDmxValue(int channel)
        {
            return dmxValue[channel];
        }

        public bool setDmxValue(int channel, int value)
        {
            if (value > 255 || value < 0)
            {
                return false;
            }

            dmxValue[channel] = value;
            return true;
        }

        public override string ToString()
        {
            string text = this.name + "\n";

            for (int i = 0; i < dmxValue.Length; i++)
            {
                text = text + (i + 1) + " : " + dmxValue[i] + "\n";
            }

            return text;
        }
    }

    public class Chase
    {
        public string name;
        public List<Scene> chaseScene = new List<Scene>();

        public Chase(string name)
        {
            this.name = name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxControlLib.Utility
{
    public class lightingDescription
    {
        public string name;
        public List<Channel> Channels;

        public lightingDescription(string name)
        {
            this.name = name;
            Channels = new List<Channel>();
        }

        public void addChannel(string name, Channeltype type, int ChannelNumber)
        {
            Channels.Add(new Channel(name, type, ChannelNumber));
        }

        public bool RmChannel(String name)
        {
            try
            {
                Channels.RemoveAt(Channels.FindIndex(x => x.name == name));
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public class Channel
        {
            public string name;
            public Channeltype type;
            public int ChannelNumber;
            public List<preset> Presets;

            public Channel(string name, Channeltype type, int ChannelNumber)
            {
                this.name = name;
                this.type = type;
                this.ChannelNumber = ChannelNumber;
                Presets = new List<preset>();
            }

            public void addPreset(String name, int Max, int Min, int Def)
            {
                Presets.Add(new preset(name, Max, Min, Def));
            }

            public bool RmPreset(String name)
            {

                try
                {
                    Presets.RemoveAt(Presets.FindIndex(x => x.name == name));
                    return true;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
            }

            public class preset
            {
                public int MaxValue;
                public int MinValue;
                public int DefValue;

                public String name;

                public preset(String name, int Max, int Min, int Def)
                {
                    this.name = name;

                    MaxValue = Max;
                    MinValue = Min;
                    DefValue = Def;
                }


            }
        }
    }
}

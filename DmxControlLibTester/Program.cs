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
            bool exit = false;

            #region construction de la description du jeu
            lightingDescription froggy = new lightingDescription("Froggy");

            froggy.addChannel("Dimmer", Channeltype.Dimmer, 1);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Dimmer"))].addPreset("Dimmer", 255, 0, 128);

            froggy.addChannel("Strobe", Channeltype.other, 2);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Strobe"))].addPreset("Strobe Speed", 255, 0, 128);

            froggy.addChannel("Rouge", Channeltype.RGB, 3);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Rouge"))].addPreset("Rouge", 255, 0, 128);

            froggy.addChannel("Vert", Channeltype.RGB, 4);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Vert"))].addPreset("Vert", 255, 0, 128);

            froggy.addChannel("Bleu", Channeltype.RGB, 5);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Bleu"))].addPreset("Bleu", 255, 0, 128);

            froggy.addChannel("Blanc", Channeltype.other, 6);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Blanc"))].addPreset("Blanc", 255, 0, 128);

            froggy.addChannel("PAN", Channeltype.Pan, 7);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("PAN"))].addPreset("Pan", 255, 0, 128);

            froggy.addChannel("TILT", Channeltype.Tilt, 8);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("TILT"))].addPreset("Tilt", 255, 0, 128);

            froggy.addChannel("Vitesse PAN/TILT", Channeltype.other, 9);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Vitesse PAN/TILT"))].addPreset("speed", 255, 0, 128);

            froggy.addChannel("Mode", Channeltype.other, 10);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Mode"))].addPreset("Manuel", 0, 0, 0);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Mode"))].addPreset("Auto", 249, 1, 124);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("Mode"))].addPreset("Son", 255, 250, 253);

            froggy.addChannel("reset", Channeltype.other, 11);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("reset"))].addPreset("Off", 254, 0, 127);
            froggy.Channels[froggy.Channels.FindIndex(x => x.name.Equals("reset"))].addPreset("Reset", 255, 255, 255);
            #endregion

            DmxController.Connect();

            int PanChannel = froggy.Channels.Find(x => x.type == Channeltype.Pan).ChannelNumber;
            int TiltChannel = froggy.Channels.Find(x => x.type == Channeltype.Tilt).ChannelNumber;

            DmxController.WriteValue(froggy.Channels.Find(x => x.type == Channeltype.Dimmer).ChannelNumber, 255);
            DmxController.WriteValue(froggy.Channels.Find(x => x.name == "Blanc").ChannelNumber, 255);

            while (!exit)
            {
                if(Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(false);

                    switch(key.Key)
                    {
                        case ConsoleKey.UpArrow :
                            DmxController.WriteValue(PanChannel, 200);
                            break;

                        case ConsoleKey.DownArrow :
                            DmxController.WriteValue(PanChannel, 100);
                            break;

                        case ConsoleKey.LeftArrow :
                            DmxController.WriteValue(TiltChannel, 100);
                            break;

                        case ConsoleKey.RightArrow :
                            DmxController.WriteValue(TiltChannel, 200);
                            break;

                        case ConsoleKey.Escape :
                            exit = true;
                            break;
                    }
                }
            }

            DmxController.Close();

        }


    }
}

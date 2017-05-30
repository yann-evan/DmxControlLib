using System;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia;

namespace DmxControlLib.Hardware
{
        public static class LaunchPadControl
        {

            const string midiDeviceName = "Launchpad Midi"; //nom du controller Midi

            /// <summary>
            /// Flux d'entrée depuis Launchpad
            /// </summary>
            static private InputDevice _InputLaunchPad;

            /// <summary>
            /// Flux de sortie vers launchpad
            /// </summary>
            static private OutputDevice _OutputLaunchPad;

            /// <summary>
            /// builder pour message midi
            /// </summary>
            static private ChannelMessageBuilder _msg;

            /// <summary>
            /// nombre de materielle midi disponible en sortie
            /// </summary>
            static private int OutDevCount;

            /// <summary>
            /// nombre de materielle midi disponible en entrée
            /// </summary>
            static private int InDevCount;

            /// <summary>
            /// ID du launchpad Out
            /// </summary>
            static private int LaunchpadOutID;

            /// <summary>
            /// ID du launchpad In
            /// </summary>
            static private int LaunchpadInID;

            /// <summary>
            /// état du controlleur
            /// </summary>
            static private bool connected = false;

            /// <summary>
            /// Evenement Input du Launchpad
            /// </summary>
            public static EventHandler<LaunchPadInputEventArgs> LaunchPadInput;


            /// <summary>
            /// connexion au launchpad
            /// </summary>
            /// <param name="name"></param>
            /// <exception cref="AlreadyConnectedMidiDevice"></exception>
            /// <exception cref="ErrorMidiConnexionException"></exception>
            public static void Connect()
            {
                _msg = new ChannelMessageBuilder();

                if (!connected)
                {
                    try
                    {
                        OutDevCount = OutputDevice.DeviceCount;
                        InDevCount = InputDevice.DeviceCount;

                        for (int i = 0; i < OutDevCount; i++)
                        {
                            if (OutputDevice.GetDeviceCapabilities(i).name == midiDeviceName)
                            {
                                LaunchpadOutID = i;
                            }
                        }

                        for (int i = 0; i < InDevCount; i++)
                        {
                            if (InputDevice.GetDeviceCapabilities(i).name == midiDeviceName)
                            {
                                LaunchpadInID = i;
                            }
                        }

                        _InputLaunchPad = new InputDevice(LaunchpadInID);
                        _InputLaunchPad.ChannelMessageReceived += HandleChannelMessageReceived;

                        _OutputLaunchPad = new OutputDevice(1);

                        _InputLaunchPad.StartRecording();

                        connected = true;
                    }
                    catch (Exception)
                    {
                        throw new ErrorMidiConnexionException();
                    }
                }
                else
                {
                    throw new AlreadyConnectedMidiDevice();
                }
            }

            /// <summary>
            /// allume toute les leds du launchpad puis Reset
            /// </summary>
            public static void RunLedtest()
            {

                SendChannel(ChannelCommand.Controller, 0, 0, 127);

                System.Threading.Thread.Sleep(1000);

                SendChannel(ChannelCommand.Controller, 0, 0, 0);

            }

            /// <summary>
            /// Déconnecte le Controlleur
            /// </summary>
            public static void close()
            {
                if (_InputLaunchPad != null)
                {
                    _InputLaunchPad.Close();
                }
                if (_OutputLaunchPad != null)
                {
                    _OutputLaunchPad.Close();
                }

                connected = false;

            }

            /// <summary>
            /// Vrai si le controlleur est connecté
            /// </summary>
            /// <returns></returns>
            public static bool IsConnect()
            {
                return connected;
            }

            /// <summary>
            /// Envoi une instruction Midi
            /// </summary>
            /// <param name="type"></param>
            /// <param name="channel"></param>
            /// <param name="data1"></param>
            /// <param name="data2"></param>
            private static void SendChannel(ChannelCommand type, int channel, int data1, int data2)
            {
                if (IsConnect())
                {
                    try
                    {
                        _msg.Command = type;
                        _msg.MidiChannel = channel;
                        _msg.Data1 = data1;
                        _msg.Data2 = data2;

                        _msg.Build();

                        _OutputLaunchPad.Send(_msg.Result);
                    }
                    catch (Exception)
                    {
                        throw new ErrorSendingDataException();
                    }
                }
                else
                {
                    throw new NoMidiDeviceConnectedException();
                }
            }

            /// <summary>
            /// Allume une Led
            /// </summary>
            /// <param name="position"></param>
            /// <param name="color"></param>
            /// <param name="flashing"></param>
            /// <param name="systemLed"></param>
            public static void LedOn(int position, String color, bool flashing, bool systemLed)
            {
                int velocity = GetVelocity(color, flashing);

                if (systemLed && position >= 0 && position <= 7)
                {
                    position = position + 104;
                    SendChannel(ChannelCommand.Controller, 0, position, velocity);
                }
                else if (position >= 0 && position <= 120)
                {
                    SendChannel(ChannelCommand.NoteOn, 0, position, velocity);
                }

            }

            /// <summary>
            /// éteint une Led
            /// </summary>
            /// <param name="position"></param>
            /// <param name="systemLed"></param>
            public static void LedOff(int position, bool systemLed)
            {
                int velocity = 12;

                if (systemLed && position >= 0 && position <= 7)
                {
                    position = position + 104;
                    SendChannel(ChannelCommand.Controller, 0, position, velocity);
                }
                else if (position >= 0 && position <= 120)
                {
                    SendChannel(ChannelCommand.NoteOn, 0, position, velocity);
                }


            }

            /// <summary>
            /// renvoi la Data1 "Velocity" pour le LaunchPad
            /// </summary>
            /// <param name="color"></param>
            /// <param name="flashing"></param>
            /// <returns></returns>
            private static int GetVelocity(string color, bool flashing)
            {
                int tempvel = 0;
                if (color.Equals("Red"))
                {
                    tempvel = 15;
                }
                else if (color.Equals("Green"))
                {
                    tempvel = 60;
                }
                else if (color.Equals("Yellow"))
                {
                    tempvel = 62;
                }
                else if (color.Equals("Ambre"))
                {
                    tempvel = 63;
                }
                else
                {
                    tempvel = -1;
                }

                if (flashing)
                {
                    tempvel = tempvel - 4;
                    SendChannel(ChannelCommand.Controller, 0, 0, 40);
                }


                return tempvel;
            }

            /// <summary>
            /// Event Input, Génere un événement avec la position et l'état de l'input
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private static void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
            {
                bool ISSYSTEMLED;
                int POSITION;
                bool ISON;

                if (e.Message.Command == ChannelCommand.Controller)
                {
                    ISSYSTEMLED = true;
                    POSITION = e.Message.Data1 - 104;
                }
                else
                {
                    ISSYSTEMLED = false;
                    POSITION = e.Message.Data1;
                }

                if (e.Message.Data2 == 127)
                {
                    ISON = true;
                }
                else
                {
                    ISON = false;
                }

                if (LaunchPadInput != null)
                {
                    LaunchPadInput(_InputLaunchPad, new LaunchPadInputEventArgs { isSystemLed = ISSYSTEMLED, position = POSITION, isOn = ISON, });
                }
            }

        }

        #region LaunchPad Event
        public class LaunchPadInputEventArgs : EventArgs //Input depuis le LaunchPad
        {
            public bool isSystemLed { get; set; } //Si Input Sur les Boutons Systemes
            public int position { get; set; } //Position du bouton
            public bool isOn { get; set; } //Si le Btn est appuyé

        }
        #endregion

        #region Midi Exception
        public class NoMidiDeviceConnectedException : Exception
        {
            public NoMidiDeviceConnectedException() : base("Aucun Controlleur Midi de connecté")
            {
            }
        }

        public class ErrorMidiConnexionException : Exception
        {
            public ErrorMidiConnexionException() : base("Impossible de connecté un controlleur Midi")
            {
            }
        }

        public class AlreadyConnectedMidiDevice : Exception
        {
            public AlreadyConnectedMidiDevice() : base("Ce controlleur Midi est déja connecté")
            {
            }
        }

        public class WrongColorException : Exception
        {
            public WrongColorException() : base("Mauvaise Couleur")
            {
            }
        }

        public class ErrorSendingDataException : Exception
        {
            public ErrorSendingDataException() : base("Erreur d'envoi")
            {

            }
        }
        #endregion
    }

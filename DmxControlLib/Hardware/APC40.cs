using System;
using Sanford.Multimedia.Midi;
using DmxControlLib.Utility;
using System.Diagnostics;

namespace DmxControlLib.Hardware
{
    /*
     * nom : APC40
     * Version : B1.0
     * auteur : Yann-evan morcet
     * description : Controlle et gestion des Output Input MIDI du controlleur MIDI AKAI APC-40(Evenement Input, leds ...)
     * état : dévellopement
     */

    public class APC40
    {
        #region constantes

        /// <summary>
        /// nom du port virtuel
        /// </summary>
        private const string VirtualPortName = "VirtualPort";

        /// <summary>
        /// Nom MIDI de l'APC40
        /// </summary>
        private const string APC40Name = "APC40 mkII";

        /// <summary>
        /// Data d'initialisation de l'interface(Mode 0)
        /// </summary>
        private readonly byte[] InitConfByteMode0 = { 0xf0, 0x47, 0x00, 0x29, 0x60, 0x00, 0x04, 0x40, 0x08, 0x04, 0x01, 0xf7 };

        /// <summary>
        /// Data d'initialisation de l'interface(Mode 1)
        /// </summary>
        private readonly byte[] InitConfByteMode1 = { 0xf0, 0x47, 0x00, 0x29, 0x60, 0x00, 0x04, 0x41, 0x08, 0x04, 0x01, 0xf7 };

        /// <summary>
        /// Data d'initialisation de l'interface(Mode 2)
        /// </summary>
        private readonly byte[] InitConfByteMode2 = { 0xf0, 0x47, 0x00, 0x29, 0x60, 0x00, 0x04, 0x42, 0x08, 0x04, 0x01, 0xf7 };

        #endregion

        #region proprietés

        /// <summary>
        /// Flux d'entrée duis l'APC40
        /// </summary>
        private InputDevice _InputAPC40;

        /// <summary>
        /// Flus de sortie vers APC40
        /// </summary>
        private OutputDevice _OuputAPC40;

        /// <summary>
        /// Port MIDI virtuel
        /// </summary>
        private TeVirtualMIDI _virtualMidiPort;

        /// <summary>
        /// Constructeur de message MIDI
        /// </summary>
        private ChannelMessageBuilder _msg;

        /// <summary>
        /// Mapping de l'APC40
        /// </summary>
        public APC40Mapping Mapping;

        /// <summary>
        /// Nombre d'interface Midi disponible en entrée
        /// </summary>
        private int InDevCount;

        /// <summary>
        /// Nombre d'interface Midi disponible en sortie
        /// </summary>
        private int OutDevCount;

        /// <summary>
        /// ID du flux d'entrée de l'APC40
        /// </summary>
        private int APC40InID = -1;

        /// <summary>
        /// ID du flux de sortie de l'APC40
        /// </summary>
        private int APC40OutID = -1;

        /// <summary>
        /// Vrai si l'interface est connectée
        /// </summary>
        private bool connected = false;

        /// <summary>
        /// Evenement Input d'un codeur sur APC40
        /// </summary>
        public event EventHandler<APC40InputCodeEventArgs> APC40_Code_Input;

        /// <summary>
        /// Evenement Input d'un Potentiometre sur APC40
        /// </summary>
        public event EventHandler<APC40InputPotEventArgs> APC40_Pot_Input;

        /// <summary>
        /// Evenement Input d'un boutton sur APC40
        /// </summary>
        public event EventHandler<APC40InputButtonEventArgs> APC40_Button_Input;

        /// <summary>
        /// Evenement Input d'un boutton groupé sur APC40
        /// </summary>
        public event EventHandler<APC40InputGroupedButtonEventArgs> APC40_GroupedButton_Input;

        /// <summary>
        /// Evenement Input d'un Fader groupé sur APC40
        /// </summary>
        public event EventHandler<APC40InputGroupedFaderEventArgs> APC40_GroupedFader_Input;

        #endregion

        /// <summary>
        /// Constructeur + decouverte MIDI
        /// </summary>
        /// <exception cref="AlreadyConnectedAPC40Device"
        /// <exception cref="NoAPC40FoundException"
        public APC40()
        {

            _msg = new ChannelMessageBuilder(); //Init du builder de msg MIDI

            if(!connected)
            {
                Log.writeLine("Initialisation de l'APC40");

                InDevCount = InputDevice.DeviceCount;
                Log.writeLine("Nombre de d'interface MIDI en Input détecté -> " + InDevCount);
                OutDevCount = OutputDevice.DeviceCount;
                Log.writeLine("Nombre de d'interface MIDI en Output détecté -> " + OutDevCount);
                #region Découverte Interfaces MIDI

                Log.writeLine("");
                Log.writeLine("Decouverte des interfaces :");

                //Interfaces d'entrées
                Log.writeLine("interface MIDI trouvées en entrée");
                for (int i = 0; i < InDevCount; i++)
                {
                    Log.writeLine(i + ">" + InputDevice.GetDeviceCapabilities(i).name);
                    if (InputDevice.GetDeviceCapabilities(i).name == APC40Name)
                    {
                        APC40InID = i;
                    }
                }

                //interfaces de sorties
                Log.writeLine("interface MIDI trouvées en sortie");
                for (int i = 0; i < OutDevCount; i++)
                {
                    Log.writeLine(i + ">" + OutputDevice.GetDeviceCapabilities(i).name);
                    if (OutputDevice.GetDeviceCapabilities(i).name == APC40Name)
                    {
                        APC40OutID = i;
                    }
                }

                if(APC40InID == -1 || APC40OutID == -1)
                {
                    connected = true;
                    close();

                    Log.writeLine("Aucun APC40 trouvée");
                    Log.writeLine("----------------------------------------------------------------------------");
                    Log.writeLine("----------------------------------------------------------------------------");

                    throw new NoAPC40FoundException(); //aucune interface trouvée
                }

                _virtualMidiPort = new TeVirtualMIDI("VirtualPort"); //Init du port virtuel
                #endregion


            }
            else
            {
                Log.writeLine("APC40 déja connectée");
                Log.writeLine("----------------------------------------------------------------------------");
                Log.writeLine("----------------------------------------------------------------------------");
                throw new AlreadyConnectedAPC40Device(); //interface déja connectée
            }


        }

        /// <summary>
        /// Connexion MIDI de l'APC40
        /// </summary>
        /// <exception cref="ErrorAPC40ConnexionException"
        public void open()
        {
            try
            {
                Log.writeLine("----------------------------------------------------------------------------");
                Log.writeLine("Connexion APC40");

                _InputAPC40 = new InputDevice(APC40InID); //Init du flux d'entrée MIDI de l'interface
                _OuputAPC40 = new OutputDevice(APC40OutID); //Init du flux de sortie MIDI de l'interface

                _InputAPC40.ChannelMessageReceived += _InputAPC40_ChannelMessageReceived; //ajout évenement Input

                _InputAPC40.StartRecording(); //début du scan d'entrée MIDI

                _OuputAPC40.Send(new SysExMessage(InitConfByteMode1)); //envoi du message d'Initialisation de l'interface, passage en mode 1 (voir DOC Dev APC40)


                connected = true;

            }
            catch
            {
                Log.writeLine("Impossible de connectér l'APC40");
                Log.writeLine("----------------------------------------------------------------------------");
                Log.writeLine("----------------------------------------------------------------------------");
                throw new ErrorAPC40ConnexionException(); //erreur de connexion
            }
            
        }

        /// <summary>
        /// Déconnexion MIDI de l'APC40
        /// </summary>
        /// <exception cref="AlreadyDisconnectedAPC40Device"
        public void close()
        {
            if(connected)
            {
                try
                {
                    Log.writeLine("Déconnexion APC40");

                    if (_virtualMidiPort != null) //déconnexion du MIDI virtuel
                    {
                        _virtualMidiPort.shutdown();
                    }
                    if (_InputAPC40 != null) //déconnexion Input
                    {
                        _InputAPC40.StopRecording(); //fin de la capture des Input
                        _InputAPC40.Close();
                    }
                    if (_OuputAPC40 != null) //déconnexion Output
                    {
                        _OuputAPC40.Send(new SysExMessage(InitConfByteMode0)); //remise en mode par defaut
                        _OuputAPC40.Close();
                    }
                }
                catch
                {
                    Log.writeLine("Erreur de deconnexion de l'APC40");
                    Log.writeLine("----------------------------------------------------------------------------");
                    Log.writeLine("----------------------------------------------------------------------------");
                    throw new ErrorAPC40DisconnectionException();
                }
                connected = false;
                
            }
            else
            {
                Log.writeLine("APC40 déja déconnecté");
                Log.writeLine("----------------------------------------------------------------------------");
                Log.writeLine("----------------------------------------------------------------------------");
                throw new AlreadyDisconnectedAPC40Device(); //interface deja deconnecté
            }
            
        }

        /// <summary>
        /// Evenement quand nouveaux message du APC40
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e" ></param>
        private void _InputAPC40_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            _InputAPC40.StopRecording();

            Log.writeLine("----------------------------------------------------------------------------");
            Log.writeLine("Nouvelle Input : " + e.Message.Message);
            if (e.Message.Command == ChannelCommand.Controller)
            {
                if(e.Message.Data1 == 47 || e.Message.Data1 == 13) //Si Codeur
                {
                    APC40_Code_ID _CODEID;
                    bool _ISCLOCKWISE;

                    _CODEID = (APC40_Code_ID)e.Message.Data1;

                    if(e.Message.Data2 == 127)
                    {
                        _ISCLOCKWISE = false;
                    }
                    else
                    {
                        _ISCLOCKWISE = true;
                    }

                    if(APC40_Code_Input != null)
                    {
                        APC40_Code_Input(this, new APC40InputCodeEventArgs { CodeID = _CODEID, IsClockWise = _ISCLOCKWISE });
                    }
                } 
                else
                {
                    if (e.Message.Data1 == 7) //si fader groupé
                    {
                        int _CHANNEL;
                        int _VALUE;

                        _CHANNEL = e.Message.MidiChannel;
                        _VALUE = e.Message.Data2;

                        if (APC40_GroupedFader_Input != null)
                        {
                            APC40_GroupedFader_Input(this, new APC40InputGroupedFaderEventArgs { channel = _CHANNEL, value = _VALUE });
                        }
                    }
                    else //Si Potentiometre
                    {
                        APC40_pot_ID _POTID;
                        int _VALUE;

                        _POTID = (APC40_pot_ID)e.Message.Data1;
                        _VALUE = e.Message.Data2;

                        if (APC40_Pot_Input != null)
                        {
                            APC40_Pot_Input(this, new APC40InputPotEventArgs {PotID = _POTID, value = _VALUE });
                        }
                    }
                }
            }
            else
            {
                if((e.Message.Data1 <= 52 && e.Message.Data1 >= 48) || e.Message.Data1 == 66) //Si Bouton groupé
                {

                    APC40_GroupedButton_ID _GROUPEDBUTTONID;
                    int _CHANNEL;
                    bool _ISON;

                    _GROUPEDBUTTONID = (APC40_GroupedButton_ID)e.Message.Data1;
                    _CHANNEL = e.Message.MidiChannel;
                    
                    if(e.Message.Command == ChannelCommand.NoteOn)
                    {
                        _ISON = true;
                    }
                    else
                    {
                        _ISON = false;
                    }

                    if (APC40_GroupedButton_Input != null)
                    {
                        APC40_GroupedButton_Input(this, new APC40InputGroupedButtonEventArgs { GroupedButtonID = _GROUPEDBUTTONID, channel = _CHANNEL, isOn = _ISON });
                    }
                }
                else //Si Boutton
                {
                    APC40_Button_ID _BUTTONID;
                    bool _ISON;

                    _BUTTONID = (APC40_Button_ID)e.Message.Data1;

                    if (e.Message.Command == ChannelCommand.NoteOn)
                    {
                        _ISON = true;
                    }
                    else
                    {
                        _ISON = false;
                    }

                    if (APC40_Button_Input != null)
                    {
                        APC40_Button_Input(this, new APC40InputButtonEventArgs { ButtonID = _BUTTONID, isOn = _ISON });
                    }

                    _virtualMidiPort.sendCommand(e.Message.GetBytes());

                    #region Mapping
                    if (Mapping != null)
                    {
                        if (((int)_BUTTONID >= 0 && (int)_BUTTONID <= 39) || ((int)_BUTTONID >= 82 && (int)_BUTTONID <= 86))//si Boutton avec Led RGB
                        {
                            RGBButton button = Mapping.RGBBT.Find(x => x.ID == (int)_BUTTONID);//récuperation du mapping du bouton

                            if (_ISON) // si appui
                            {
                                 if(button.Type == buttonType.Momentary) //si Bouton Momentanné
                                {
                                    Led((RGB_Led)_BUTTONID, button.onFlashingtype, button.onprimaryColor, button.onsecondaryColor, button.onFlashingspeed);

                                    if (button.Groupe != -1)// si bouton groupé
                                    {
                                        foreach (RGBButton bt in Mapping.RGBBT) //on éteint tout les autres boutons du même groupe
                                        {

                                            if (bt.Groupe == button.Groupe && bt.ID != (int)_BUTTONID)
                                            {
                                                RGBButton samegroupebutton = Mapping.RGBBT.Find(x => x.ID == bt.ID); //on recupère le mapping du bouton du meme groupe

                                                Led((RGB_Led)bt.ID, samegroupebutton.offFlashingtype, samegroupebutton.offprimaryColor, samegroupebutton.offsecondaryColor, samegroupebutton.offFlashingspeed);
                                                samegroupebutton.IsOnToogle = false;
                                            }
                                        }
                                    }
                                }

                                if (button.Type == buttonType.Toogle) //si Bouton Toogle
                                {
                                    if (button.IsOnToogle == true)
                                    {
                                        Led((RGB_Led)_BUTTONID, button.offFlashingtype, button.offprimaryColor, button.offsecondaryColor, button.offFlashingspeed);
                                        button.IsOnToogle = false;
                                    }
                                    else
                                    {
                                        Led((RGB_Led)_BUTTONID, button.onFlashingtype, button.onprimaryColor, button.onsecondaryColor, button.onFlashingspeed);
                                        button.IsOnToogle = true;

                                        if(button.Groupe != -1) // si bouton groupé
                                        {
                                            foreach (RGBButton bt in Mapping.RGBBT) //on éteint tout les autres boutons du même groupe
                                            {
                                                if(bt.Groupe == button.Groupe && bt.ID != (int)_BUTTONID)
                                                {
                                                    RGBButton samegroupebutton = Mapping.RGBBT.Find(x => x.ID == bt.ID); //on recupère le mapping du bouton du meme groupe

                                                    Led((RGB_Led)bt.ID, samegroupebutton.offFlashingtype, samegroupebutton.offprimaryColor, samegroupebutton.offsecondaryColor, samegroupebutton.offFlashingspeed);
                                                    samegroupebutton.IsOnToogle = false;
                                                }
                                            }
                                        }
                                    }
                                    
                                }
                            }
                            else //si relachement 
                            {
                                if(button.Type == buttonType.Momentary) //si Bouton Momentanné
                                {
                                    Led((RGB_Led)_BUTTONID, button.offFlashingtype, button.offprimaryColor, button.offsecondaryColor, button.offFlashingspeed);
                                }
                            }
                        }
                    }
                    #endregion

                }
            }

            _InputAPC40.StartRecording();
        }

        /// <summary>
        /// APC40 est connecté ?
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            Log.writeLine("APC40 connecté : " + Convert.ToString(connected));
            return connected;
        }


        #region LED

        /// <summary>
        /// Gestion  des Led RGB
        /// </summary>
        /// <param name="LedID"></param>
        /// <param name="type"></param>
        /// <param name="primarycolor"></param>
        /// <param name="secondarycolor"></param>
        /// <param name="speed"></param>
        /// <exception cref="ErrorSendingDataToAPC40Exception"
        public void Led(RGB_Led LedID, BlinkingType type, int primarycolor, int secondarycolor, BlinkingSpeed speed)
        {
            try
            {
                Log.writeLine("Changement Leds -> ID : " + LedID);

                _OuputAPC40.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, (int)LedID, primarycolor));

                _OuputAPC40.Send(new ChannelMessage(ChannelCommand.NoteOn, (int)type * (int)speed, (int)LedID, secondarycolor));
            }
            catch
            {
                Log.writeLine("Impossible de communiquer avec l'APC40");
                throw new ErrorSendingDataToAPC40Exception();
            }
        }

        /// <summary>
        /// Gestion des Leds Orange
        /// </summary>
        /// <param name="LedID"></param>
        /// <param name="IsOn"></param>
        /// <exception cref="ErrorSendingDataToAPC40Exception"
        public void Led(Button_One_Color_Led LedID, bool IsOn)
        {
            try
            {
                Log.writeLine("Changement Leds -> ID : " + LedID);

                _OuputAPC40.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, (int)LedID, Convert.ToInt32(IsOn)));
            }
            catch
            {
                Log.writeLine("Impossible de communiquer avec l'APC40");
                throw new ErrorSendingDataToAPC40Exception();
            }
        }

        /// <summary>
        /// Gestion des Leds Orange groupé
        /// </summary>
        /// <param name="LedID"></param>
        /// <param name="LedChannel"></param>
        /// <param name="IsOn"></param>
        /// <exception cref="ErrorSendingDataToAPC40Exception"
        public void Led(Button_One_Color_Led_Grouped LedID, int LedChannel, bool IsOn)
        {
            try
            {
                Log.writeLine("Changement Leds -> ID : " + LedID + " Channel : " + LedChannel);
                _OuputAPC40.Send(new ChannelMessage(ChannelCommand.NoteOn, LedChannel, (int)LedID, Convert.ToInt32(IsOn)));
            }
            catch
            {
                Log.writeLine("Impossible de communiquer avec l'APC40");
                throw new ErrorSendingDataToAPC40Exception();
            }
        }

        /// <summary>
        /// gestion des Leds BiColor Groupés
        /// </summary>
        /// <param name="LedID"></param>
        /// <param name="LedChannel"></param>
        /// <param name="color"></param>
        /// <exception cref="ErrorSendingDataToAPC40Exception"
        public void Led(Button_dual_Color_led_Grouped LedID, int LedChannel, Dual_Color_Color color)
        {
            try
            {
                Log.writeLine("Changement Leds -> ID : " + LedID + " Channel : " + LedChannel);
                _OuputAPC40.Send(new ChannelMessage(ChannelCommand.NoteOn, LedChannel, (int)LedID, Convert.ToInt32(color)));
            }
            catch
            {
                Log.writeLine("Impossible de communiquer avec l'APC40");
                throw new ErrorSendingDataToAPC40Exception();
            }
        }
        #endregion

        /// <summary>
        /// Configuration des Leds des Potentiomètres
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ErrorSendingDataToAPC40Exception"
        public void Pot_Led_Conf(Pot_Led_Conf_ID ID, Pot_Led_Conf_Type configuration)
        {
            try
            {
                Log.writeLine("Changement Config Potar -> ID : " + ID + " Type : " + configuration);
                _OuputAPC40.Send(new ChannelMessage(ChannelCommand.Controller, 0, (int)ID, (int)configuration));
            }
            catch
            {
                Log.writeLine("Impossible de communiquer avec l'APC40");
                throw new ErrorSendingDataToAPC40Exception();
            }
            
        }

        /// <summary>
        /// Reset de toute les Leds
        /// </summary>
        public void resetLed()
        {
            Log.writeLine("Reset des Leds");

            for (int i = 0; i < 40; i++)
            {
                Led((RGB_Led)i, BlinkingType.OneShot, 0, 0, BlinkingSpeed._1_24);
            }

            Led(Button_One_Color_Led.Pan, false);
            Led(Button_One_Color_Led.Sends, false);
            Led(Button_One_Color_Led.Metronome, false);
            Led(Button_One_Color_Led.User, false);
            Led(Button_One_Color_Led.Play, false);
            Led(Button_One_Color_Led.Record, false);
            Led(Button_One_Color_Led.Session, false);
            Led(Button_One_Color_Led.Device_Left, false);
            Led(Button_One_Color_Led.Device_Right, false);
            Led(Button_One_Color_Led.Bank_Left, false);
            Led(Button_One_Color_Led.Bank_Right, false);
            Led(Button_One_Color_Led.Dev_On_Off, false);
            Led(Button_One_Color_Led.Dev_Lock, false);
            Led(Button_One_Color_Led.Clip_Dev_View, false);
            Led(Button_One_Color_Led.Detail_View, false);
            Led(Button_One_Color_Led.Bank, false);
            Led(Button_One_Color_Led.Master_Button, false);

            for (int i = 0; i < 8; i++)
            {
                Led(Button_One_Color_Led_Grouped.Channel_Select, i, false);
                Led(Button_One_Color_Led_Grouped.Clip_Stop, i, false);
                Led(Button_One_Color_Led_Grouped.One, i, false);
                Led(Button_One_Color_Led_Grouped.REC, i, false);
                Led(Button_One_Color_Led_Grouped.Single, i, false);
                Led(Button_dual_Color_led_Grouped.A_B, i, Dual_Color_Color.off);
            }
        }

        /// <summary>
        /// Animation de démarrage
        /// </summary>
        public void AnimatedStartAnimation()
        {
            Log.writeLine("Animation des Leds");
            int Time = 10;
            Random rand = new Random();

            for (int i = 0; i < 40; i++)
            {
                Led((RGB_Led)i, BlinkingType.OneShot, rand.Next(0, 127), rand.Next(0, 127), BlinkingSpeed._1_2);
                System.Threading.Thread.Sleep(Time*10);
            }

            for (int i = 82; i < 87; i++)
            {
                Led((RGB_Led)i, BlinkingType.OneShot, rand.Next(0, 127), rand.Next(0, 127), BlinkingSpeed._1_2);
                System.Threading.Thread.Sleep(Time * 10);
            }

            System.Threading.Thread.Sleep(Time);

            Led(Button_One_Color_Led.Pan, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Sends, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Metronome, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.User, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Play, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Record, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Session, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Device_Left, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Device_Right, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Bank_Left, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Bank_Right, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Dev_On_Off, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Dev_Lock, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Clip_Dev_View, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Detail_View, true);
            System.Threading.Thread.Sleep(Time);
            Led(Button_One_Color_Led.Bank, true);

            System.Threading.Thread.Sleep(Time);

            for (int i = 0; i < 8; i++)
            {
                Led(Button_One_Color_Led_Grouped.Channel_Select, i, true);
                Led(Button_One_Color_Led_Grouped.Clip_Stop, i, true);
                Led(Button_One_Color_Led_Grouped.One, i, true);
                Led(Button_One_Color_Led_Grouped.REC, i, true);
                Led(Button_One_Color_Led_Grouped.Single, i, true);
                Led(Button_dual_Color_led_Grouped.A_B, i, Dual_Color_Color.orange);
                System.Threading.Thread.Sleep(Time);
            }
          
            Led(Button_One_Color_Led.Master_Button, true);
            System.Threading.Thread.Sleep(Time*5  );

            for (int i = 0; i < 8; i++)
            {
                Led(Button_dual_Color_led_Grouped.A_B, i, Dual_Color_Color.yellow);
            }

            System.Threading.Thread.Sleep(Time*10);

            resetLed();
        }

        /// <summary>
        /// Link l4APC 40 Avec un Mapping
        /// </summary>
        /// <param name="map"></param>
        public void LinkMapping(APC40Mapping map)
        {
            Log.writeLine("Link Mapping");

            Mapping = map;

            foreach (RGBButton item in Mapping.RGBBT)
            {
                Led((RGB_Led)item.ID, item.offFlashingtype, item.offprimaryColor, item.offsecondaryColor, item.offFlashingspeed);
            }
        }
    }


    #region Midi Event
    public class APC40InputButtonEventArgs : EventArgs
    {
        public APC40_Button_ID ButtonID { get; set; }
        public bool isOn { get; set; }

    }

    public class APC40InputPotEventArgs : EventArgs
    {
        public APC40_pot_ID PotID { get; set; }
        public int value { get; set; }
    }

    public class APC40InputCodeEventArgs : EventArgs
    {
        public APC40_Code_ID CodeID { get; set; }
        public bool IsClockWise { get; set; }
    }

    public class APC40InputGroupedButtonEventArgs : EventArgs
    {
        public APC40_GroupedButton_ID GroupedButtonID { get; set; }
        public int channel { get; set; }
        public bool isOn { get; set; }
    }

    public class APC40InputGroupedFaderEventArgs : EventArgs
    {
        public int channel { get; set; }
        public int value { get; set; }
    }
    #endregion

    #region Midi Exception
    public class NoAPC40FoundException : Exception
    {
        public NoAPC40FoundException() : base("Aucun interface APC40 trouvée")
        {
        }
    }

    public class ErrorAPC40ConnexionException : Exception
    {
        public ErrorAPC40ConnexionException() : base("Impossible de connecté un controlleur Midi")
        {
        }
    }

    public class AlreadyConnectedAPC40Device : Exception
    {
        public AlreadyConnectedAPC40Device() : base("Ce controlleur Midi est déja connecté")
        {
        }
    }

    public class AlreadyDisconnectedAPC40Device : Exception
    {
        public AlreadyDisconnectedAPC40Device() : base("Ce controlleur Midi est déja déconnecté")
        {
        }
    }

    public class ErrorSendingDataToAPC40Exception : Exception
    {
        public ErrorSendingDataToAPC40Exception() : base("Erreur d'envoi")
        {
        }
    }

    public class ErrorAPC40DisconnectionException : Exception
    {
        public ErrorAPC40DisconnectionException() : base("Impossible de déconnecté un controlleur Midi")
        {
        }
    }
    #endregion
}

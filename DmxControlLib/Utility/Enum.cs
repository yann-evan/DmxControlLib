using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxControlLib.Utility
{
    /// <summary>
    /// Type de bouton (Momentary, Toogle)
    /// </summary>
    public enum buttonType
    {
        Toogle,
        Momentary
    }

    /// <summary>
    /// Couleur des boutons sur le Launchpad MINI
    /// </summary>
    public enum ButtonColor
    {
        Red,
        Green,
        Yellow,
        Ambre,
        None
    }

    /// <summary>
    /// Type de channel pour un jeux DMX
    /// </summary>
    public enum Channeltype
    {
        Pan,
        Tilt,
        Dimmer,
        RGB,
        other
    }

    /// <summary>
    /// Type de clignotement pour les boutons RGB de l'apc40
    /// </summary>
    public enum BlinkingType
    {
        OneShot = 1,
        Pulsing = 2,
        blinking = 3
    }

    /// <summary>
    /// Vitesse de clignotement des bouton RGB pour l'apc40
    /// </summary>
    public enum BlinkingSpeed
    {
        _1_24 = 1,
        _1_16 = 2,
        _1_8 = 3,
        _1_4 = 4,
        _1_2 = 5

    }

    /// <summary>
    /// ID des Codeur sur l'apc40
    /// </summary>
    public enum APC40_Code_ID
    {
        Cue_Level = 47,
        Tempo = 13
    }

    /// <summary>
    /// ID des potar sur l'apc40
    /// </summary>
    public enum APC40_pot_ID
    {
        Top_Pot0 = 48,
        Top_Pot1 = 49,
        Top_Pot2 = 50,
        Top_Pot3 = 51,
        Top_Pot4 = 52,
        Top_Pot5 = 53,
        Top_Pot6 = 54,
        Top_Pot7 = 55,

        Device_control0 = 16,
        Device_control1 = 17,
        Device_control2 = 18,
        Device_control3 = 19,
        Device_control4 = 20,
        Device_control5 = 21,
        Device_control6 = 22,
        Device_control7 = 23,

        Master = 14,
        CrossFader = 15

    }

    /// <summary>
    /// ID des bouton groupé sur l'apc40
    /// </summary>
    public enum APC40_GroupedButton_ID
    {
        Clip_Stop = 52,
        Channel_Select = 51,

        One = 50,
        Single = 49,
        REC = 48,
        A_B = 66
    }

    /// <summary>
    /// ID des boutons sur l'apc40
    /// </summary>
    public enum APC40_Button_ID
    {
        Pad0 = 0,
        Pad1 = 1,
        Pad2 = 2,
        Pad3 = 3,
        Pad4 = 4,
        Pad5 = 5,
        Pad6 = 6,
        Pad7 = 7,
        Pad8 = 8,
        Pad9 = 9,
        Pad10 = 10,
        Pad11 = 11,
        Pad12 = 12,
        Pad13 = 13,
        Pad14 = 14,
        Pad15 = 15,
        Pad16 = 16,
        Pad17 = 17,
        Pad18 = 18,
        Pad19 = 19,
        Pad20 = 20,
        Pad21 = 21,
        Pad22 = 22,
        Pad23 = 23,
        Pad24 = 24,
        Pad25 = 25,
        Pad26 = 26,
        Pad27 = 27,
        Pad28 = 28,
        Pad29 = 29,
        Pad30 = 30,
        Pad31 = 31,
        Pad32 = 32,
        Pad33 = 33,
        Pad34 = 34,
        Pad35 = 35,
        Pad36 = 36,
        Pad37 = 37,
        Pad38 = 38,
        Pad39 = 39,

        Scene_Launch0 = 82,
        Scene_Launch1 = 83,
        Scene_Launch2 = 84,
        Scene_Launch3 = 85,
        Scene_Launch4 = 86,

        Stop_All_Clips = 81,
        Master_Button = 82,

        Pan = 87,
        Play = 91,
        Record = 93,
        Session = 102,
        Sends = 88,
        Metronome = 90,
        Tap_Tempo = 99,
        User = 89,
        Nudge_Less = 100,
        Nudge_More = 101,

        Device_Left = 58,
        Device_Right = 59,
        Bank_Left = 60,
        Bank_Right = 61,
        Dev_On_Off = 62,
        Dev_Lock = 63,
        Clip_Dev_View = 64,
        Detail_View = 65,

        Shift = 98,
        Bank = 103,

        Up = 94,
        Down = 95,
        Left = 97,
        Right = 96
    }

    /// <summary>
    /// ID des Leds RGb sur l'apc40
    /// </summary>
    public enum RGB_Led
    {
        Pad0 = 0,
        Pad1 = 1,
        Pad2 = 2,
        Pad3 = 3,
        Pad4 = 4,
        Pad5 = 5,
        Pad6 = 6,
        Pad7 = 7,
        Pad8 = 8,
        Pad9 = 9,
        Pad10 = 10,
        Pad11 = 11,
        Pad12 = 12,
        Pad13 = 13,
        Pad14 = 14,
        Pad15 = 15,
        Pad16 = 16,
        Pad17 = 17,
        Pad18 = 18,
        Pad19 = 19,
        Pad20 = 20,
        Pad21 = 21,
        Pad22 = 22,
        Pad23 = 23,
        Pad24 = 24,
        Pad25 = 25,
        Pad26 = 26,
        Pad27 = 27,
        Pad28 = 28,
        Pad29 = 29,
        Pad30 = 30,
        Pad31 = 31,
        Pad32 = 32,
        Pad33 = 33,
        Pad34 = 34,
        Pad35 = 35,
        Pad36 = 36,
        Pad37 = 37,
        Pad38 = 38,
        Pad39 = 39,

        Scene_Launch0 = 82,
        Scene_Launch1 = 83,
        Scene_Launch2 = 84,
        Scene_Launch3 = 85,
        Scene_Launch4 = 86,
    }

    /// <summary>
    /// ID des Leds Orange sur l'apc40
    /// </summary>
    public enum Button_One_Color_Led
    {
        Master_Button = 80,

        Pan = 87,
        Play = 91,
        Record = 93,
        Session = 102,
        Sends = 88,
        Metronome = 90,
        User = 89,

        Device_Left = 58,
        Device_Right = 59,
        Bank_Left = 60,
        Bank_Right = 61,
        Dev_On_Off = 62,
        Dev_Lock = 63,
        Clip_Dev_View = 64,
        Detail_View = 65,

        Bank = 103,
    }

    /// <summary>
    /// ID des boutons orange groupé sur l'apc40
    /// </summary>
    public enum Button_One_Color_Led_Grouped
    {
        Clip_Stop = 52,
        Channel_Select = 51,

        One = 50,
        Single = 49,
        REC = 48
    }

    /// <summary>
    /// ID des Leds Bi_Color sur l'apc40
    /// </summary>
    public enum Button_dual_Color_led_Grouped
    {
        A_B = 66
    }

    /// <summary>
    /// Couleurs des Leds Bi-color sur l'apc40
    /// </summary>
    public enum Dual_Color_Color
    {
        off = 0,
        yellow = 1,
        orange = 2,
    }

    /// <summary>
    /// ID des Led de potar
    /// </summary>
    public enum Pot_Led_Conf_ID
    {
        Top_Pot0 = 56,
        Top_Pot1 = 57,
        Top_Pot2 = 58,
        Top_Pot3 = 59,
        Top_Pot4 = 60,
        Top_Pot5 = 61,
        Top_Pot6 = 62,
        Top_Pot7 = 63,

        Device_control0 = 24,
        Device_control1 = 25,
        Device_control2 = 26,
        Device_control3 = 27,
        Device_control4 = 28,
        Device_control5 = 29,
        Device_control6 = 30,
        Device_control7 = 31,
    }

    /// <summary>
    /// Type de configuration pour les leds des potar
    /// </summary>
    public enum Pot_Led_Conf_Type
    {
        Off = 0,
        Single = 1,
        Volume = 2,
        Pan = 3
    }
}

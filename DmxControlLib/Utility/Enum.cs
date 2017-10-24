﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxControlLib.Utility
{
    public enum buttonType
    {
        Toogle,
        Momentary
    }

    public enum ButtonColor
    {
        Red,
        Green,
        Yellow,
        Ambre,
        None
    }

    public enum Channeltype
    {
        Pan,
        Tilt,
        Dimmer,
        RGB,
        other
    }

    #region  Input APC40 MKII
    public enum APC40_Code_ID
    {
        Cue_Level = 47,
        Tempo = 13
    }

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

    public enum APC40_GroupedButton_ID
    {
        Clip_Stop = 52,
        Channel_Select = 51,

        One = 50,
        Single = 49,
        REC = 48,
        A_B = 66
    }

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
    #endregion

    #region Output A MKII
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

    public enum Button_One_Color_Led_Grouped
    {
        Clip_Stop = 52,
        Channel_Select = 51,

        One = 50,
        Single = 49,
        REC = 48
    }

    public enum Button_dual_Color_led_Grouped
    {
        A_B = 66
    }

    public enum Dual_Color_Color
    {
        off = 0,
        yellow = 1,
        orange = 2,
    }
    #endregion
}

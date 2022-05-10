using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOBList
{    
    public static class AOB
    {
        //All these AOBs are for traits points level 19
        public struct MeleeSpd
        {
            public const string name = "Melee Speed";
            public const string aob = "71 3D 92 3F ?? ?? ?? ?? ?? 00 00 00 ?? ?? 05 00";   //base melee speed = 1.142500043
            public const float val = 1.75f;
        }

        public struct MvmtSpdBuff
        {
            public const string name = "Movement Speed Buff";
            public const string aob = "00 00 64 41 ?? ?? ?? ?? ?? 00 00 00 ?? ?? 05 00";   //% movement speed buff = 14.25
            public const float val = 40;
        }

        public struct FireRate
        {
            public const string name = "Fire Rate Buff";
            public const string aob = "00 00 98 41 ?? ?? ?? ?? ?? 00 00 00 ?? ?? 05 00";   //% fire rate buff = 19
            public const float val = 60;
        }

        public struct ReloadBuff
        {
            public const string name = "Reload Speed Buff";
            public const string aob = "00 00 BE 41 ?? ?? ?? ?? ?? 00 00 00 ?? ?? 05 00";   //% reload rate buff = 23.75
            public const float val = 100;
        }

        public struct ConsumeSpd
        {
            public const string name = "Consume Speed Buff";
            public const string aob = "00 00 05 42 ?? ?? ?? ?? ?? 00 00 00 ?? ?? 05 00";   //% consume speed buff = 32.5
            public const float val = 100;
        }


    }
}
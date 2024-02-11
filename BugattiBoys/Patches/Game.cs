using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugattiBoys.Stores.Portals;
using HarmonyLib;

namespace BugattiBoys.Patches
{
    [HarmonyPatch(typeof(Game), nameof(Game.Start))]
    static class Game_Start
    {
        static void Postfix()
        {
            Environment.GameStarted = true;
        }
    }
}

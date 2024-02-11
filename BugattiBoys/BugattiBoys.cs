using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;

namespace BugattiBoys
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Patch)]
    internal class BugattiBoys : BaseUnityPlugin
    {
        public const string PluginGUID = "BugattiBoys.Valheim.BugattiBoys";
        public const string PluginName = "BugattiBoys";
        public const string PluginVersion = "0.0.1";

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            MinimapManager.OnVanillaMapDataLoaded += MinimapManager_OnVanillaMapDataLoaded;

            // Load the AddPortalPinsPlugin

            Patcher.Patch();
        }

        private static void MinimapManager_OnVanillaMapDataLoaded()
        {
            // Ask the server to send us the portals
            var myId = ZDOMan.GetSessionID();
            var myName = Game.instance.GetPlayerProfile().GetName();
            SendToServer.SyncRequest($"{myName} ({myId}) has joined the game");
        }
    }
}
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BugattiBoys.Plugins.AddPortalPins;
using BugattiBoys.Plugins.AddPortalPins.RPC;
using BugattiBoys.Stores.Portals;
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
            Jotunn.Logger.LogInfo("BugattiBoys has landed");

            // Load the AddPortalPinsPlugin

            Log.Info("Awakening mods");
            AwakenMods();

            Jotunn.Logger.LogInfo("Applying patches");
            Patcher.Patch();
        }

        private static void AwakenMods()
        {
            AddPortalPinsPlugin.Register();
        }

        internal static void GameStarted()
        {

        }
    }
}
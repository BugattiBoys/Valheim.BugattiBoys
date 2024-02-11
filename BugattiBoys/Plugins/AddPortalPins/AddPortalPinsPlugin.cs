using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugattiBoys.Stores.Portals;
using HarmonyLib;
using Jotunn.Managers;
using static Mono.Security.X509.X520;

namespace BugattiBoys.Plugins.AddPortalPins
{
    internal static class AddPortalPinsPlugin
    {
        public static void Register()
        {
            Log.Info("Awakening AddPortalPinsPlugin");
            MinimapManager.OnVanillaMapDataLoaded += MinimapManager_OnVanillaMapDataLoaded;
        }

        private static void MinimapManager_OnVanillaMapDataLoaded()
        {
            Log.Info("MiniMapManager.OnVanillaMapDataLoaded");
            // Ask the server to send us the portals
            var myId = ZDOMan.GetSessionID();
            var myName = Game.instance.GetPlayerProfile().GetName();
            Plugins.AddPortalPins.RPC.Client.RequestSync($"{myName} ({myId}) has joined the game");
        }

        public static void UpdateFromPackage(ZPackage pkg)
        {
            var pinOverlay = MinimapManager.Instance.GetMapDrawing("PinOverlay");
            Log.Info("Here, we should update the client's map with all portal pins");
            var count = pkg.ReadInt();

            Log.Info($"Received {count} portals from server");

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var portalPkg = pkg.ReadPackage();
                    var tag = portalPkg.ReadString();
                    var location = portalPkg.ReadVector3();
                    Log.Info($"Client writing {tag} to {location}");
                    var pos = MinimapManager.Instance.WorldToOverlayCoords(location, pinOverlay.TextureSize);
                    if (Minimap.instance.HaveSimilarPin(pos, Minimap.PinType.Icon4, tag, true))
                        return;

                    Minimap.instance.AddPin(pos, Minimap.PinType.Icon4, tag, true, false, 0L);

                    pinOverlay.MainTex.Apply();
                    pinOverlay.FogFilter.Apply();
                    pinOverlay.ForestFilter.Apply();
                    pinOverlay.HeightFilter.Apply();
                }
            }
        }

        public static void ProcessSyncRequest()
        {
            Log.Info("Responding to sync request");
            RPC.Server.ResponseToSyncRequest(Package());
        }

        public static ZPackage Package()
        {
            var portals = ZDOMan.instance.GetPortals();

            var pkg = new ZPackage();
            pkg.Write(portals.Count);

            foreach (var portal in portals)
            {
                pkg.Write(PackagePortal(portal));
            }

            return pkg;
        }

        public static ZPackage PackagePortal(ZDO portal)
        {
                var pkg = new ZPackage();
                pkg.Write(portal.GetString("tag"));
                pkg.Write(portal.GetPosition());
                return pkg;
        }


        public static void Write(List<Portal> portals)
        {
            
        }

        public static void GameStarted()
        {
            Log.Info("Registering RPC handlers");
            Plugins.AddPortalPins.RPC.Manager.Register();
        }
    }
}

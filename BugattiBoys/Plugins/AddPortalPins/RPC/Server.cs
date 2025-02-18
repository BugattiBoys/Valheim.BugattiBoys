﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugattiBoys.Stores.Portals;

namespace BugattiBoys.Plugins.AddPortalPins.RPC
{
    internal static class Server
    {
        public static void SyncRequest(long sender, string reason)
        {
            Log.Info($"Received sync request from `{sender}` because: {reason}");
            AddPortalPinsPlugin.ProcessSyncRequest();
        }

        public static void ResponseToSyncRequest(ZPackage pkg)
        {
            
            if (ZNet.instance.GetConnectedPeers().Count == 0)
            {
                Log.Info("Not sending resync package: nobody is connected");
                // TODO: This is disabled while I figure out why it 
                // decides not to send events when running locally
                // return;
            }

            Log.Info($"Sending all portals to everybody");
            ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, Manager.RPC_RESYNC, pkg);
        }

        internal static void AddOrUpdateRequest(long sender, ZPackage pkg)
        {
            
        }

        internal static void RemoveRequest(long sender, ZDOID portalId)
        {
            if (!Environment.IsServer) {
                return;
            }

            if (!PortalManager.Instance.ContainsId(portalId)) {
                return; 
            }
        }
    }
}

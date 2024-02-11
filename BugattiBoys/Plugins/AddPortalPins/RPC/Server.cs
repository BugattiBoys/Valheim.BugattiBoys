using System;
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
            // XPortal.ProcessSyncRequest(reason);
        }

        internal static void AddOrUpdateRequest(long sender, ZPackage pkg)
        {
            
        }

        internal static void RemoveRequest(long sender, ZDOID portalId)
        {

        }
    }
}

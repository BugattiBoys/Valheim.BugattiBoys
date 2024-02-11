using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jotunn.Managers;

namespace BugattiBoys
{
    internal static class Environment
    {
        internal static bool IsServer
        {
            get
            {
                return ZNet.instance != null && ZNet.instance.IsServer();
            }
        }

        internal static bool IsHeadless
        {
            get
            {
                return GUIManager.IsHeadless();
            }
        }

        internal static bool GameStarted { get; set; } = false;

        internal static bool ShuttingDown
        {
            get
            {
                return Game.instance.m_shuttingDown;
            }
        }

        internal static long ServerPeerId
        {
            get
            {
                return ZRoutedRpc.instance.GetServerPeerID();
            }
        }
    }
}

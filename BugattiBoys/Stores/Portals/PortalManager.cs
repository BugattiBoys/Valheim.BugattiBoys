using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BugattiBoys.Stores.Portals
{
    internal sealed class PortalManager : IDisposable
    {

        private static readonly Lazy<PortalManager> lazy = new Lazy<PortalManager>(() => new PortalManager());
        public static PortalManager Instance { get { return lazy.Value; } }
        private readonly Dictionary<ZDOID, Portal> portals = new Dictionary<ZDOID, Portal>();

        public int Count
        {
            get
            {
                return portals.Count;
            }
        }

        private PortalManager() { }

        public bool ContainsId(ZDOID id)
        {
            return portals.ContainsKey(id);
        }

        public Portal GetKnownPortalById(ZDOID id)
        {
            return portals[id];
        }

        public Portal GetKnownPortalByPreviousId(ZDOID previousId)
        {
            return portals.Where(p => p.Value.PreviousId == previousId).Select(kvp => kvp.Value).FirstOrDefault();
        }

        public List<Portal> GetList()
        {
            return portals.Values.ToList();
        }

        public ZPackage Pack()
        {
            var allPortals = GetList();

            var pkg = new ZPackage();
            pkg.Write(allPortals.Count);

            foreach (var knownPortal in allPortals)
            {
                pkg.Write(knownPortal.Pack());
            }

            return pkg;
        }

        public List<Portal> GetSortedList()
        {
            var list = GetList();
            list.Sort((valueA, valueB) => valueA.Name.CompareTo(valueB.Name));
            return list;
        }



        public Portal AddOrUpdate(Portal portal)
        {
            if (!ContainsId(portal.Id))
            {
                //Log.Debug($"Adding {portal}");
                portals.Add(portal.Id, portal);
            }
            else
            {
                //Log.Debug($"Updating {portal}");
                portals[portal.Id] = portal;
            }

            return portals[portal.Id];
        }

        public bool Remove(ZDOID id)
        {
            return portals.Remove(id);
        }

        public bool Remove(Portal portal)
        {
            return Remove(portal.Id);
        }

        public void UpdateFromZDOList(List<ZDO> zdoList)
        {
            var portalsWithZdos = new List<Portal>();

            // Create a list of all portals
            foreach (var portalZDO in zdoList)
            {
                var knownPortal = new Portal(portalZDO.m_uid)
                {
                    Name = portalZDO.GetString("tag"),
                    Location = portalZDO.GetPosition(),
                };

                portalsWithZdos.Add(knownPortal);
            }

            // Update our known portals
            UpdateFromList(portalsWithZdos);
        }

        public void UpdateFromResyncPackage(ZPackage pkg)
        {
            var count = pkg.ReadInt();

            Log.Debug($"Received {count} portals from server");

            // Unpack all portals 
            var portalsInPackage = new List<Portal>();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var portalPkg = pkg.ReadPackage();
                    var portal = new Portal(portalPkg);
                    portalsInPackage.Add(portal);
                }
            }

            // Update our known portals
            UpdateFromList(portalsInPackage);
        }

        private void UpdateFromList(List<Portal> updatedPortals)
        {
            // First, update the portals we already know, and add new ones
            Log.Debug($"Updating {updatedPortals.Count} portals");
            foreach (var portal in updatedPortals)
            {
                AddOrUpdate(portal);
            }

            // Second, remove Known Portals that didn't appear in the sync package
            var knownPortals = GetList();
            var deletedPortals = knownPortals.Where(p => !updatedPortals.Contains(p));
            Log.Debug($"Removing {deletedPortals.Count()} portals");
            foreach (var portal in deletedPortals)
            {
                Remove(portal);
            }

   
            Log.Info($"Known portals updated");
            ReportAllPortals();
        }

        public void ReportAllPortals()  // "reportalls" hehehe
        {
            if (!portals.Any())
            {
                Log.Debug(" No portals found.");
                return;
            }

            foreach (Portal p in portals.Values)
            {
                Log.Debug($" {p}");
            }
        }

        public void Reset()
        {
            portals.Clear();
        }

        public void Dispose()
        {
            portals?.Clear();
        }

    }
}

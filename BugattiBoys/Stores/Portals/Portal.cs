using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace BugattiBoys.Stores.Portals
{
    internal class Portal
    {
        public ZDOID Id { get; set; }
        public string Name { get; set; }
        public ZDOID PreviousId { get; set; }
        public Vector3 Location { get; set; }

        public Portal(ZDOID id)
        {
            Id = id;
            Name = string.Empty;
            Location = Vector3.zero;
            PreviousId = ZDOID.None;
        }

        public Portal(ZDOID id, Vector3 location) : this(id)
        {
            Location = location;
        }

        public Portal(ZPackage pkg)
        {
            Id = pkg.ReadZDOID();
            Name = pkg.ReadString();
            Location = pkg.ReadVector3();
            PreviousId = pkg.ReadZDOID();
        }

        public string GetFriendlyName()
        {
            var portalName = Name;
            if (string.IsNullOrEmpty(portalName))
            {
                return Localization.instance.Localize("$piece_portal_tag_none");  // "(No Name)"
            }
            else
            {
                return portalName;
            }
        }

        public ZPackage Pack()
        {
            var pkg = new ZPackage();
            pkg.Write(Id);
            pkg.Write(Name);
            pkg.Write(Location);
            pkg.Write(PreviousId);
            return pkg;
        }

        public override string ToString()
        {
            return $"{{ Id: `{Id}`, Name; `{GetFriendlyName()}`, Location: `{Location}`}}";
        }

    }
}

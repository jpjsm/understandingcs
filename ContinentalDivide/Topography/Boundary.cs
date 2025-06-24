using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topography
{
    public enum Boundary
    {
        Self,
        Neighbor,
        DeadEnd,
        NorthLimit,
        SouthLimit,
        EastLimit,
        WestLimit,
        AtlanticOcean = EastLimit,
        PacificOcean = WestLimit
    }
}

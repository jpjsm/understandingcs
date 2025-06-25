using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topography
{
    [Flags]
    public enum VisibilityLimits
    {
        Unknowkn = 0,
        Crater = 1,
        NorthLimit = 2,
        SouthLimit = 4,
        EastLimit = 8,
        WestLimit = 16,
        AtlanticOcean = EastLimit,
        PacificOcean = WestLimit,
        Lake = Crater
    }
}

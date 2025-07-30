using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topography
{
    public class TopoData
    {
        public double Elevation { get; private set; }
        public bool Visited { get; set; }
        public VisibilityLimits Visibility { get; set; } = VisibilityLimits.Unknowkn; 


        public TopoData()
        {
            Elevation = 0.0;
            Visited = false;
        }

        public TopoData(double e)
        {
            Elevation = e;
            Visited = false;
        }
    }
}

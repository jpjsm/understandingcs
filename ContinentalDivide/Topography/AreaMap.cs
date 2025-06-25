using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topography
{
    public class AreaMap
    {
        private TopoData[,] area;
        private List<Point> peaks = new List<Point>();
        public List<Point> ContinentalDivision { get; private set; } = new List<Point>();

        public AreaMap(double[,] elevations)
        {
            if (elevations == null)
            {
                throw new ArgumentNullException("elevations");
            }

            int LengthX = elevations.GetLength((int)DimensionNames.X);
            int LengthY = elevations.GetLength((int)DimensionNames.Y);

            area = new TopoData[LengthX, LengthY];

            for (int x = 0; x < LengthX; x++)
            {
                for (int y = 0; y < LengthY; y++)
                {
                    area[x, y] = new TopoData(elevations[x, y]);
                    peaks.Add(new Point(x, y));
                }
            }

            // Order peaks in descending order
            peaks = peaks.OrderByDescending(p => area[p.X, p.Y].Elevation).ToList();


            foreach (Point peak in peaks)
            {
                if (area[peak.X, peak.Y].Visited)
                {
                    continue;
                }


                VisibilityLimits v = GetPathEnds(peak);
            }


        }

        private VisibilityLimits GetPathEnds(Point peak)
        {
            HashSet<Point> walkedpath = new HashSet<Point>();
            return GetPathEnds(peak, walkedpath);
        }

        private VisibilityLimits GetPathEnds(Point peak, HashSet<Point> walkedpath)
        {
            if (area[peak.X, peak.Y].Visited)
            {
                return area[peak.X, peak.Y].Visibility;
            }

            walkedpath.Add(peak);

            VisibilityLimits visibility = VisibilityLimits.Unknowkn;
            foreach (Tuple<Direction, Boundary> neighbortype in GetNeighborType(peak))
            {
                if (neighbortype.Item2 == Boundary.Neighbor)
                {
                    Point neighbor = GetNeighborFromDirection(peak, neighbortype.Item1);
                    if (!walkedpath.Contains(neighbor) && area[neighbor.X, neighbor.Y].Elevation <= area[peak.X, peak.Y].Elevation)
                    {
                        visibility |= GetPathEnds(neighbor, walkedpath);
                    }
                }
                else
                {
                    switch (neighbortype.Item2)
                    {
                        case Boundary.NorthLimit:
                            visibility |= VisibilityLimits.NorthLimit;
                            break;
                        case Boundary.SouthLimit:
                            visibility |= VisibilityLimits.SouthLimit;
                            break;
                        case Boundary.EastLimit:
                            visibility |= VisibilityLimits.EastLimit;
                            break;
                        case Boundary.WestLimit:
                            visibility |= VisibilityLimits.WestLimit;
                            break;
                        default:
                            break;
                    }
                }

            };

            if (visibility == VisibilityLimits.Unknowkn)
            {
                visibility = VisibilityLimits.Crater;
            }

            area[peak.X, peak.Y].Visited = true;
            area[peak.X, peak.Y].Visibility = visibility;

            if ((visibility & (VisibilityLimits.AtlanticOcean | VisibilityLimits.PacificOcean)) == (VisibilityLimits.AtlanticOcean | VisibilityLimits.PacificOcean))
            {
                ContinentalDivision.Add(peak);
            }

            walkedpath.Remove(peak);

            return visibility;
        }

        private Point GetNeighborFromDirection(Point peak, Direction direction)
        {
            int directionindex = (int)direction;
            int offsetX = (directionindex % 3) - 1;
            int offsetY = (directionindex / 3) - 1;
            return new Point(peak.X + offsetX, peak.Y + offsetY);
        }

        public List<Tuple<Direction, Boundary>> GetNeighborType(Point p)
        {
            List<Tuple<Direction, Boundary>> NeighborsType = new List<Tuple<Direction, Boundary>>();
            int x, y;
            for (int i = -1; i <= 1; i++)
            {
                x = p.X + i;

                for (int j = -1; j <= 1; j++)
                {
                    y = p.Y + j;

                    if (i==0 && j==0) { continue; } // skip self


                    if (y < 0)
                    {
                        NeighborsType.Add(new Tuple<Direction, Boundary>((Direction)(3 * (j + 1) + i + 1), Boundary.SouthLimit));
                    }
                    else if (y >= area.GetLength((int)DimensionNames.Y))
                    {
                        NeighborsType.Add(new Tuple<Direction, Boundary>((Direction)(3 * (j + 1) + i + 1), Boundary.NorthLimit));
                    }
                    else
                    {
                        if (x < 0)
                        {
                            NeighborsType.Add(new Tuple<Direction, Boundary>((Direction)(3 * (j + 1) + i + 1), Boundary.WestLimit));
                        }
                        else if (x >= area.GetLength((int)DimensionNames.X))
                        {
                            NeighborsType.Add(new Tuple<Direction, Boundary>((Direction)(3 * (j + 1) + i + 1), Boundary.EastLimit));
                        }
                        else
                        {
                            NeighborsType.Add(new Tuple<Direction, Boundary>((Direction)(3 * (j + 1) + i + 1), Boundary.Neighbor));
                        }
                    }
                }
            }

            return NeighborsType;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTopNOfVeryLargeM
{
    public class RankableItem : IComparable
    {
        public Guid Id { get; }
        public int X { get; }
        public int Y { get; }

        public  RankableItem(int x, int y)
        {
            X = x;
            Y = y;
            Id = Guid.NewGuid();
        }

        public int CompareTo(object obj)
        {
            RankableItem otherObject = obj as RankableItem;
            if (otherObject == null)
            {
                throw new ArgumentException(string.Format("Object is not {0}", nameof(RankableItem)));
            }

            double thisDistance = Utils.Distance(this.X, this.Y);
            double otherDistance = Utils.Distance(otherObject.X, otherObject.Y);

            if (thisDistance < Utils.Distance(otherObject.X,otherObject.Y))
            {
                return 1;
            }

            if (thisDistance > otherDistance)
            {
                return -1;
            }

            return 0;
        }
    }
}

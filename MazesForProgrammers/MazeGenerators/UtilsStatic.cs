using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerators
{
    public static class UtilsStatic
    {
        private static Random rnd = new Random();
        public static CoinSides CoinFlip()
        {
            return (CoinSides) (rnd.Next() & 0x1) ;
        }
    }
}

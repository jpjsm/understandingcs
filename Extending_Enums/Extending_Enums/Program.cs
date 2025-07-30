using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Original_Enum
{
    public enum Seasons
    {
        Spring,
        Summer,
        Fall,
        Autumm = Fall,
        Winter
    }
}

namespace Extending_Enums
{
    public enum Seasons: Original_Enum.Seasons
    {

    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}

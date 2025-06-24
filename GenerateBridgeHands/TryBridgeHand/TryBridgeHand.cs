using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.TryBridgeHand
{
    using BridgeHand;

    class TryBridgeHand
    {
         
        static void Main(string[] args)
        {
            BridgeHand hands = new BridgeHand();

            List<BridgeHand> openinghands = new List<BridgeHand>(100);

            for (int i = 0; i < 100; i++)
            {
                openinghands.Add(new BridgeHand(10, -1, 14, -1));
            }

            foreach (BridgeHand hand in openinghands)
            {
                //Console.WriteLine(hand.GetHashCode());
                Console.WriteLine(hand.PrintHands());
                Console.WriteLine();
            }
        }
    }
}

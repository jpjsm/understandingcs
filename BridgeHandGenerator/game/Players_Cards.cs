using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeHandGenerator
{
    public readonly record struct Players_Cards(Hand North, Hand East, Hand South, Hand West);
}
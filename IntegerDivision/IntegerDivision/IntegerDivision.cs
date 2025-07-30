using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math
{
    public class IntegerDivision
    {
        public long Quotient { get; set; }
        public long Reminder { get; set; }

        public IntegerDivision(long numerator, long denominator)
        {
            long f, p;
            Reminder = numerator;
            Quotient = 0L;

            while (Reminder >= denominator)
            {
                f = denominator;
                p = 1;
                while (Reminder >= f)
                {
                    f <<= 1;
                    p <<= 1;
                }

                f >>= 1;
                p >>= 1;
                Reminder -= f;
                Quotient += p;
            }
        }
    }

}

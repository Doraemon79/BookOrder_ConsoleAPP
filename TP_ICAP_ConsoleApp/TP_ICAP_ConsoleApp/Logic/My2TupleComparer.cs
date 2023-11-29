using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    internal class My2TupleComparer : IComparer<Tuple<double, TimeSpan, string>>
    {
        public int Compare(Tuple<double, TimeSpan,string> x, Tuple<double, TimeSpan, string> y)
        {
            int cc;
            if (x == null && y == null) cc = 0;
            else if (x == null && y != null) cc = -1;
            else if (x != null && y == null) cc = +1;
            else /* ( x != null && y != null ) */
            {
                 cc = y.Item1.CompareTo(x.Item1);
                //cc = string.Compare(x.Item1, y.Item1, StringComparison.OrdinalIgnoreCase);
                if (cc == 0)
                {
                    cc = x.Item2.CompareTo(y.Item2);
                }
                if (cc==0)
                {
                    cc = x.Item3.CompareTo(y.Item3);
                }
            }
            return cc;
        }
    }
}

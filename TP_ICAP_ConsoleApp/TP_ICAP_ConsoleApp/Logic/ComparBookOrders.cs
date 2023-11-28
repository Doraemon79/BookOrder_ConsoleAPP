using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    internal class ComparBookOrders : IComparer<BookOrder>
    {
        public int Compare(BookOrder bookOrder1, BookOrder bookOrder2)
        {
            //if ((employee1 is null) || (employee2 is null))
            //{
            //    return 0;
            //}

            var priceResult = bookOrder2.Notional.CompareTo(bookOrder1.Notional);

            if (priceResult == 0)
            { priceResult = bookOrder1.OrderDateTime.CompareTo(bookOrder2.OrderDateTime); }
            if (priceResult == 0) { priceResult = bookOrder1.OrderId.CompareTo(bookOrder2.OrderId); }

            return priceResult;
        }

    }
}

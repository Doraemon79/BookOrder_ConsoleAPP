using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Containers
{
    public class FastBookOrdered : IFastBookOrdered
    {
        private Dictionary<string, BookOrder> FastBookOrderedTable;
        private List<BookOrder> InvalidOrders;
        public FastBookOrdered() { }

        public void InvalidOrderListCreator()
        {
            InvalidOrders= new List<BookOrder>();
        }
        public void FastBookOrderCreator()
        {
            FastBookOrderedTable = new Dictionary<string, BookOrder>();
        }

        public Dictionary<string, BookOrder> BookOrder()
        {
            return FastBookOrderedTable;
        }

        public List<BookOrder> InvalidOrdersList() { return InvalidOrders; }

        public void BookOrderFiller(List<BookOrder> bookOrders)
        {
            var tempOrder = new BookOrder();
            //foreach (var t in bookOrders)
            for (int i=0;i<=bookOrders.Count-1;i++)
            {
                if (bookOrders[i].Matches==null)
                { 
                    tempOrder = bookOrders[i];
                    tempOrder.Matches = new List<Match>();

                }
                
                var isDuplicate = FastBookOrderedTable.TryAdd(tempOrder.OrderId, tempOrder);
                if (!isDuplicate)
                {
                    InvalidOrders.Add(tempOrder);
                }

            }
        }

        public void OrderUpdate(BookOrder order)
        {
            FastBookOrderedTable[order.OrderId] = order;
        }

    }
}

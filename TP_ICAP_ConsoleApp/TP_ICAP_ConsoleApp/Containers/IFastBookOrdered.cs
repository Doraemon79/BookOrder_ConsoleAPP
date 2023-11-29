using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Containers
{
    public interface IFastBookOrdered
    {
        Dictionary<string, BookOrder> BookOrder();

        void FastBookOrderCreator();
        List<BookOrder> InvalidOrdersList();
        void BookOrderFiller(List<BookOrder> bookOrders);

        void OrderUpdate(BookOrder order);

        void InvalidOrderListCreator();


    }
}

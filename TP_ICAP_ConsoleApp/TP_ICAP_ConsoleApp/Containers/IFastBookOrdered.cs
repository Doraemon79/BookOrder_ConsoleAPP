using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Containers
{
    public interface IFastBookOrdered
    {
        Dictionary<string, BookOrder> BookOrder();

        void FastBookOrderCreator();
        List<BookOrder> InvalidOrdersList();
        void BookOrderFiller(List<BookOrder> bookOrders, Dictionary<string, BookOrder> FastBookOrderedTable, List<BookOrder> InvalidOrders);

        void OrderUpdate(BookOrder order);

        void InvalidOrderListCreator();

        void OrderToBookProcessor(List<BookOrder> bookOrders, Dictionary<string, BookOrder> FastBookOrderedTable, List<BookOrder> InvalidOrders);
        BookOrder GetOrder(string orderId);

    }
}

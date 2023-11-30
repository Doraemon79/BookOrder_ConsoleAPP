using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Containers
{
    public class FastBookOrdered : IFastBookOrdered
    {
        private Dictionary<string, BookOrder>? FastBookOrderedTable;
        private List<BookOrder>? InvalidOrders;
        public FastBookOrdered() { }

        public void InvalidOrderListCreator()
        {
            InvalidOrders = new List<BookOrder>();
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

        private bool OrdersChecker(BookOrder order)
        {
            bool result = true;
                if (string.IsNullOrWhiteSpace(order.OrderId) || string.IsNullOrWhiteSpace(order.Company)
                    || order.Volume == null || order.Volume <=0 || order.Notional < 0)
                {
                result = false;
                    Console.WriteLine($" {order.OrderId} - The order is not correctly inserted it will be skipped. ");
                }
                else if (order.Volume == 0 || order.MatchState == "FullMatch" || order.MatchState == "InvalidMatch")
                {
                result = false;
                Console.WriteLine($" {order.OrderId} - The order cannot be processed due to its state.It will be skipped. ");
                }

            return result;
            }

        public void BookOrderFiller(List<BookOrder> bookOrders, Dictionary<string, BookOrder> FastBookOrderedTable, List<BookOrder> InvalidOrders)
        {
            var tempOrder = new BookOrder();
            for (int i = 0; i <= bookOrders.Count - 1; i++)
            {
                //if( string.IsNullOrWhiteSpace(bookOrders[i].OrderId) || string.IsNullOrWhiteSpace(bookOrders[i].Company) 
                //    || bookOrders[i].Volume == null)
                //{
                //    Console.WriteLine($" {bookOrders[i].OrderId} - The order is not correctly inserted it will be skipped. ");
                //    continue;
                //}else if (bookOrders[i].Volume ==  0 || bookOrders[i].MatchState=="FullMatch" || bookOrders[i].MatchState == "InvalidMatch")
                //    {
                //    Console.WriteLine($" {bookOrders[i].OrderId} - The order cannot be processed due to its state.It will be skipped. ");
                //    continue;
                //}

               //if( OrderChecker(bookOrders[i])

                 if (OrdersChecker(bookOrders[i]) && bookOrders[i].Matches == null)
                {
                    tempOrder = bookOrders[i];
                    tempOrder.Matches = new List<Match>();
                }

                var isUnique = FastBookOrderedTable.TryAdd(tempOrder.OrderId, tempOrder);
                if (!isUnique)
                {
                    FastBookOrderedTable.TryGetValue(tempOrder.OrderId, out BookOrder order  );
                    if (order.OrderDateTime> tempOrder.OrderDateTime)
                    {
                        FastBookOrderedTable.Remove(tempOrder.OrderId);
                        FastBookOrderedTable.TryAdd(tempOrder.OrderId, tempOrder);
                        tempOrder = order;
                    }
                    else
                    { 
                        tempOrder = bookOrders[i];
                    }
                    tempOrder.MatchState = "InvalidMatch";
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

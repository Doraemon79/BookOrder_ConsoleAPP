using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

        public BookOrder GetOrder(string orderId)
        {
            FastBookOrderedTable.TryGetValue(orderId, out BookOrder value);
            return value;
        }

        public List<BookOrder> InvalidOrdersList() { return InvalidOrders; }

        private bool OrdersChecker(BookOrder order)
        {
            bool result = true;
            if (string.IsNullOrWhiteSpace(order.OrderId) || string.IsNullOrWhiteSpace(order.Company)
                 || order.Volume <= 0 || order.Notional < 0)
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
                    FastBookOrderedTable.TryGetValue(tempOrder.OrderId, out BookOrder order);
                    if (order.OrderDateTime > tempOrder.OrderDateTime)
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

        public void OrderToBookProcessor(List<BookOrder> bookOrders, Dictionary<string, BookOrder> FastBookOrderedTable, List<BookOrder> InvalidOrders)
        {
            var tempOrder = new BookOrder();
            for (int i = 0; i <= bookOrders.Count - 1; i++)
            {
                if (OrdersChecker(bookOrders[i]) && bookOrders[i].Matches == null)
                {
                    tempOrder = bookOrders[i];
                    tempOrder.Matches = new List<Match>();
                }

                //var invalidOrder= AddToFastTable(FastBookOrderedTable, bookOrders[i], tempOrder) ;
                //if (invalidOrder.MatchState == "InvalidMatch")
                //{
                //    InvalidOrders.Add(tempOrder);
                //}


                ref var valOrNull = ref CollectionsMarshal.GetValueRefOrNullRef(FastBookOrderedTable, tempOrder.OrderId);
                if (!Unsafe.IsNullRef(ref valOrNull))
                {
                    var invalidOrder = new BookOrder();
                    if (FastBookOrderedTable[tempOrder.OrderId].OrderDateTime > tempOrder.OrderDateTime)
                    {
                        invalidOrder = FastBookOrderedTable[tempOrder.OrderId];
                        valOrNull = tempOrder;

                    }
                    else
                    {
                        invalidOrder = tempOrder;
                    }
                    invalidOrder.MatchState = "InvalidMatch";
                    InvalidOrders.Add(tempOrder);
                }
                else { FastBookOrderedTable[tempOrder.OrderId] = tempOrder; }
            }

            //if (!AddToFastTable(FastBookOrderedTable, bookOrders[i], tempOrder))
            //{
            //    if (bookOrders[i].OrderDateTime > tempOrder.OrderDateTime)
            //    {
            //        var invalidOrder = bookOrders[i];
            //        FastBookOrderedTable[bookOrders[i].OrderId] = tempOrder;
            //        tempOrder = invalidOrder;

            //    }

            //    InvalidOrders.Add(tempOrder);
            //}
        }


        public BookOrder AddToFastTable(Dictionary<string, BookOrder> FastBookOrderedTable, BookOrder CurrentOrder, BookOrder tempOrder)
        {
            BookOrder invalidOrder = new BookOrder();
            ref var valOrNull = ref CollectionsMarshal.GetValueRefOrNullRef(FastBookOrderedTable, tempOrder.OrderId);
            if (!Unsafe.IsNullRef(ref valOrNull))
            {
                if (CurrentOrder.OrderDateTime > tempOrder.OrderDateTime)
                {
                    invalidOrder = CurrentOrder;
                    valOrNull.OrderId = tempOrder.OrderId;

                }
                else
                {
                    invalidOrder = tempOrder;

                }
                invalidOrder.MatchState = "InvalidMatch";
            }

            return invalidOrder;
        }

        public void OrderUpdate(Dictionary<string, BookOrder> FastBookOrderedTable, BookOrder order)
        {
            ref var valOrNull = ref CollectionsMarshal.GetValueRefOrNullRef(FastBookOrderedTable, order.OrderId);
            if (!Unsafe.IsNullRef(ref valOrNull))
            {
                valOrNull.OrderId = order.OrderId;
            }

        }

        public void OrderUpdate(BookOrder order)
        {
            FastBookOrderedTable[order.OrderId] = order;
        }

    }
}

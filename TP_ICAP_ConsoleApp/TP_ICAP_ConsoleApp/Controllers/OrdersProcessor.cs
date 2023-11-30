using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Helpers;
using TP_ICAP_ConsoleApp.Logic;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Controllers
{
    public class OrdersProcessor : IOrdersProcessor
    {
        private readonly IFastBookOrdered _fastBookOrdered;
        private readonly ISellOrders _sellOrders;
        private readonly IPriceTimePriorityAlgorithm _priceTimePriorityAlgorithms;
        private readonly IProRataAlgorithm _proRataAlgorithm;
        private readonly IWriteOutput _writeOutput;

        public OrdersProcessor(IFastBookOrdered fastBookOrdered, ISellOrders sellOrders, IPriceTimePriorityAlgorithm matchAlgorithms, IProRataAlgorithm proRataAlgorithm, IWriteOutput writeOutput)
        {
            _fastBookOrdered = fastBookOrdered;
            _sellOrders = sellOrders;
            _priceTimePriorityAlgorithms = matchAlgorithms;
            _proRataAlgorithm = proRataAlgorithm;
            _writeOutput = writeOutput;
        }

        private Queue<BookOrder> BuyQueue(List<BookOrder> inputBookOrder)
        {
            Queue<BookOrder> BuyQueue = new Queue<BookOrder>();

            var BuyOrders = inputBookOrder.OrderBy(x => x.OrderDateTime).ThenByDescending(x => x.Notional).Where(x => x.OrderType.Equals("Buy")).ToList();

            foreach (var t in inputBookOrder)
            {
                BuyQueue.Enqueue(t);
            }

            return BuyQueue;
        }

        public void ProcessBookOrder(List<BookOrder> inputBookOrder, string inputAlgo)
        {
            /////////////Factory

            MatcherFactory factory = new ConcreteMatcherFactory();

            //IMatchingPatterns priceTimePriority=factory.GetAlgorithm(inputAlgo);

            ///////////
            //creat quick container
            _fastBookOrdered.FastBookOrderCreator();
            _fastBookOrdered.InvalidOrderListCreator();
            _fastBookOrdered.OrderToBookProcessor(inputBookOrder, _fastBookOrdered.BookOrder(), _fastBookOrdered.InvalidOrdersList());

            IMatchingPatterns priceTimePriority = factory.GetAlgorithm(inputAlgo, _fastBookOrdered, _sellOrders, _priceTimePriorityAlgorithms, _proRataAlgorithm, _writeOutput);
            priceTimePriority.Calculate(inputBookOrder);

            Console.ReadLine();

            //List<BookOrder> InvalidOrders = _fastBookOrdered.InvalidOrdersList();



            ////divide into 2 list
            //Queue<BookOrder> QueueForBuy = new Queue<BookOrder>();
            //List<KeyValuePair<string, BookOrder>> tempBuy = new List<KeyValuePair<string, BookOrder>>();
            //int totalVolume = 0;

            //if (inputAlgo == "Pro-rata")
            //{
            //    totalVolume = _fastBookOrdered.BookOrder().Sum(x => x.Value.Volume);
            //    tempBuy = _fastBookOrdered.BookOrder().OrderBy(x => x.Value.Volume / _fastBookOrdered.BookOrder().Sum(i => i.Value.Volume)).ToList();
            //}
            //else
            //{


            //    tempBuy = _fastBookOrdered.BookOrder().OrderBy(x => x.Value.OrderDateTime).ThenByDescending(x => x.Value.Notional).Where(x => x.Value.OrderType.Equals("Buy")).ToList();
            //}

            //foreach (var t in tempBuy)
            //{
            //    QueueForBuy.Enqueue(t.Value);
            //}

            //var SellOrdersList = _fastBookOrdered.BookOrder().OrderBy(x => x.Value.OrderDateTime).ThenByDescending(x => x.Value.Notional).Where(x => x.Value.OrderType.Equals("Sell")).ToList();
            //_sellOrders.SellsQuickListMaker(SellOrdersList);

            //while (QueueForBuy.Any())
            //{
            //    BookOrder order = QueueForBuy.Dequeue();
            //    BookOrder bid = new BookOrder();
            //    if (inputAlgo == "Pro-rata")
            //    {

            //        bid = _proRataAlgorithm.ProRataMatcherForBuy(order, totalVolume);
            //    }
            //    else
            //    {

            //        bid = _priceTimePriorityAlgorithms.PriceTimePriority(order);
            //    }

            //    _fastBookOrdered.OrderUpdate(bid);
            //}

            ////process I/O
            //WriteConsoleOutput(_fastBookOrdered.InvalidOrdersList());
            //WriteConsoleOutput(_fastBookOrdered.BookOrder());
        }

        //public void WriteConsoleOutput(List<BookOrder> invalidOrders)
        //{
        //    foreach (var invalid in invalidOrders)
        //    {
        //        var state = invalid.MatchState;
        //        Console.WriteLine(state switch
        //        {
        //            "InvalidMatch" => $" {invalid.OrderId} - InvalidMatch a previous order with same Id already existed. ",
        //            "WrongEntry" => $" {invalid.OrderId} - The order is not correctly inserted. ",
        //            _ => $" {invalid.OrderId}-The order is Invalid. "
        //        });
        //    }
        //}

        //public void WriteConsoleOutput(Dictionary<string, BookOrder> fastBookOrdertable)
        //{

        //    foreach (var order in fastBookOrdertable)
        //    {
        //        Console.WriteLine($" {order.Key} - {order.Value.MatchState}");
        //        if (order.Value.Matches != null && order.Value.Matches.Count > 0)
        //        {
        //            foreach (var m in order.Value.Matches)
        //            {
        //                Console.WriteLine($"          +{m.OrderId}  -  {m.Volume}");
        //            }
        //        }
        //    }
        //}
    }
}

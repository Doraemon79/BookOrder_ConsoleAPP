using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Helpers;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public class PriceTimePriorityMatcher : IMatchingPatterns
    {
        private readonly IFastBookOrdered _fastBookOrdered;
        private readonly ISellOrders _sellOrders;
        private readonly IPriceTimePriorityAlgorithm _priceTimePriorityAlgorithms;
        private readonly IWriteOutput _writeOutput;
        public PriceTimePriorityMatcher(IFastBookOrdered fastBookOrdered, ISellOrders sellOrders, IPriceTimePriorityAlgorithm matchAlgorithms, IWriteOutput writeOutput)
        {
            _fastBookOrdered = fastBookOrdered;
            _sellOrders = sellOrders;
            _priceTimePriorityAlgorithms = matchAlgorithms;
            _writeOutput = writeOutput;
        }

        public void Calculate(List<BookOrder> inputBookOrder)
        {
            Queue<BookOrder> QueueForBuy = new Queue<BookOrder>();
            List<KeyValuePair<string, BookOrder>> tempBuy = new List<KeyValuePair<string, BookOrder>>();

            tempBuy = _fastBookOrdered.BookOrder().OrderBy(x => x.Value.OrderDateTime).ThenByDescending(x => x.Value.Notional).Where(x => x.Value.OrderType.Equals("Buy")).ToList();
            foreach (var t in tempBuy)
            {
                QueueForBuy.Enqueue(t.Value);
            }
            var SellOrdersList = _fastBookOrdered.BookOrder().OrderBy(x => x.Value.OrderDateTime).ThenByDescending(x => x.Value.Notional).Where(x => x.Value.OrderType.Equals("Sell")).ToList();
            _sellOrders.SellsQuickListMaker(SellOrdersList);
            while (QueueForBuy.Any())
            {
                BookOrder order = QueueForBuy.Dequeue();
                BookOrder bid = new BookOrder();
                {
                    bid = _priceTimePriorityAlgorithms.PriceTimePriority(order);
                }
                _fastBookOrdered.OrderUpdate(bid);
            }

            _writeOutput.WriteConsoleOutput(_fastBookOrdered.InvalidOrdersList());
            _writeOutput.WriteConsoleOutput(_fastBookOrdered.BookOrder());
        }
    }
}

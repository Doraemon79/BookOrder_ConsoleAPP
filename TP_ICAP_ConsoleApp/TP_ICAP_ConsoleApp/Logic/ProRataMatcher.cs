using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Helpers;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public class ProRataMatcher : IMatchingPatterns
    {
        private readonly IFastBookOrdered _fastBookOrdered;
        private readonly ISellOrders _sellOrders;
        private readonly IProRataAlgorithm _proRataAlgorithm;
        private readonly IWriteOutput _writeOutput;

        public ProRataMatcher(IFastBookOrdered fastBookOrdered, ISellOrders sellOrders, IProRataAlgorithm matchAlgorithms, IWriteOutput writeOutput)
        {
            _fastBookOrdered = fastBookOrdered;
            _sellOrders = sellOrders;
            _proRataAlgorithm = matchAlgorithms;
            _writeOutput = writeOutput;
        }

        public void Calculate(List<BookOrder> inputBookOrder)
        {
            Queue<BookOrder> QueueForBuy = new Queue<BookOrder>();
            List<KeyValuePair<string, BookOrder>> tempBuy = new List<KeyValuePair<string, BookOrder>>();
            int totalVolume = 0;

            totalVolume = _fastBookOrdered.BookOrder().Where(x => x.Value.OrderType.Equals("Buy")).Sum(x => x.Value.Volume);
            tempBuy = _fastBookOrdered.BookOrder().Where(x => x.Value.OrderType.Equals("Buy")).OrderByDescending(x => x.Value.Volume / _fastBookOrdered.BookOrder().Sum(i => i.Value.Volume)).ToList();

            foreach (var t in tempBuy)
            {
                QueueForBuy.Enqueue(t.Value);
            }
            var SellOrdersList = _fastBookOrdered.BookOrder().OrderBy(x => x.Value.OrderDateTime).ThenByDescending(x => x.Value.Notional).Where(x => x.Value.OrderType.Equals("Sell")).ToList();
            _sellOrders.SellsQuickListMaker(SellOrdersList);
            var totalSaleVolume = _sellOrders.SellsOrderQuickList().Sum(x => x.Value.Volume);
            while (QueueForBuy.Any())
            {
                BookOrder order = QueueForBuy.Dequeue();
                BookOrder bid = new BookOrder();
                {
                    bid = _proRataAlgorithm.ProRataMatcherForBuy(order, totalVolume, totalSaleVolume);
                }
                _fastBookOrdered.OrderUpdate(bid);
            }

            _writeOutput.WriteConsoleOutput(_fastBookOrdered.InvalidOrdersList());
            _writeOutput.WriteConsoleOutput(_fastBookOrdered.BookOrder());
        }
    }
}

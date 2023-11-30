using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public class ProRataAlgorithm : IProRataAlgorithm
    {
        private readonly IFastBookOrdered _fastBookOrdered;
        private readonly ISellOrders _sellOrders;
        public ProRataAlgorithm(IFastBookOrdered fastBookOrdered, ISellOrders sellOrders)
        {
            _fastBookOrdered = fastBookOrdered;
            _sellOrders = sellOrders;
        }

        public BookOrder ProRataMatcherForBuy(BookOrder bid, int totalVolume, int totalSaleVolume)
        {
            float ratio = ((float)bid.Volume / (float)totalVolume);
            if (bid.Matches == null)
            {
                bid.Matches = new List<Match>();
            }
            var tst = _fastBookOrdered.BookOrder();
            foreach (var s in _sellOrders.SellsOrderQuickList())
            {
                var sale = _fastBookOrdered.GetOrder(s.Value.OrderId);

                BookOrder tempSaleOrder = sale;
                if (tempSaleOrder.Matches == null)
                {
                    tempSaleOrder.Matches = new List<Match>();
                }
                int sales = (int)Math.Floor(ratio * sale.Volume);
                var finalVolume = bid.Volume - sales;

                switch (finalVolume)
                {
                    case int n when n > 0:
                        bid.Volume = finalVolume;
                        bid.MatchState = "PartialMatch";
                        bid.Matches.Add(new Match { OrderId = tempSaleOrder.OrderId, Notional = tempSaleOrder.Notional, Volume = sales });


                        tempSaleOrder.Matches.Add(new Match { OrderId = bid.OrderId, Notional = tempSaleOrder.Notional, Volume = sales });
                        tempSaleOrder.Volume -= sales;
                        if (tempSaleOrder.Volume == 0)
                        { tempSaleOrder.MatchState = "FullMatch"; }
                        else { tempSaleOrder.MatchState = "Partialmatch"; }
                        _fastBookOrdered.OrderUpdate(tempSaleOrder);
                        break;

                    case int n when n == 0:
                        tempSaleOrder.MatchState = "FullMatch";
                        tempSaleOrder.Matches.Add(new Match { OrderId = bid.OrderId, Notional = tempSaleOrder.Notional, Volume = tempSaleOrder.Volume });
                        tempSaleOrder.Volume = 0;
                        _fastBookOrdered.OrderUpdate(tempSaleOrder);

                        bid.MatchState = "FullMatch";
                        bid.Matches.Add(new Match { OrderId = tempSaleOrder.OrderId, Notional = tempSaleOrder.Notional, Volume = bid.Volume });
                        bid.Volume = 0;
                        break;

                }
            }
            return bid;
        }
    }
}

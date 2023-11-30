using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public class PriceTimePriorityAlgorithm : IPriceTimePriorityAlgorithm
    {
        private readonly IFastBookOrdered _fastBookOrdered;
        private readonly ISellOrders _sellOrders;

        public PriceTimePriorityAlgorithm(IFastBookOrdered fastBookOrdered, ISellOrders sellOrders)
        {
            _fastBookOrdered = fastBookOrdered;
            _sellOrders = sellOrders;
        }

        public BookOrder PriceTimePriority(BookOrder inputBid)
        {
            _ = new BookOrder();
            BookOrder bid = inputBid;

            if (bid.MatchState != "FullMatch")
            {
                foreach (var s in _sellOrders.SellsOrderQuickList())
                {
                    var sale = _fastBookOrdered.GetOrder(s.Value.OrderId);

                    if (bid.Notional >= sale.Notional)
                    {
                        var tempSaleOrder = sale;
                        tempSaleOrder.Matches = new List<Match>();

                        int FinalVolume = bid.Volume - tempSaleOrder.Volume;
                        switch (FinalVolume)
                        {
                            case int n when n > 0:
                                bid.Volume -= tempSaleOrder.Volume;
                                bid.MatchState = "PartialMatch";
                                bid.Matches.Add(new Match { OrderId = tempSaleOrder.OrderId, Notional = tempSaleOrder.Notional, Volume = tempSaleOrder.Volume });

                                tempSaleOrder.MatchState = "FullMatch";
                                tempSaleOrder.Matches.Add(new Match { OrderId = bid.OrderId, Notional = tempSaleOrder.Notional, Volume = tempSaleOrder.Volume });
                                tempSaleOrder.Volume = 0;
                                _fastBookOrdered.OrderUpdate(tempSaleOrder);
                                break;

                            case int n when n < 0:
                                tempSaleOrder.Volume -= bid.Volume;
                                tempSaleOrder.MatchState = "PartialMatch";
                                tempSaleOrder.Matches.Add(new Match { OrderId = bid.OrderId, Notional = tempSaleOrder.Notional, Volume = bid.Volume });
                                _fastBookOrdered.OrderUpdate(tempSaleOrder);

                                bid.MatchState = "Fullmatch";
                                bid.Matches.Add(new Match { OrderId = tempSaleOrder.OrderId, Notional = tempSaleOrder.Notional, Volume = bid.Volume });
                                bid.Volume = 0;
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
                    else { break; }
                }
            }

            return bid;
        }

    }
}

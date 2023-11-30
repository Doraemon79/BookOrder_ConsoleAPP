using Moq;
using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Logic;
using TP_ICAP_ConsoleApp.Models;
using Match = TP_ICAP_ConsoleApp.Models.Match;

namespace TP_ICAP_ConsoleAPP_Tests
{
    public class PriceTimePriorityAlgorithmTests
    {
        private readonly PriceTimePriorityAlgorithm PriceTimePriorityAlgorithm_Sut;
        private readonly Mock<IFastBookOrdered> _fastBookOrdered = new Mock<IFastBookOrdered>();
        private readonly Mock<ISellOrders> _sellOrders = new Mock<ISellOrders>();

        public PriceTimePriorityAlgorithmTests()
        {
            PriceTimePriorityAlgorithm_Sut = new PriceTimePriorityAlgorithm(_fastBookOrdered.Object, _sellOrders.Object);
        }

        BookOrder buy0 = new BookOrder { OrderId = "A0", Company = "A", Notional = 2.00, Volume = 0, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Buy" };
        BookOrder buy1 = new BookOrder { OrderId = "A1", Company = "A", Notional = 2.01, Volume = 100, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Buy" };
        BookOrder buy2 = new BookOrder { OrderId = "A2", Company = "A", Notional = 2.01, Volume = 150, OrderDateTime = new TimeSpan(1, 30, 0), MatchState = "NoMatch", OrderType = "Buy" };
        BookOrder buy3 = new BookOrder { OrderId = "A3", Company = "A", Notional = 2.01, Volume = 10, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Buy" };

        BookOrder sell0 = new BookOrder { OrderId = "A0", Company = "A", Notional = 2.01, Volume = 0, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Sell" };
        BookOrder sell1 = new BookOrder { OrderId = "B1", Company = "B", Notional = 2.01, Volume = 100, OrderDateTime = new TimeSpan(1, 30, 0), MatchState = "NoMatch", OrderType = "Sell" };
        BookOrder sell2 = new BookOrder { OrderId = "C2", Company = "C", Notional = 2.01, Volume = 150, OrderDateTime = new TimeSpan(1, 30, 0), MatchState = "NoMatch", OrderType = "Sell" };
        BookOrder sell3 = new BookOrder { OrderId = "D3", Company = "D", Notional = 2.01, Volume = 10, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Sell" };
        BookOrder sell4 = new BookOrder { OrderId = "E4", Company = "E", Notional = 2.01, Volume = 200, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Sell" };
        BookOrder sell5 = new BookOrder { OrderId = "F5", Company = "C", Notional = 2.02, Volume = 150, OrderDateTime = new TimeSpan(1, 30, 0), MatchState = "NoMatch", OrderType = "Sell" };


        [Fact]
        public void PriceTimePriority_ShouldReturn_BuyOrder_FullMatch_SellOrder_FullMatch()
        {
            //Arrange
            List<BookOrder> SampleBookOrder = new List<BookOrder>();
            sell1.Matches = new List<Match>();
            sell5.Matches = new List<Match>();
            buy1.Matches = new List<Match>();
            var sellsOrder = new List<KeyValuePair<string, BookOrder>>();
            sellsOrder.Add(new KeyValuePair<string, BookOrder>(sell1.OrderId, sell1));
            sellsOrder.Add(new KeyValuePair<string, BookOrder>(sell5.OrderId, sell5));
            _sellOrders.Setup(x => x.SellsOrderQuickList()).Returns(sellsOrder);
            _fastBookOrdered.Setup(x => x.OrderUpdate(buy1));

            SampleBookOrder.Add(buy1);

            //Act
            var tst = PriceTimePriorityAlgorithm_Sut.PriceTimePriority(buy1);

            //Assert
            Assert.Equal(0, tst.Volume);
            Assert.Equal("FullMatch", tst.MatchState);
        }

        [Fact]
        public void PriceTimePriority_ShouldReturn_BuyOrder_PartialMatch_SellOrder_FullMatch()
        {
            //Arrange
            List<BookOrder> SampleBookOrder = new List<BookOrder>();
            var sellsOrder = new List<KeyValuePair<string, BookOrder>>();
            var orders = new Dictionary<string, BookOrder>();

            sell3.Matches = new List<Match>();
            sell5.Matches = new List<Match>();
            buy1.Matches = new List<Match>();
            sellsOrder.Add(new KeyValuePair<string, BookOrder>(sell3.OrderId, sell3));
            sellsOrder.Add(new KeyValuePair<string, BookOrder>(sell5.OrderId, sell5));

            //orders.Add(buy1.OrderId, buy1);
            //orders.Add(sell3.OrderId, sell3);
            //orders.Add(sell5.OrderId, sell5);
            _sellOrders.Setup(x => x.SellsOrderQuickList()).Returns(sellsOrder);
            //var tst2 = _fastBookOrdered.Setup(x => x.BookOrder()).Returns(orders);
            _fastBookOrdered.Setup(x => x.OrderUpdate(buy1));

            SampleBookOrder.Add(buy1);

            //Act
            var tst = PriceTimePriorityAlgorithm_Sut.PriceTimePriority(buy1);
           

            //Assert
            Assert.Equal(90, tst.Volume);
            Assert.Equal("PartialMatch", tst.MatchState);
            Assert.Equal("D3", tst.Matches[0].OrderId);
            Assert.Equal(10, tst.Matches[0].Volume);
        }
    }
}

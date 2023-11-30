using Moq;
using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Logic;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleAPP_Tests
{
    public class ProRataPriorityAlgorithmTests
    {
        private readonly ProRataAlgorithm ProRataAlgorithm_Sut;
        private readonly Mock<IFastBookOrdered> _fastBookOrdered = new Mock<IFastBookOrdered>();
        private readonly Mock<ISellOrders> _sellOrders = new Mock<ISellOrders>();

        public ProRataPriorityAlgorithmTests()
        {
            //FastBookOrdered_Sut = new FastBookOrdered();
            //_fastBookOrdered = fastBookOrdered;
            //_sellOrders = sellOrders;
            ProRataAlgorithm_Sut = new ProRataAlgorithm(_fastBookOrdered.Object, _sellOrders.Object);
        }

        BookOrder buy0 = new BookOrder { OrderId = "A0", Company = "A", Notional = 5.00, Volume = 0, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "buy" };
        BookOrder buy1 = new BookOrder { OrderId = "A1", Company = "A", Notional = 5.00, Volume = 50, OrderDateTime = new TimeSpan(9, 27, 43), MatchState = "NoMatch", OrderType = "buy" };
        BookOrder buy2 = new BookOrder { OrderId = "B1", Company = "A", Notional = 5.00, Volume = 200, OrderDateTime = new TimeSpan(1, 30, 0), MatchState = "NoMatch", OrderType = "buy" };
        BookOrder buy3 = new BookOrder { OrderId = "A3", Company = "A", Notional = 2.01, Volume = 10, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "buy" };

        BookOrder sell0 = new BookOrder { OrderId = "A0", Company = "A", Notional = 2.01, Volume = 0, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "sell" };
        BookOrder sell1 = new BookOrder { OrderId = "B1", Company = "B", Notional = 5.00, Volume = 200, OrderDateTime = new TimeSpan(10, 21, 46), MatchState = "NoMatch", OrderType = "sell" };
        BookOrder sell2 = new BookOrder { OrderId = "C1", Company = "C", Notional = 5.00, Volume = 200, OrderDateTime = new TimeSpan(10, 26, 18), MatchState = "NoMatch", OrderType = "sell" };
        BookOrder sell3 = new BookOrder { OrderId = "D3", Company = "D", Notional = 5.00, Volume = 10, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "sell" };
        BookOrder sell4 = new BookOrder { OrderId = "E4", Company = "E", Notional = 5.00, Volume = 200, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "sell" };
        BookOrder sell5 = new BookOrder { OrderId = "F5", Company = "C", Notional = 5.00, Volume = 150, OrderDateTime = new TimeSpan(1, 30, 0), MatchState = "NoMatch", OrderType = "sell" };


        [Fact]
        public void ProRata_ShouldReturn_BuyOrder_FullMatch_SellOrder_FullMatch()
        {
            //Arrange
            List<BookOrder> SampleBookOrder = new List<BookOrder>();
            var sellsOrder = new List<KeyValuePair<string, BookOrder>>();
            sellsOrder.Add(new KeyValuePair<string, BookOrder>(sell1.OrderId, sell1));
            _sellOrders.Setup(x => x.SellsOrderQuickList()).Returns(sellsOrder);
            _fastBookOrdered.Setup(x => x.OrderUpdate(buy1));


            SampleBookOrder.Add(buy1);

            //Act
            var tst = ProRataAlgorithm_Sut.ProRataMatcherForBuy(buy1, 250, 200);


            //Assert
            Assert.Equal(10, tst.Volume);
            Assert.Equal("PartialMatch", tst.MatchState);
        }

        [Fact]
        public void ProRata_ShouldReturn_BuyOrder_PartialMatch_SellOrder_FullMatch()
        {
            //Arrange
            List<BookOrder> SampleBookOrder = new List<BookOrder>();
            var sellsOrder = new List<KeyValuePair<string, BookOrder>>();
            var bookOrders = new Dictionary<string, BookOrder>();
            bookOrders.Add(sell3.OrderId, sell3);
            bookOrders.Add(sell5.OrderId, sell5);
            sellsOrder.Add(new KeyValuePair<string, BookOrder>(sell3.OrderId, sell3));
            sellsOrder.Add(new KeyValuePair<string, BookOrder>(sell5.OrderId, sell5));
            _sellOrders.Setup(x => x.SellsOrderQuickList()).Returns(sellsOrder);
            _fastBookOrdered.Setup(x => x.OrderUpdate(buy1));
            _fastBookOrdered.Setup(x => x.BookOrder()).Returns(bookOrders);
            _fastBookOrdered.Setup(x => x.GetOrder(sell3.OrderId)).Returns(sell3);
            _fastBookOrdered.Setup(x => x.GetOrder(sell5.OrderId)).Returns(sell5);

            SampleBookOrder.Add(buy1);
            SampleBookOrder.Add(buy2);

            //Act


            var tst = ProRataAlgorithm_Sut.ProRataMatcherForBuy(buy1, 250, 160);


            //Assert
            Assert.Equal(18, tst.Volume);
            Assert.Equal("PartialMatch", tst.MatchState);
            Assert.Equal("D3", tst.Matches[0].OrderId);
            Assert.Equal(2, tst.Matches[0].Volume);
        }
    }
}

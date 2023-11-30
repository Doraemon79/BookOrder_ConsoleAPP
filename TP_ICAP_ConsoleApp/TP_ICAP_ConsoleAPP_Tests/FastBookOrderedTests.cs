using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Logic;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleAPP_Tests
{
    public class FastBookOrderedTests
    {
        private readonly FastBookOrdered FastBookOrdered_Sut;

        public FastBookOrderedTests()
        {
            FastBookOrdered_Sut = new FastBookOrdered();
        }

        BookOrder buy0 = new BookOrder { OrderId = "A0", Company = "A", Notional = 2.00, Volume = 0, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Buy" };
        BookOrder buy1 = new BookOrder { OrderId = "A1", Company = "A", Notional = 2.00, Volume = 100, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Buy" };
        BookOrder buy2 = new BookOrder { OrderId = "A1", Company = "A", Notional = 2.00, Volume = 100, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Buy" };

        BookOrder sell0 = new BookOrder { OrderId = "A2", Company = "A", Notional = 2.00, Volume = 80, OrderDateTime = new TimeSpan(1, 30, 1), MatchState = "NoMatch", OrderType = "Sell" };
        BookOrder sell1 = new BookOrder { OrderId = "A1", Company = "Z", Notional = 2.00, Volume = 10, OrderDateTime = new TimeSpan(1, 30, 2), MatchState = "NoMatch", OrderType = "Sell" };


        [Fact]
        public void BookOrderFiller_ShouldReturn_2orders()
        {
            //Arrange
            var bookOrders = new List<BookOrder>();
            bookOrders.Add(buy0);
            bookOrders.Add(buy1);
            bookOrders.Add(sell0);

            FastBookOrdered_Sut.FastBookOrderCreator();
            FastBookOrdered_Sut.InvalidOrderListCreator();
            var orders = FastBookOrdered_Sut.BookOrder();
            var invalids = FastBookOrdered_Sut.InvalidOrdersList();

            //Act
            FastBookOrdered_Sut.BookOrderFiller(bookOrders, orders, invalids);

            //Assert
            Assert.Equal(2, orders.Count);
        }

        [Fact]
        public void BookOrderFiller_ShouldReturn_Earliest_Invalid()
        {
            //Arrange
            var bookOrders = new List<BookOrder>();
            bookOrders.Add(buy0);
            bookOrders.Add(buy1);
            bookOrders.Add(sell0);

            FastBookOrdered_Sut.FastBookOrderCreator();
            FastBookOrdered_Sut.InvalidOrderListCreator();
            var orders = FastBookOrdered_Sut.BookOrder();
            var invalids = FastBookOrdered_Sut.InvalidOrdersList();

            //Act
            FastBookOrdered_Sut.BookOrderFiller(bookOrders, orders, invalids);

            //Assert
            var time = new TimeSpan(1, 30, 2);
            Assert.Single(invalids);
            Assert.Equal(time, invalids[0].OrderDateTime);
        }

        [Fact]
        public void OrderUpdate_ShouldReturn_OrderUpdate()
        {
            //Arrange
            var bookOrders = new List<BookOrder>();
            bookOrders.Add(buy0);
            bookOrders.Add(buy1);
            bookOrders.Add(sell0);

            FastBookOrdered_Sut.FastBookOrderCreator();
            FastBookOrdered_Sut.InvalidOrderListCreator();
            var orders = FastBookOrdered_Sut.BookOrder();
            var invalids = FastBookOrdered_Sut.InvalidOrdersList();

            //Act
            FastBookOrdered_Sut.OrderUpdate(sell1);
            orders.TryGetValue(sell1.OrderId, out BookOrder order);

            //Assert
            Assert.Equal("Z", order.Company);
        }

    }
}

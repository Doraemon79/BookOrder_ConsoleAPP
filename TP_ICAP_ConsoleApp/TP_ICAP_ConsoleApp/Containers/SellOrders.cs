using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Containers
{
    public class SellOrders : ISellOrders
    {
        public List<BookOrder> SellOrdersList { get; set; }

        //public Dictionary<string,BookOrder> SellsQuick{ get; set; }
        public List<KeyValuePair<string, BookOrder>> SellsQuickList { get; set; }

        public SellOrders() { }

       //public void SellOrdersMaker(List<BookOrder> inputOrders)
       // {
       //     SellOrdersList= inputOrders;
       // }

        //public Dictionary<string, BookOrder> OrdersOfSell()
        //{
        //    return SellsQuick;
        //}

        public void SellsQuickListMaker(List<KeyValuePair<string, BookOrder>> salesInput)
        {
            //Dictionary<string, BookOrder> tempSale=new Dictionary<string, BookOrder>();
            //foreach (var el in salesInput)
            //{
            //    tempSale.Add(el.Value.OrderId, el.Value);
            //}
            //SellsQuickList = tempSale;
            SellsQuickList = salesInput;
        }

        public List<KeyValuePair<string, BookOrder>> SellsOrderQuickList()
        {
            return SellsQuickList;
        }

        //public void SellsOrderUpdate(BookOrder sale)
        //{
        //    throw new NotImplementedException();
        //}

        //public Dictionary<string, BookOrder> OrdersOfSell()
        //{
        //    throw new NotImplementedException();
        //}

        //public void SellsOrderUpdate(BookOrder sale)
        //{
        //    SellsQuick[sale.OrderId]= sale;
        //}
    }
}

using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Containers
{
    public class SellOrders : ISellOrders
    {
        public List<BookOrder> SellOrdersList { get; set; }
        public List<KeyValuePair<string, BookOrder>> SellsQuickList { get; set; }

        public SellOrders() { }
        public void SellsQuickListMaker(List<KeyValuePair<string, BookOrder>> salesInput)
        {
            SellsQuickList = salesInput;
        }

        public List<KeyValuePair<string, BookOrder>> SellsOrderQuickList()
        {
            return SellsQuickList;
        }
    }
}

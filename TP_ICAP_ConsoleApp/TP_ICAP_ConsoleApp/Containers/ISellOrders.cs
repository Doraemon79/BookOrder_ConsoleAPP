using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Containers
{
    public interface ISellOrders
    {
        void SellsQuickListMaker(List<KeyValuePair<string, BookOrder>> salesInput);
        List<KeyValuePair<string, BookOrder>> SellsOrderQuickList();
    }
}

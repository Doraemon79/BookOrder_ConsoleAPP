using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Controllers
{
    public interface IOrdersProcessor
    {
        void ProcessBookOrder(List<BookOrder> inputBookOrder, string inputAlgo);
    }
}

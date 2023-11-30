using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public interface IMatchingPatterns
    {
        void Calculate(List<BookOrder> inputBookOrder);
    }
}

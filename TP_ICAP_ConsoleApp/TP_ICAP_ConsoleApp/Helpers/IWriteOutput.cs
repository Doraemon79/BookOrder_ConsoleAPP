using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Helpers
{
    public interface IWriteOutput
    {
        void WriteConsoleOutput(List<BookOrder> invalidOrders);
        void WriteConsoleOutput(Dictionary<string, BookOrder> fastBookOrdertable);
    }
}
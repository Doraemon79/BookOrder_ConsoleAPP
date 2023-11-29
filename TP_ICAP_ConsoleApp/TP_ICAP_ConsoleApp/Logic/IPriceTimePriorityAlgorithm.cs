using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public interface IPriceTimePriorityAlgorithm
    {
        BookOrder PriceTimePriority( BookOrder inputBid);
    }
}

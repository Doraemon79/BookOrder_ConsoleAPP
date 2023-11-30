using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Helpers;

namespace TP_ICAP_ConsoleApp.Logic
{
    public abstract class MatcherFactory
    {
        public abstract IMatchingPatterns GetAlgorithm(string Algorithm, IFastBookOrdered fastBookOrdered, ISellOrders sellOrders, IPriceTimePriorityAlgorithm matchAlgorithms, IProRataAlgorithm proRataAlgorithm, IWriteOutput writeOutput);
    }
}

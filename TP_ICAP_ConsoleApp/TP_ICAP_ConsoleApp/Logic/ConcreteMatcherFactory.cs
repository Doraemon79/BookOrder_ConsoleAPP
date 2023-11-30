using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Helpers;

namespace TP_ICAP_ConsoleApp.Logic
{
    public class ConcreteMatcherFactory : MatcherFactory
    {
        public override IMatchingPatterns GetAlgorithm(string Algorithm, IFastBookOrdered fastBookOrdered, ISellOrders sellOrders, IPriceTimePriorityAlgorithm priceTimePriorityAlgorithms, IProRataAlgorithm proRataAlgorithm, IWriteOutput writeOutput)
        {
            switch (Algorithm)
            {
                case ("Price-Time-Priority"):
                    return new PriceTimePriorityMatcher(fastBookOrdered, sellOrders, priceTimePriorityAlgorithms, writeOutput);

                case ("Pro-Rata"):
                    return new ProRataMatcher(fastBookOrdered, sellOrders, proRataAlgorithm, writeOutput);
                default:
                    throw new ApplicationException(string.Format("Algorithm of choice '{0}' cannot be found", Algorithm));
            }
        }
    }
}

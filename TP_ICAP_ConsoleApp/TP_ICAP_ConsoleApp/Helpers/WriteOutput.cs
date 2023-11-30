using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Helpers
{
    public class WriteOutput : IWriteOutput
    {
        public void WriteConsoleOutput(List<BookOrder> invalidOrders)
        {
            foreach (var invalid in invalidOrders)
            {
                var state = invalid.MatchState;
                Console.WriteLine(state switch
                {
                    "InvalidMatch" => $" {invalid.OrderId} - InvalidMatch a previous order with same Id already existed. ",
                    "WrongEntry" => $" {invalid.OrderId} - The order is not correctly inserted. ",
                    _ => $" {invalid.OrderId}-The order is Invalid. "
                });
            }
        }

        public void WriteConsoleOutput(Dictionary<string, BookOrder> fastBookOrdertable)
        {

            foreach (var order in fastBookOrdertable)
            {
                Console.WriteLine($" {order.Key} - {order.Value.MatchState}");
                if (order.Value.Matches != null && order.Value.Matches.Count > 0)
                {
                    foreach (var m in order.Value.Matches)
                    {
                        Console.WriteLine($"          +{m.OrderId}  -  {m.Volume}");
                    }
                }
            }
        }
    }
}

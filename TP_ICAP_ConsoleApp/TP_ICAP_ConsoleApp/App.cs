using Newtonsoft.Json;
using TP_ICAP_ConsoleApp.Controllers;
using TP_ICAP_ConsoleApp.Logic;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp
{
    public class App
    {
        private readonly IPriceTimePriorityAlgorithm _priceTimePriorityAlgorithms;
        private readonly IOrdersProcessor _orderProcessor;

        public App(IPriceTimePriorityAlgorithm matchAlgorithms, IOrdersProcessor ordersProcessor)
        {
            _priceTimePriorityAlgorithms = matchAlgorithms;
            _orderProcessor = ordersProcessor;
        }

        public void Run(string[] args)
        {


            Console.WriteLine("Welcome, do you want to add a BookOrder? please use the format:  " +
    " [   { \"OrderId\": \"A1\",    \"Company\": \"A\",    \"OrderType\": \"Buy\",   \"Notional\": 2.014, \"Volume\": 150,    \"OrderDateTime\": \"09:27:43\"  },  {    \"OrderId\": \"B1\",    \"Company\": \"B\",    \"OrderType\": \"Buy\",    \"Notional\": 23.014,  \"Volume\": 150,    \"OrderDateTime\": \"10:21:43\" } ]");
            var Orders = Console.ReadLine();
            Console.WriteLine("Would you like to use Price-Time-Priority or Pro-Rata Alhgorithm?");


            string? AlgorithmChoice = Console.ReadLine();
            List<BookOrder> BookOrders = new();
            if (Orders != null)
            { BookOrders = JsonConvert.DeserializeObject<List<BookOrder>>(Orders); }

            if (AlgorithmChoice.Equals("Price-Time-Priority") || String.IsNullOrEmpty(AlgorithmChoice))
            {
                _orderProcessor.ProcessBookOrder(BookOrders, "Price-Time-Priority");
            }
            else
            {
                _orderProcessor.ProcessBookOrder(BookOrders, "Pro-Rata");
            }
            Console.ReadLine();
        }
    }
}

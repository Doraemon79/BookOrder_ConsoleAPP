using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Containers;
using TP_ICAP_ConsoleApp.Controllers;
using TP_ICAP_ConsoleApp.Logic;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp
{
    public class App
    {
        private readonly IMatchAlgorithms _matchAlgorithms;
        private readonly IOrdersProcessor _orderProcessor;

        public App(IMatchAlgorithms matchAlgorithms, IOrdersProcessor ordersProcessor)
        {
            _matchAlgorithms = matchAlgorithms;
            _orderProcessor = ordersProcessor;
        }

        public void Run(string[] args)
        {
            Console.WriteLine("Welcome, do you want to add a BookOrder? please use the format:  " +
    " [   { \"OrderId\": \"A1\",    \"Company\": \"A\",    \"OrderType\": \"Buy\",   \"Notional\": 2.014, \"Volume\": 150,    \"OrderDateTime\": \"09:27:43\"  },  {    \"OrderId\": \"B1\",    \"Company\": \"B\",    \"OrderType\": \"Buy\",    \"Notional\": 23.014,  \"Volume\": 150,    \"OrderDateTime\": \"10:21:43\" } ]");
            string Orders = Console.ReadLine();
    
            List<BookOrder> BookOrders = JsonConvert.DeserializeObject<List<BookOrder>>(Orders);
       


            Console.WriteLine("Would you like to use Price-Time-Priority or Pro-Rata Alhgorithm?");
            
            
            string AlgorithmChoice = Console.ReadLine();
            if (AlgorithmChoice.Equals(" Price-Time-Priority") || String.IsNullOrEmpty(AlgorithmChoice))
            {
                _orderProcessor.ProcessBookOrder(BookOrders);
                //var matches = _matchAlgorithms.PriceTimePriority(BookOrders);
            }
            Console.ReadLine();
            Console.WriteLine(BookOrders[0]);
        }
    }
}

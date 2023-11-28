using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public interface IMatchAlgorithms
    {
        Task<List<BookOrder>> PriceTimePriority(List<BookOrder> bookOrders);

        List<BookOrder> ProRata(List<BookOrder> bookOrders);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Containers
{
    public interface ISellOrders
    {
        void SellsQuickListMaker(List<KeyValuePair<string, BookOrder>> salesInput);
        List<KeyValuePair<string, BookOrder>> SellsOrderQuickList();
    }
}

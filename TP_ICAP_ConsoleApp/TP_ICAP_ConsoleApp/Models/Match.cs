using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_ICAP_ConsoleApp.Models
{
    public struct Match
    {
       public string OrderId {  get; set; }
       public double Notional { get; set; }
       public int Volume {  get; set; }   
    }
}

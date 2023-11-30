using Newtonsoft.Json;
using System.ComponentModel;

namespace TP_ICAP_ConsoleApp.Models
{
    public struct BookOrder
    {
        public string OrderId { get; set; }
        public string Company { get; set; }
        public double Notional { get; set; }
        public string OrderType { get; set; }
        public int Volume { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("NoMatch")]
        public string MatchState { get; set; }
        public TimeSpan OrderDateTime { get; set; }

        public List<Match> Matches { get; set; }

    }
}

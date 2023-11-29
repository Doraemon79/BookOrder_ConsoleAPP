using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public class MatchAlgorithms : IMatchAlgorithms
    {
       private List<BookOrder> BookOrders { get; set; }
        private List<KeyValuePair<string, BookOrder>> SellOrders { get; set; }
        private SortedSet<BookOrder> FullMatchBookOrders { get; set; }
        private Dictionary<string, BookOrder> FastBookOrdertable = new Dictionary<string, BookOrder>();
        SortedList<Tuple<double, TimeSpan, string>, string> BookOrdersSortedList = new SortedList<Tuple<double, TimeSpan, string>, string>(new My2TupleComparer());
        
         
        public async Task<List<BookOrder>> PriceTimePriority(List<BookOrder> bookOrders)
        {


            //create 2 classes for the algho
            //%of ratio and the remainder is subtracted from the buyOrder


            //////////Pric TIme Priority
            //sort list bby date 

            List<BookOrder> InvalidOrders = new List<BookOrder>();
            List<BookOrder> sortedBookOrder= bookOrders.OrderBy(x => x.OrderDateTime).ToList();
            // make dicrionary FastBookOrdertable
            foreach(var t in sortedBookOrder)
            {
                var isDuplicate = FastBookOrdertable.TryAdd(t.OrderId,t);
                if(!isDuplicate)
                {
                    InvalidOrders.Add(t);
                }

            }

            var BuyOrders = FastBookOrdertable.OrderBy(x => x.Value.OrderDateTime).ThenByDescending(x => x.Value.Notional).Where(x => x.Value.OrderType.Equals("Buy")).ToList();
            SellOrders = FastBookOrdertable.OrderBy(x => x.Value.OrderDateTime).ThenByDescending(x => x.Value.Notional).Where(x => x.Value.OrderType.Equals("Sell")).ToList();

            foreach(var el in BuyOrders)
            {
                var bid = MatcherForBuy_v2(el);
                FastBookOrdertable[bid.OrderId] = bid;

            }

            //foreach(var sale in SellOrders)
            //{
            //    FastBookOrdertable[sale.Value.OrderId] = sale.Value;
            //}

            foreach (var invalid in InvalidOrders)
            {
                Console.WriteLine($" {invalid.OrderId} - InvalidMatch a previous order with same Id already existed. ");
            }
            foreach (var order in FastBookOrdertable)
            {
                Console.WriteLine($" {order.Key} - {order.Value.MatchState}");
                if (order.Value.Matches != null&&  order.Value.Matches.Count > 0)
                {
                    foreach (var m in order.Value.Matches)
                    {
                        Console.WriteLine($"          +{m.OrderId}  -  { m.Volume}");
                    }
                }
            }


//////////////////////////////////////////////////////////////////old ///////////////////////////////////////////////////////

            /*
                // save invalid and remove from original list

                //select buy and order them by time ascending
                BookOrders = bookOrders;
            //HashSet<string> OrderIds = new HashSet<string>();
            //HashSet<BookOrder> FastBookOrder = new HashSet<BookOrder>();
            //BookOrders = SortedBookOrder;

            //var BuyOrders = BookOrders.OrderBy(x => x.OrderDateTime).ThenByDescending(x => x.Notional).Where(x => x.OrderType.Equals("Buy")).ToArray();
            //var SellOrders = BookOrders.OrderBy(x => x.OrderDateTime).ThenByDescending(x => x.Notional).Where(x => x.OrderType.Equals("Sell")).ToArray();

            foreach (var buy in BuyOrders)
            {
                var t = sellOrders.Where(sell => sell.OrderDateTime <= buy.OrderDateTime)
            }

            // divide main input in 2 list bid and offer
            //order the lists
            // take first BUY and browse for matching and update
            //Next BUY



            //////////////////////////////////////////
            //Pro Rata
            //  StringSplitOptions into lists buy sell
            // 
            var rataBuy = BuyOrders.OrderBy(x => x.Volume / BuyOrders.Sum(i => i.Volume));
            var rataSell = SellOrders;
            var orders = new List<(string, int)>();

            foreach (var buy in BuyOrders)
            {
                var item = rataSell.Where(sell => sell.Notional <= buy.Notional).Sum(i => i.Volume);

            }




            //foreach (var order in bookOrders)
            BookOrder tempOrder = new BookOrder();
            SortedList<Tuple<double, TimeSpan, string>, string> BookOrdersSortedList = new SortedList<Tuple<double, TimeSpan, string>, string>(new My2TupleComparer());
            List<string> OrderIds=new List<string>();

            //List<BookOrder> InvalidOrders = new List<BookOrder>();
            //Hashtable FastBookOrdertable = new Hashtable();
            //Dictionary<string, BookOrder> FastBookOrdertable = new Dictionary<string, BookOrder>();
            for (int i= 0; i <= bookOrders.Count - 1; i++)
            {
                tempOrder = bookOrders[i];

                if (FastBookOrdertable.ContainsKey(tempOrder.OrderId))
                {
                    if ( FastBookOrdertable[tempOrder.OrderId].OrderDateTime< tempOrder.OrderDateTime)
                    {
                        InvalidOrders.Add(tempOrder);
                    }
                    else
                    {
                        InvalidOrders.Add(FastBookOrdertable[tempOrder.OrderId]);
                        BookOrdersSortedList.Remove(new Tuple<double, TimeSpan, string>(FastBookOrdertable[tempOrder.OrderId].Notional, FastBookOrdertable[tempOrder.OrderId].OrderDateTime, FastBookOrdertable[tempOrder.OrderId].OrderId ));
                        BookOrdersSortedList.Add(new Tuple<double, TimeSpan, string>(tempOrder.Notional, tempOrder.OrderDateTime, tempOrder.OrderId), tempOrder.OrderId);
                        FastBookOrdertable[tempOrder.OrderId]= tempOrder;
                       
                    }
                }

                else
                {
                    FastBookOrdertable.Add(tempOrder.OrderId, tempOrder);
                    BookOrdersSortedList.Add(new Tuple<double, TimeSpan, string>(tempOrder.Notional, tempOrder.OrderDateTime, tempOrder.OrderId), tempOrder.OrderId);

                }
            }


            List<BookOrder> buyOrders = (List<BookOrder>)bookOrders.Where(x => x.OrderType.Equals("buy")).OrderBy(x => x.OrderDateTime).ThenBy(x => x.OrderDateTime).ToList();
            List<BookOrder> sellOrders = (List<BookOrder>)bookOrders.Where(x => x.OrderType.Equals("sell")).OrderBy(x => x.Notional).ThenBy(x => x.OrderDateTime).ToList();

            List<BookOrder> SortedBookOrder = bookOrders.OrderByDescending(x => x.Notional).ThenBy(x => x.OrderDateTime).ToList();






            //SortedList<Tuple<double, TimeSpan>, BookOrder> BookOrdersSortedList = new SortedList<Tuple<double, TimeSpan>, BookOrder>(new My2TupleComparer());

            foreach (var el in BookOrdersSortedList)
            {
                BookOrder bid = MatcherForBuy(FastBookOrdertable[el.Value]);
                FastBookOrdertable[el.Value] = bid;
                //if (bid.MatchState.Equals("FullMatch"))
                //{
                //    FullMatchBookOrders.Add(bid);
                //    BookOrders.RemoveAt(i);
                //}
                //else { BookOrders[i] = bid; }
            }


        //    if (BookOrders.Any())
        //    {


        //        for (int i = 0; i <= BookOrders.Count - 1; i++)
        //        {
        //            BookOrder bid =  MatcherForBuy(BookOrders[i]);
        //            if (bid.MatchState.Equals("FullMatch"))
        //            {
        //                FullMatchBookOrders.Add(bid);
        //                BookOrders.RemoveAt(i);
        //            }
        //            else { BookOrders[i] = bid; }
        //}

        //    }

            //List<BookOrder> fullMatches = [.. FullMatchBookOrders, .. BookOrders];

            //massage list to make it readable

            foreach(var invalid in InvalidOrders)
            {
                Console.WriteLine($" {invalid.OrderId} - InvalidMatch a previous order with same Id already existed. ");
            }

            foreach(var el in BookOrdersSortedList)
            {
                BookOrders.Add(FastBookOrdertable[el.Value]);
                Console.WriteLine($" {FastBookOrdertable[el.Value].OrderId} - {FastBookOrdertable[el.Value].MatchState}");
                if(FastBookOrdertable[el.Value].Matches.Count>0 )
                {
                  
                    foreach (var m in FastBookOrdertable[el.Value].Matches)
                    {
                        Console.WriteLine($"          +{FastBookOrdertable[el.Value].OrderId}  -  {FastBookOrdertable[el.Value].Volume}");
                    }
                }
            }


            */
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
          
            return BookOrders;
        }

        public BookOrder MatcherForBuy_v2 (KeyValuePair<string,BookOrder> inputBid)
        {
            BookOrder bid = new BookOrder();
            bid = inputBid.Value;
            if (bid.Matches == null)
            {
                bid.Matches = new List<Match>();
            }
            if (bid.MatchState != "FullMatch")
            {
                foreach(var sale in SellOrders)
                {
                    if(bid.Notional>=sale.Value.Notional)
                    {
                        var tempSaleOrder = sale.Value;
                        tempSaleOrder.Matches=new List<Match>();

                        int FinalVolume = bid.Volume - tempSaleOrder.Volume  ;
                        switch (FinalVolume)
                        {
                            case int n when n > 0:
                                bid.Volume -= tempSaleOrder.Volume;
                                bid.MatchState = "Partialmatch";
                                bid.Matches.Add( new Match { OrderId=tempSaleOrder.OrderId, Notional= tempSaleOrder.Notional, Volume=tempSaleOrder.Volume });

                                
                                tempSaleOrder.MatchState = "Fullmatch";
                                tempSaleOrder.Matches.Add(new Match { OrderId = bid.OrderId, Notional = tempSaleOrder.Notional, Volume = tempSaleOrder.Volume });
                                tempSaleOrder.Volume = 0;
                                FastBookOrdertable[tempSaleOrder.OrderId] = tempSaleOrder;
                                break;

                            case int n when n < 0:
                                tempSaleOrder.Volume -= bid.Volume;
                                tempSaleOrder.MatchState = "Partialmatch";
                                tempSaleOrder.Matches.Add(new Match { OrderId=bid.OrderId, Notional=tempSaleOrder.Notional, Volume=bid.Volume });
                                FastBookOrdertable[tempSaleOrder.OrderId] = tempSaleOrder;

                                bid.MatchState = "Fullmatch";
                                bid.Matches.Add(new Match { OrderId = tempSaleOrder.OrderId, Notional = tempSaleOrder.Notional, Volume = bid.Volume });
                                bid.Volume = 0;
                                break;

                                case int n when n == 0:
                                tempSaleOrder.MatchState = "FullMatch";
                                tempSaleOrder.Matches.Add(new Match { OrderId = bid.OrderId, Notional = tempSaleOrder.Notional, Volume = tempSaleOrder.Volume });
                                tempSaleOrder.Volume = 0;
                                FastBookOrdertable[tempSaleOrder.OrderId] = tempSaleOrder;

                                bid.MatchState = "FullMatch";
                                bid.Matches.Add(new Match { OrderId=tempSaleOrder.OrderId, Notional=tempSaleOrder.Notional, Volume=bid.Volume});
                                bid.Volume = 0;
                                break;


                        }

                    }
                    else { break; }
                }

            }



                return bid;
        }

        //public   BookOrder MatcherForBuy(BookOrder inputBid)
        //{
        //    BookOrder bid = new BookOrder();
        //    bid = inputBid;
        //    if (bid.Matches == null)
        //    {
        //        bid.Matches = new SortedList<string, Match>();
        //    }
        //    if (bid.MatchState != "FullMatch")
        //    {

        //        //for (int i = 0; i <= BookOrdersSortedList.Count - 1; i++)
        //            foreach(var el in BookOrdersSortedList)
        //        {
        //            BookOrder tempOrder = new BookOrder();
        //            tempOrder = FastBookOrdertable[el.Value];
        //            if (tempOrder.Matches == null)
        //            {
        //                tempOrder.Matches = new SortedList<string, Match>();
        //            }

        //            if (!tempOrder.OrderType.Equals(bid.OrderType) && tempOrder.Notional <= bid.Notional && tempOrder.MatchState!="FullMatch" )
        //            {

        //                int FinalVolume = tempOrder.Volume - bid.Volume;
        //                switch (FinalVolume)
        //                {
        //                    case int n when n > 0:
        //                        tempOrder.Matches.Add(bid.OrderId, new() { OrderId = bid.OrderId, Volume = bid.Volume, Notional = bid.Notional });
        //                        tempOrder.MatchState = "PartialMatch";
        //                        tempOrder.Volume = FinalVolume;
        //                        FastBookOrdertable[tempOrder.OrderId]= tempOrder;
        //                        //BookOrders[i] = tempOrder;

        //                        bid.Matches.Add(bid.OrderId, new() { OrderId = tempOrder.OrderId, Volume = bid.Volume, Notional = bid.Notional });
        //                        bid.MatchState = "FullMatch";
        //                        bid.Volume = 0;
        //                        FastBookOrdertable[bid.OrderId]= bid;
        //                        //FullMatchBookOrders.Add(bid);
        //                        //add Match
        //                        break;

        //                    case int n when n < 0:
        //                        bid.Matches.Add(bid.OrderId, new() { OrderId = tempOrder.OrderId, Volume = tempOrder.Volume, Notional = bid.Notional });
        //                        bid.MatchState = "PartialMatch";
        //                        bid.Volume -= tempOrder.Volume;

        //                        tempOrder.Matches.Add(bid.OrderId, new() { OrderId = bid.OrderId, Volume = tempOrder.Volume, Notional = bid.Notional });
        //                        tempOrder.MatchState = "Fullmatch";
        //                        tempOrder.Volume = 0;
        //                        FastBookOrdertable[tempOrder.OrderId] = bid;
        //                        //FullMatchBookOrders.Add(tempOrder);
        //                        //BookOrders.RemoveAt(i);
        //                        //BookOrders[i] = tempOrder;
        //                        //add Match
        //                        break;

        //                    case int n when n == 0:
        //                        bid.Matches.Add(bid.OrderId, new() { OrderId = tempOrder.OrderId, Volume = tempOrder.Volume, Notional = bid.Notional });
        //                        bid.MatchState = "Fullmatch";
        //                        bid.Volume = 0;
        //                        FastBookOrdertable[bid.OrderId]= bid;
        //                        //FullMatchBookOrders.Add(bid);

        //                        tempOrder.Matches.Add(bid.OrderId, new() { OrderId = bid.OrderId, Volume = bid.Volume, Notional = bid.Notional });
        //                        tempOrder.MatchState = "Fullmatch";
        //                        tempOrder.Volume = 0;
        //                        FastBookOrdertable[tempOrder.OrderId]= tempOrder;
        //                        //FullMatchBookOrders.Add(tempOrder);
        //                        //BookOrders.RemoveAt(i);
        //                        //BookOrders[i] = tempOrder;
        //                        //add Match
        //                        break;
        //                    default:
        //                        throw new NotImplementedException();
        //                };

        //            }
        //        }
        //    }

        //    return bid;
        //}


        public List<BookOrder> ProRata(List<BookOrder> bookOrders)
        {
            throw new NotImplementedException();
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_ICAP_ConsoleApp.Models;

namespace TP_ICAP_ConsoleApp.Logic
{
    public class MatchAlgorithms : IMatchAlgorithms
    {
       private List<BookOrder> BookOrders { get; set; }
        private SortedSet<BookOrder> FullMatchBookOrders { get; set; }

        //public int ComparBookOrders(BookOrder bookOrder1, BookOrder bookOrder2)
        //{
        //    //if ((employee1 is null) || (employee2 is null))
        //    //{
        //    //    return 0;
        //    //}

        //  var priceResult= bookOrder1.Notional.CompareTo(bookOrder2.Notional);

        //    if (priceResult == 0)
        //    { priceResult = bookOrder2.OrderDateTime.CompareTo(bookOrder1.OrderDateTime); }
        //    if (priceResult == 0) { priceResult = bookOrder1.Id.CompareTo(bookOrder2.Id); }

        //    return priceResult;
        //}

        public async Task<List<BookOrder>> PriceTimePriority(List<BookOrder> bookOrders)
        {
            //select buy and order them by time ascending
            BookOrders=bookOrders;

            List<BookOrder> buyOrders = (List<BookOrder>)bookOrders.Where(x => x.OrderType.Equals("buy")).OrderBy(x => x.OrderDateTime).ThenBy(x => x.OrderDateTime).ToList();
            List<BookOrder> sellOrders = (List<BookOrder>)bookOrders.Where(x => x.OrderType.Equals("sell")).OrderBy(x => x.Notional).ThenBy(x => x.OrderDateTime).ToList();

            List<BookOrder> SortedBookOrder = bookOrders.OrderByDescending(x => x.Notional).ThenBy(x => x.OrderDateTime).ToList();
            BookOrders = SortedBookOrder;

           FullMatchBookOrders = new SortedSet<BookOrder>( new ComparBookOrders());

            //SortedSet<BookOrder> BookOrdersSet = new SortedSet<BookOrder>(bookOrders,new ComparBookOrders());
            SortedList<Tuple<double, TimeSpan>, BookOrder> BookOrdersSortedList = new SortedList<Tuple<double, TimeSpan>, BookOrder>(new My2TupleComparer());
           
            //foreach (var el in bookOrders)
            //{
            //    BookOrdersSortedList.Add(Tuple.Create(el.Notional, el.OrderDateTime), el);
            //}

            //if buy list not empty
            if (BookOrders.Any())
            {


                for (int i = 0; i <= BookOrders.Count - 1; i++)
                {
                    BookOrder bid =  MatcherForBuy(BookOrders[i]);
                    if (bid.MatchState.Equals("FullMatch"))
                    {
                        FullMatchBookOrders.Add(bid);
                        BookOrders.RemoveAt(i);
                    }
                    else { BookOrders[i] = bid; }
        }

            }

            List<BookOrder> fullMatches = [.. FullMatchBookOrders, .. BookOrders];


            foreach (var el in fullMatches)
            {
                Console.WriteLine(el);
            }
            Console.ReadLine();

            return fullMatches;
            //if (buyOrders.Any())
            //{
            //    //sellOrders = (List<BookOrder>)bookOrders.Where(x => x.OrderType.Equals("sell")).OrderBy(x => x.Notional).ThenBy(x => x.OrderDateTime).ToList();
            //    if (sellOrders.Any())
            //    {
            //        // foreach element in the buy list do:
            //        // check the price is less or equal to any value in the sell list, if there is any if not set no match
            //        //foreach (var buy in buyOrders)foreach doesnot allow for changes and I do not know if link reads the list in order
            //        for (int i = 0; i <= buyOrders.Count - 1; i++)
            //        {
            //            //brose sellList
            //            //foreach(var sale in sellOrders)
            //            for (int j = 0; j <= sellOrders.Count - 1; j++)
            //            {
            //                if (buyOrders[i].Notional >= sellOrders[j].Notional)
            //                {

            //                    if (sellOrders[j].Volume < buyOrders[i].Volume)
            //                    {

            //                        BookOrder tempOrder = buyOrders[i];
            //                        tempOrder = buyOrders[i];
            //                        tempOrder.Volume -= sellOrders[j].Volume;
            //                        tempOrder.MatchState = "PartialMatch";
            //                        buyOrders[i] = tempOrder;

            //                        tempOrder = sellOrders[j];
            //                        tempOrder.Volume = 0;
            //                        tempOrder.MatchState = "FullMatch";
            //                        sellOrders[j] = tempOrder;

            //                    }
            //                    else if (sellOrders[j].Volume > buyOrders[i].Volume)
            //                    {
            //                        BookOrder tempOrder = sellOrders[j];
            //                        tempOrder.Volume -= buyOrders[i].Volume;
            //                        tempOrder.MatchState = "PartialMatch";
            //                        sellOrders[j] = tempOrder;

            //                        tempOrder = buyOrders[i];
            //                        tempOrder.Volume = 0;
            //                        tempOrder.MatchState = "FullMatch";
            //                        buyOrders[i] = tempOrder;
            //                        break;
            //                    }
            //                    else if (sellOrders[j].Volume == buyOrders[i].Volume)
            //                    {
            //                        BookOrder tempOrder = sellOrders[j];
            //                        tempOrder.Volume = 0;
            //                        tempOrder.MatchState = "FullMatch";
            //                        sellOrders[j] = tempOrder;

            //                        tempOrder = buyOrders[i];
            //                        tempOrder.Volume = 0;
            //                        tempOrder.MatchState = "FullMatch";
            //                        buyOrders[i] = tempOrder;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}

            //buyOrders.AddRange(sellOrders);

            return buyOrders;
        }

        public   BookOrder MatcherForBuy(BookOrder inputBid)
        {
            BookOrder bid = new BookOrder();
            bid = inputBid;
            if (bid.Matches == null)
            {
                bid.Matches = new SortedList<string, Match>();
            }
        

            for (int i = 0; i <= BookOrders.Count - 1; i++)
            {
                BookOrder tempOrder = new BookOrder();
                tempOrder = BookOrders[i];
                if(tempOrder.Matches==null)
                {
                    tempOrder.Matches = new SortedList<string, Match>();
                }
                if (bid.MatchState == "FullMatch")
                {
                    break ;
                }
                if (!tempOrder.OrderType.Equals(bid.OrderType) && tempOrder.Notional <= bid.Notional)
                {
                   
                    int FinalVolume = tempOrder.Volume - bid.Volume;
                  switch(FinalVolume)
                    {
                        case int n when n >0:
                            tempOrder.Matches.Add(bid.OrderId, new() { OrderId = bid.OrderId, Volume = bid.Volume, Notional = bid.Notional });
                            tempOrder.MatchState = "PartialMatch";
                            tempOrder.Volume = FinalVolume;
                            BookOrders[i]=tempOrder;

                            bid.Matches.Add(bid.OrderId, new() { OrderId = tempOrder.OrderId, Volume = bid.Volume, Notional = bid.Notional });
                            bid.MatchState = "FullMatch";
                            bid.Volume = 0;
                            FullMatchBookOrders.Add(bid);
                            //add Match
                            break;

                        case int n when n < 0:
                            bid.Matches.Add(bid.OrderId, new() { OrderId = tempOrder.OrderId, Volume = tempOrder.Volume, Notional = bid.Notional });
                            bid.MatchState = "PartialMatch";
                            bid.Volume -= tempOrder.Volume;

                            tempOrder.Matches.Add(bid.OrderId, new() { OrderId = bid.OrderId, Volume = tempOrder.Volume, Notional = bid.Notional });
                            tempOrder.MatchState = "Fullmatch";
                            tempOrder.Volume = 0;
                            FullMatchBookOrders.Add(tempOrder);
                            BookOrders.RemoveAt(i);
                            //BookOrders[i] = tempOrder;
                            //add Match
                            break;

                        case int n when n == 0:
                            bid.Matches.Add(bid.OrderId, new() { OrderId = tempOrder.OrderId, Volume = tempOrder.Volume, Notional = bid.Notional });
                            bid.MatchState = "Fullmatch";
                            bid.Volume = 0;
                            FullMatchBookOrders.Add(bid);

                            tempOrder.Matches.Add(bid.OrderId, new() { OrderId = bid.OrderId, Volume = bid.Volume, Notional = bid.Notional });
                            tempOrder.MatchState = "Fullmatch";
                            tempOrder.Volume = 0;
                            FullMatchBookOrders.Add(tempOrder);
                            BookOrders.RemoveAt(i);
                            //BookOrders[i] = tempOrder;
                            //add Match
                            break;
                        default:
                            throw new NotImplementedException(); 
                    };

                }
            }

            return bid;
        }


        public List<BookOrder> ProRata(List<BookOrder> bookOrders)
        {
            throw new NotImplementedException();
        }
    }
}

using CompraVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class AuctionsViewModel
    {
        public AuctionsViewModel()
        {
            MinPrice = double.MinValue;
            MaxPrice = double.MaxValue;
            SearchText = "";
            ACategory = "All";
        }
        public IEnumerable<Auction> Auctions { get; set; }
        public string SearchText { get; set; }
        public string ACategory { get; set; }
        public string State { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}

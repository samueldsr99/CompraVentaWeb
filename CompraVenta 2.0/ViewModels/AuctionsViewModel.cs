using CompraVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class AuctionsViewModel
    {
        public IEnumerable<AuctionViewModel> Auctions { get; set; }
        public string SearchText { get; set; }
        public int Page { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public class Auction
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public string SellerUserName { get; set; }

        public string CurrentOwner { get; set; }

        public string Title { get; set; }

        public double StartPrice { get; set; }

        public double CurrentPrice { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public string Details { get; set; }
    }
}

using CompraVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class ShoppingCarViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<Article> Articles { get; set; }
        public long TotalPrice { get; set; }
    }
}

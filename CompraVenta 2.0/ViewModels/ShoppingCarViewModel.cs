using CompraVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class ShoppingCarViewModel
    {
        public string UserName { get; set; }
        public List<Article> Articles { get; set; }
        public double TotalPrice { get; set; }
    }
}

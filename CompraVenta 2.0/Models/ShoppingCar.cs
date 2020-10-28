using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public class ShoppingCar
    {
        public ShoppingCar()
        {
            this.TotalPrice = 0;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public long TotalPrice { get; set; }
    }
}

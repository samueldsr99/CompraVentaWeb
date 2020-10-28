using CompraVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class RemoveFromCarViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public Article Article { get; set; }
    }
}

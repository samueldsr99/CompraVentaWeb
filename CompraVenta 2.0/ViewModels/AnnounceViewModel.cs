using CompraVenta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class AnnounceViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string SellerId { get; set; }
        public string SellerUserName { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ArticleCategory Category { get; set; }
        [Required]
        [DataType(DataType.Duration)]
        public double Price { get; set; }
        public string Description { get; set; }
    }
}

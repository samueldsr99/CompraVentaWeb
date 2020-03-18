using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public enum ArticleCategory
    {
        Hogar,
        Electronico,
        Vivienda,
        Autos,
        Undefined
    };

    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ArticleCategory Category { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string SellerUserName { get; set; }
    }
}

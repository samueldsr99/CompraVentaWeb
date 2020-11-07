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
        Undefined,
        Vestuario,
        Otro,
        All
    };

    public class Article
    {
        public Article()
        {
            Sold = false;
            Owner = null;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ArticleCategory Category { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string SellerUserName { get; set; }

        public string ImageFilePath { get; set; }

        public bool Sold { get; set; }

        public string Owner { get; set; }
    }
}

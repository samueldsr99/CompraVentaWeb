using CompraVenta.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class AuctionViewModel
    {

        public AuctionViewModel()
        {
            ImageFilePath = "user.png";
        }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Details { get; set; }
        public string SellerUserName { get; set; }
        public string CurrentOwner { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Begin { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        public TimeSpan Time { get; set; }

        [Required]
        [DataType(DataType.Duration)]
        public double StartPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double BidAmount { get; set; }
        [Required]
        public string AName { get; set; }
        public string ACategory { get; set; }

        public IFormFile ImageFile { get; set; }

        public string ImageFilePath { get; set; }

        public ArticleCategory getCategory() => AuctionViewModel.getCategory(ACategory);
        public static ArticleCategory getCategory(string s)
        {
            switch (s)
            {
                case "Hogar":
                    return ArticleCategory.Hogar;
                case "Autos":
                    return ArticleCategory.Autos;
                case "Vivienda":
                    return ArticleCategory.Vivienda;
                case "Electronico":
                    return ArticleCategory.Electronico;
                default:
                    return ArticleCategory.Undefined;
            }
        }
    }
}

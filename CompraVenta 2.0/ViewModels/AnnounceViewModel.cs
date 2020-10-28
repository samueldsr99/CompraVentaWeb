﻿using CompraVenta.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class AnnounceViewModel
    {
        public AnnounceViewModel()
        {
            ImagePath = "user.png";
            ImageFile = null;
        }
        public int Id { get; set; }

        public int ArticleId { get; set; }

        [Required]
        public string Title { get; set; }

        public string SellerId { get; set; }

        public string SellerUserName { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Duration)]
        public double Price { get; set; }

        public string Description { get; set; }

        public bool InCar { get; set; }

        public List<Comment> Comments { get; set; }

        public IFormFile ImageFile { get; set; }

        public string ImagePath { get; set; }

        public ArticleCategory getCategory() => AnnounceViewModel.getCategory(Category);

        /******** Comment Form **************/
        public string CommentFormDescription { get; set; }

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
                case "All":
                    return ArticleCategory.All;
                default:
                    return ArticleCategory.Undefined;
            }
        }
    }
}

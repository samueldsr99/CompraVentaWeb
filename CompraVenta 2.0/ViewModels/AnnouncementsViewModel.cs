using CompraVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class AnnouncementsViewModel
    {
        public AnnouncementsViewModel()
        {
            MinPrice = double.MinValue;
            MaxPrice = double.MaxValue;
            Category = "All";
            SearchText = "";
        }
        public IEnumerable<AnnounceViewModel> Announcements { get; set; }
        public string SearchText{ get; set; }
        public string Category { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }

        public int Page { get; set; }
        public int TotalPages { get; set; }
        public string Seller { get; set; }
        public string State { get; set; }
        public int FirstAnnounceOnPage() => (Page - 1) * 5;
        public int LastAnnounceOnPage() => FirstAnnounceOnPage() + 4;
    }
}

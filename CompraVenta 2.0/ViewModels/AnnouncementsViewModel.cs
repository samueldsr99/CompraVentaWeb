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
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }

        // Unused for now
        public int Page { get; set; }
        public bool IsLastPage()
        {
            return Pages() == Page;
        }
        public int Pages()
        {
            int len = Announcements.Count();
            return len % 5 == 0 ? len / 5 : len / 5 + 1;
        }
        public int FirstAnnounceOnPage() => (Page - 1) * 5;
        public int LastAnnounceOnPage() => FirstAnnounceOnPage() + 4;
    }
}

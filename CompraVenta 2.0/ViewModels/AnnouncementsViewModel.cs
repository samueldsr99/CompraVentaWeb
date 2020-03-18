using CompraVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class AnnouncementsViewModel
    {
        public IEnumerable<AnnounceViewModel> Announcements { get; set; }
        public string SearchText{ get; set; }
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

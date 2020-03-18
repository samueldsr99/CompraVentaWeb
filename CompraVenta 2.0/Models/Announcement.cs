using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public enum AnnouncementFilter
    {
        Title,
        Date,
        SellerUserName,
        ArticleName,
        ArticleCategory,
        ArticlePrice,
    }

    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int ArticleId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime PubDate { get; set; }
        public int AnnouncementId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }

    }
}

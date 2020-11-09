using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompraVenta.Models
{
    public class AppDbContext : IdentityDbContext<Account>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ShoppingCar> ShoppingCar { get; set; }

        public new DbSet<Account> Users { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<Auction> Auctions { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}

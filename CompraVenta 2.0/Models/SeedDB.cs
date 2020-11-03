using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompraVenta.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CompraVenta.Models
{
    public static class SeedDB
    {
        public static void SeedData(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices.GetRequiredService<AppDbContext>();

            context.Database.Migrate();

            SeedArticles(context);
            SeedAnnouncements(context);
            SeedAuctions(context);
        }

        public static void SeedArticles(AppDbContext context)
        {
            var articles = new List<Article> {
                new Article
                {
                    Name = "Lavadora automática",
                    Category = ArticleCategory.Electronico,
                    Description = "Una lavadora para que limpies tu ropa.",
                    Price = 300,
                    SellerUserName = "emilio"
                },
                new Article
                {
                    Name = "Ventilador de pared",
                    Category = ArticleCategory.Electronico,
                    Description = "Un ventilador para que te eches fresco",
                    Price = 40,
                    SellerUserName = "markzuckerberg"
                },
                new Article
                {
                    Name = "Refrigerador Haier",
                    Category = ArticleCategory.Electronico,
                    Description = "Un refrigerador para que enfríes los pomos de agua.",
                    Price = 400,
                    SellerUserName = "everdesia"
                },
                new Article
                {
                    Name = "Lada",
                    Category = ArticleCategory.Autos,
                    Description = "Un lada para que se te peguen jebitas. Está barato para como está en la calle, no te demores que solo me queda uno.",
                    Price = 50000,
                    SellerUserName = "samueldsr"
                },
                new Article
                {
                    Name = "Laptop",
                    Category = ArticleCategory.Electronico,
                    Description = "Una laptop para que la uses, no corre el Call Of Duty pero el notepad corre perfecto. Excelente opción.",
                    Price = 500,
                    SellerUserName = "emilio"
                },
                new Article
                {
                    Name = "Sillones",
                    Category = ArticleCategory.Vivienda,
                    Description = "Par de sillones súper buenos para que te sientes, adiós al dolor de espalda.",
                    Price = 100,
                    SellerUserName = "masin"
                },
                new Article
                {
                    Name = "Casa en la Habana",
                    Category = ArticleCategory.Vivienda,
                    Description = "Se vende tremenda casona en la Habana, Playa. Dirección / 99 & 99. Tiene cocina, baño, cuarto, comedor. "
                    + "Vamos está perfecta. No bajo ni un kilo. Contactar al 99999999. No llames con 99 porque a mi no me recargan de afuera.",
                    Price = 50000,
                    SellerUserName = "markzuckerberg"
                },
                new Article
                {
                    Name = "Mazda G67900",
                    Category = ArticleCategory.Autos,
                    Description = "Un Mazda último modelo de los que no hay, es de segunda mano pues el mismísimo Chacal de la bachata lo usaba."
                    + "Por eso es tan caro. Llamar a Migdalis al número: 55885588",
                    Price = 500000,
                    SellerUserName = "samueldsr"
                },
                new Article
                {
                    Name = "Mesita de noche",
                    Category = ArticleCategory.Hogar,
                    Description = "Mesita de noche lindísima. Si quieren fotos contactar por privado. LLame a 89652364 gracias.",
                    Price = 40,
                    SellerUserName = "everdesia"
                },
                new Article
                {
                    Name = "Mouse Logitech",
                    Category = ArticleCategory.Electronico,
                    Description = "Un mouse para que juegues en la PC.",
                    Price = 15,
                    SellerUserName = "samueldsr"
                },
                new Article
                {
                    Name = "Ropita de bebé",
                    Category = ArticleCategory.Vestuario,
                    Description = "Ropita para tu bebé.",
                    Price = 20,
                    SellerUserName = "magalis"
                },
                new Article
                {
                    Name = "Zapatos de Michael Jackson",
                    Category = ArticleCategory.Vestuario,
                    SellerUserName = "markzuckerberg",
                },
                new Article
                {
                    Name = "Jarrón de oro",
                    Category = ArticleCategory.Hogar,
                    SellerUserName = "samueldsr"
                },
                new Article
                {
                    Name = "Cuadro original de Picasso",
                    Category = ArticleCategory.Hogar,
                    SellerUserName = "everdesia"
                }
            };

            foreach (var article in articles)
            {
                if (context.Articles.Where(e => e.Name.Equals(article.Name) && e.Description.Equals(article.Description)).Count() == 0)
                {
                    context.Articles.Add(article);
                }
            }

            context.SaveChanges();
        }

        public static void SeedAnnouncements(AppDbContext context)
        {
            var announcements = new List<Announcement> {
                new Announcement
                {
                    ArticleId = 1,
                    Title = "***Se vende lavadora automatica***",
                    Date = DateTime.Now - new TimeSpan(50, 0, 0, 0)
                },
                new Announcement
                {
                    ArticleId = 2,
                    Title = "Se vende un ventiladorzaso",
                    Date = DateTime.Now - new TimeSpan(70, 0, 0, 0)
                },
                new Announcement
                {
                    ArticleId = 3,
                    Title = "Ganga: Refrigerador Haier",
                    Date = DateTime.Now - new TimeSpan(10, 10, 2, 1)
                },
                new Announcement
                {
                    ArticleId = 4,
                    Title = "Lada en venta",
                    Date = DateTime.Now - new TimeSpan(11, 10, 2, 2)
                },
                new Announcement
                {
                    ArticleId = 5,
                    Title = "Laptop HP del quinquenio",
                    Date = DateTime.Now - new TimeSpan(2, 0, 0, 0)
                },
                new Announcement
                {
                    ArticleId = 6,
                    Title = "Sillones de Sala, cómpralo que se me acaban.",
                    Date = DateTime.Now - new TimeSpan(7, 0, 0, 0)
                },
                new Announcement
                {
                    ArticleId = 7,
                    Title = "Casón en Playa",
                    Date = DateTime.Now - new TimeSpan(9, 9, 9, 9)
                },
                new Announcement
                {
                    ArticleId = 8,
                    Title = "Carrazo",
                    Date = DateTime.Now - new TimeSpan(1, 1, 1, 1)
                },
                new Announcement
                {
                    ArticleId = 9,
                    Title = "Mesita de noche",
                    Date = DateTime.Now - new TimeSpan(100, 1, 1, 1)
                },
                new Announcement
                {
                    ArticleId = 10,
                    Title = "Ganga: Mouse inalámbrico",
                    Date = DateTime.Now - new TimeSpan(0, 0, 30, 0)
                },
                new Announcement
                {
                    ArticleId = 11,
                    Title = "Compren: Ropita para tu bebé",
                    Date = DateTime.Now - new TimeSpan(0, 1, 0, 7)
                }
            };

            foreach (var announcement in announcements)
            {
                if (context.Announcements.Where(e => e.ArticleId.Equals(announcement.ArticleId)).Count() == 0)
                {
                    context.Announcements.Add(announcement);
                }
            }

            context.SaveChanges();
        }

        public static void SeedAuctions(AppDbContext context)
        {
            var auctions = new List<Auction>
            {
                new Auction
                {
                    ArticleId = 12,
                    Title = "Subasta: Zapatos orisha de Michael Jackson",
                    StartPrice = 7000,
                    CurrentPrice = 7000,
                    Details = "Los zapatos orishas de Michael Jackson, para que hagas el Moon Walk por las noches en la disco.",
                    SellerUserName = "markzuckerberg",
                    Begin = DateTime.Now - new TimeSpan(1, 0, 0, 0),
                    End = DateTime.Now + new TimeSpan(1, 0, 0, 0),
                },
                new Auction
                {
                    ArticleId = 13,
                    Title = "*** Un jarrón de oro chino original ***",
                    StartPrice = 1000,
                    Details = "Un jarrón de oro perteneciente a la dinastía Howasda de China",
                    SellerUserName = "masin",
                    Begin = DateTime.Now - new TimeSpan(10, 0, 0, 0),
                    End = DateTime.Now - new TimeSpan(9, 0, 0, 0),
                    CurrentOwner = "samueldsr",
                    CurrentPrice = 9000,
                },
                new Auction
                {
                    ArticleId = 14,
                    Title = "Subasta: Cuadro de Picasso caballero para tu pared",
                    StartPrice = 60000,
                    CurrentPrice = 60000,
                    Details = "Cuadro de Picasso original: El entierro de Casagemas",
                    SellerUserName = "everdesia",
                    Begin = DateTime.Now + new TimeSpan(0, 0, 10, 55),
                    End = DateTime.Now + new TimeSpan(0, 0, 40, 55),
                }
            };

            foreach (var auction in auctions)
            {
                if (context.Auctions.Where(e => e.ArticleId.Equals(auction.ArticleId)).Count() == 0)
                {
                    context.Auctions.Add(auction);
                }
            }
            context.SaveChanges();
        }
    }
}

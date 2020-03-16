using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly AppDbContext context;
        public AnnouncementController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Announcements()
        {
            List<AnnounceViewModel> model = new List<AnnounceViewModel>();
            foreach (var announce in context.Announcements)
            {
                var article = context.Articles.FirstOrDefault(e => e.Id.Equals(announce.ArticleId));

                var obj = new AnnounceViewModel
                {
                    Id = announce.Id,
                    Title = announce.Title,
                    Date = announce.Date,
                    Name = article.Name,
                    SellerUserName = article.SellerUserName,
                    Category = article.Category,
                    Description = article.Description,
                    Price = article.Price
                };
                obj.SellerId = context.Users.FirstOrDefault(e => e.UserName.Equals(obj.SellerUserName)).Id;
                model.Add(obj);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Announce() => View();

        [HttpPost]
        public IActionResult Announce(AnnounceViewModel model)
        {
            var date = DateTime.Now;
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Name = model.Name,
                    Category = model.Category,
                    Price = model.Price,
                    Description = model.Description,
                    SellerUserName = User.Identity.Name
                };
                context.Articles.Add(article);
                context.SaveChanges();
                var announcement = new Announcement
                {
                    Title = model.Title,
                    Date = date,
                    ArticleId = article.Id,
                };
                context.Announcements.Add(announcement);
                context.SaveChanges();
                return RedirectToAction("Announcements", "Announcement");
            }
            return View(model);
        }

        public IActionResult AnnouncementDetails(int? id)
        {
            var announcement = context.Announcements.FirstOrDefault(e => e.Id.Equals(id));
            var article = context.Articles.FirstOrDefault(e => e.Id.Equals(announcement.ArticleId));
            var obj = new AnnounceViewModel
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Date = announcement.Date,
                Name = article.Name,
                SellerUserName = article.SellerUserName,
                Category = article.Category,
                Description = article.Description,
                Price = article.Price
            };
            return View(obj);
        }
    }
}

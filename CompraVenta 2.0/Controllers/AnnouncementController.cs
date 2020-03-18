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
        public IActionResult Announcements(AnnouncementsViewModel model)
        {
            List<AnnounceViewModel> announcements = toAnnounceViewModel(context.Announcements);

            return View(new AnnouncementsViewModel
            {
                Announcements = announcements,
                Page = model.Page,
                SearchText = model.SearchText
            });
        }

        [HttpPost]
        public IActionResult Search(AnnouncementsViewModel model)
        {
            if (model.SearchText == null || model.SearchText.Length == 0)
            {
                return View("Announcements", new AnnouncementsViewModel
                {
                    Announcements = toAnnounceViewModel(context.Announcements),
                    Page = 1,
                    SearchText = ""
                });
            }
            return View("Announcements", new AnnouncementsViewModel
            {
                Announcements = SearchAnnouncements(context.Announcements, model.SearchText),
                Page = 1,
                SearchText = ""
            });
        }

        [HttpPost]
        public IActionResult Filter(string category)
        {
            return View("Announcements", new AnnouncementsViewModel
            {
                Announcements = FilterBy(toAnnounceViewModel(context.Announcements), AnnounceViewModel.getCategory(category)),
                Page = 1,
                SearchText = ""
            });
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
                    Category = model.getCategory(),
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
                return View("Announcements", new AnnouncementsViewModel
                {
                    Announcements = toAnnounceViewModel(context.Announcements),
                    Page = 1,
                    SearchText = ""
                });
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
                Category = article.Category.ToString(),
                Description = article.Description,
                Price = article.Price
            };
            return View(obj);
        }

        /**************************< Utility Functions >****************************************/

        public List<AnnounceViewModel> toAnnounceViewModel(IEnumerable<Announcement> list)
        {
            List<AnnounceViewModel> ret = new List<AnnounceViewModel>();
            foreach (var announce in list)
            {
                var article = context.Articles.FirstOrDefault(e => e.Id.Equals(announce.ArticleId));

                var obj = new AnnounceViewModel
                {
                    Id = announce.Id,
                    Title = announce.Title,
                    Date = announce.Date,
                    Name = article.Name,
                    SellerUserName = article.SellerUserName,
                    Category = article.Category.ToString(),
                    Description = article.Description,
                    Price = article.Price
                };
                obj.SellerId = context.Users.FirstOrDefault(e => e.UserName.Equals(obj.SellerUserName)).Id;
                ret.Add(obj);
            }
            return ret;
        }

        public IEnumerable<AnnounceViewModel> FilterBy(IEnumerable<AnnounceViewModel> announcements, ArticleCategory category)
        {
            var ret = announcements.Where(e => e.getCategory().Equals(category));
            return ret;
        }

        public List<Announcement> SortBy()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AnnounceViewModel> SearchAnnouncements(IEnumerable<Announcement> announcements, string text)
        {
            List<AnnounceViewModel> ret = new List<AnnounceViewModel>();
            foreach (var announcement in announcements)
            {
                var article = context.Articles.FirstOrDefault(e => e.Id.Equals(announcement.ArticleId));
                var view_model = new AnnounceViewModel
                {
                    Id = announcement.Id,
                    Date = announcement.Date,
                    Title = announcement.Title,
                    SellerUserName = article.SellerUserName,
                    Name = article.Name,
                    Category = article.Category.ToString(),
                    Description = article.Description,
                    Price = article.Price
                };
                bool ok = false;
                ok |= (view_model.Title != null && view_model.Title.Contains(text, StringComparison.CurrentCultureIgnoreCase));
                ok |= (view_model.SellerUserName != null && view_model.SellerUserName.Contains(text, StringComparison.CurrentCultureIgnoreCase));
                ok |= (view_model.Name != null && view_model.Name.Contains(text, StringComparison.CurrentCultureIgnoreCase));
                ok |= (view_model.Description != null && view_model.Description.Contains(text, StringComparison.CurrentCultureIgnoreCase));

                if (ok)
                {
                    ret.Add(view_model);
                }
            }
            return ret;
        }
    }
}

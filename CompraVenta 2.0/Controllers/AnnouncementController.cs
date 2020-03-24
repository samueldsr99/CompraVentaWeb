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
        [HttpGet]
        public IActionResult Announcements(AnnouncementsViewModel model)
        {
            List<AnnounceViewModel> announcements = ToAnnounceViewModel(context.Announcements);

            return View(new AnnouncementsViewModel
            {
                Announcements = announcements,
                Page = model.Page,
                SearchText = model.SearchText
            });
        }
        [HttpPost]
        public IActionResult Filter(AnnouncementsViewModel model)
        {
            var list = ToAnnounceViewModel(context.Announcements);
            var filter = list.AsEnumerable();
            if (AnnounceViewModel.getCategory(model.Category) != ArticleCategory.All)
                filter = FilterByCategory(list, AnnounceViewModel.getCategory(model.Category));
            if (model.SearchText != null && model.SearchText != "")
                filter = FilterByText(filter, model.SearchText);
            filter = FilterByPrice(filter, model.MinPrice, model.MaxPrice);
            return View("Announcements", new AnnouncementsViewModel
            {
                Announcements = filter,
                SearchText = model.SearchText,
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                Category = model.Category
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
                    Announcements = ToAnnounceViewModel(context.Announcements),
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

        public List<AnnounceViewModel> ToAnnounceViewModel(IEnumerable<Announcement> list)
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

        public IEnumerable<AnnounceViewModel> FilterByCategory(IEnumerable<AnnounceViewModel> announcements, ArticleCategory category)
        {
            var ret = announcements.Where(e => e.getCategory() == category);
            return ret;
        }
        public IEnumerable<AnnounceViewModel> FilterByText(IEnumerable<AnnounceViewModel> announcements, string SearchText)
        {
            return announcements.Where(e => 
                e.Description.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                e.Category.ToString().Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                e.Name.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                e.SellerUserName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                e.Title.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)
            );
        }
        public IEnumerable<AnnounceViewModel> FilterByPrice(IEnumerable<AnnounceViewModel> announcements, double MinPrice, double MaxPrice)
        {
            return announcements.Where(e => 
                e.Price >= MinPrice && e.Price <= MaxPrice
            );
        }

        public List<Announcement> SortBy()
        {
            throw new NotImplementedException();
        }
    }
}

using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Controllers
{
    [Authorize]
    public class AnnouncementController : Controller
    {
        private readonly AppDbContext context;
        public AnnouncementController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Announcements(double? maxPrice, double? minPrice, string searchText = "", string category = "", int page = 1)
        {
            var announcements = from announcement in context.Announcements
                                join article in context.Articles on announcement.ArticleId equals article.Id
                                select new AnnounceViewModel
                                {
                                    Id = announcement.Id,
                                    ArticleId = article.Id,
                                    Title = announcement.Title,
                                    SellerUserName = article.SellerUserName,
                                    Date = announcement.Date,
                                    Name = article.Name,
                                    Category = article.Category.ToString(),
                                    Price = article.Price,
                                    Description = article.Description
                                };
            // Filter by category
            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                announcements = announcements.Where(e => e.getCategory() == AnnounceViewModel.getCategory(category));
            }
            // Filter by price
            if (minPrice != null)
            {
                announcements = announcements.Where(e => e.Price >= minPrice);
            }
            if (maxPrice != null)
            {
                announcements = announcements.Where(e => e.Price <= maxPrice);
            }
            // Filter by text
            if (!string.IsNullOrEmpty(searchText))
            {
                announcements = announcements.Where(e => 
                                                    e.Description.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                                                    || e.Title.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                                                    || e.SellerUserName.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                                                    );
            }

            int amount = announcements.Count();

            announcements = announcements.OrderByDescending(e => e.Date).Skip(4 * (page - 1)).Take(4);

            return View(new AnnouncementsViewModel
            {
                Announcements = announcements,
                Page = page,
                SearchText = searchText,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Category = category,
                TotalPages = (int)Math.Ceiling((decimal)amount / 4),
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

                string uniqueFileName = null;
                if (model.ImageFile != null)
                {
                    string uploadsFolder = "./wwwroot/images/";
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(fs);
                    }
                }

                var article = new Article
                {
                    Name = model.Name,
                    Category = model.getCategory(),
                    Price = model.Price,
                    Description = model.Description,
                    SellerUserName = User.Identity.Name,
                    ImageFilePath = uniqueFileName,
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
                return RedirectToAction("Announcements");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AnnouncementDetails(int? id)
        {
            var announcement = context.Announcements.FirstOrDefault(e => e.Id.Equals(id));
            var article_ = context.Articles.FirstOrDefault(e => e.Id.Equals(announcement.ArticleId));
            var comments = context.Comments.Where(e => e.AnnouncementId.Equals(id)).OrderByDescending(e => e.PubDate);

            var userArticles = from userArticle in context.UserArticle
                               join article in context.Articles on userArticle.ArticleId equals article.Id
                               select new
                               {
                                   article.Id,
                                   article.Name,
                                   article.Price,
                                   article.Category,
                                   userArticle.UserName,
                                   article.SellerUserName
                               };
            userArticles = userArticles.Where(e => e.UserName.Equals(User.Identity.Name));

            bool inCar = userArticles.FirstOrDefault(e => e.Id.Equals(id)) != null;

            var obj = new AnnounceViewModel
            {
                Id = announcement.Id,
                ArticleId = article_.Id,
                Title = announcement.Title,
                Date = announcement.Date,
                Name = article_.Name,
                SellerUserName = article_.SellerUserName,
                Category = article_.Category.ToString(),
                Description = article_.Description,
                Price = article_.Price,
                Comments = comments.ToList(),
                ImagePath = article_.ImageFilePath,
                InCar = inCar
            };
            return View(obj);
        }

        [HttpPost]
        public IActionResult AnnouncementDetails(AnnounceViewModel model)
        {
            var date = DateTime.Now;
            var comment = new Comment
            {
                Description = model.CommentFormDescription,
                UserId = User.Identity.Name,
                PubDate = date,
                AnnouncementId = model.Id,
            };
            context.Comments.Add(comment);
            context.SaveChanges();
            return RedirectToAction("AnnouncementDetails", model.Id);
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

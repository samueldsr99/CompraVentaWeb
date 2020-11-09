using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Controllers
{
    [Authorize(Roles = "Client,Admin")]
    public class AnnouncementController : Controller
    {
        private readonly AppDbContext context;
        private readonly IHostingEnvironment hostingEnvironment;

        public AnnouncementController(AppDbContext context,
                                        IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Announcements(double? maxPrice, double? minPrice, string searchText = "", string category = "", int page = 1, string seller = "", string state = "All")
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
                                    Description = article.Description,
                                    Sold = article.Sold,
                                    Owner = article.Owner,
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
            if (!string.IsNullOrEmpty(seller))
            {
                announcements = announcements.Where(e => e.SellerUserName.Equals(seller));
            }
            if (!User.IsInRole("Admin"))
            {
                announcements = announcements.Where(e => !e.Sold);

            }
            else
            {
                if (state.ToLower() == "free")
                {
                    announcements = announcements.Where(e => !e.Sold);
                }
                else if (state.ToLower() == "sold")
                {
                    announcements = announcements.Where(e => e.Sold);
                }
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
                Seller = seller,
                State = state
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
                    uniqueFileName = Utils.FileProcess.UploadFile(model.ImageFile, hostingEnvironment);
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

            var userArticles = from userArticle in context.ShoppingCar
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
                InCar = inCar,
                Sold = article_.Sold,
                Owner = article_.Owner
            };
            return View(obj);
        }

        [HttpPost]
        public IActionResult Remove(int id, int articleId)
        {
            var announcement = context.Announcements.FirstOrDefault(e => e.Id.Equals(id));
            var article = context.Articles.FirstOrDefault(e => e.Id.Equals(articleId));

            if (announcement == null || article == null)
            {
                return RedirectToAction("NotFound");
            }

            context.Announcements.Remove(announcement);
            context.Articles.Remove(article);
            context.SaveChanges();

            return RedirectToAction("Announcements");
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
    }
}

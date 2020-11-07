using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompraVenta.Controllers
{
    [Authorize(Roles = "Admin,Client")]
    public class ShoppingCarController : Controller
    {
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;
        private readonly AppDbContext context;

        public ShoppingCarController(UserManager<Account> userManager,
                                SignInManager<Account> signInManager,
                                AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Details(string username)
        {
            var shoppingCar = context.ShoppingCar.FirstOrDefault(e => e.UserName.Equals(username));
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
            userArticles = userArticles.Where(e => e.UserName.Equals(username));

            List<Article> articles = new List<Article>();

            foreach (var article in userArticles)
            {
                articles.Add(new Article
                {
                    Id = article.Id,
                    Category = article.Category,
                    Name = article.Name,
                    Price = article.Price,
                    SellerUserName = article.SellerUserName
                });
            }

            if (shoppingCar == null)
            {
                return View("NotFound");
            }

            return View(new ShoppingCarViewModel
            {
                Id = shoppingCar.Id,
                UserName = shoppingCar.UserName,
                TotalPrice = shoppingCar.TotalPrice,
                Articles = articles
            });
        }

        [HttpPost]
        public IActionResult AddToCar(int articleId, string username)
        {
            // check if item is in car
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
            userArticles = userArticles.Where(e => e.UserName.Equals(username));

            if (userArticles.FirstOrDefault(e => e.Id.Equals(articleId)) != null)
            {
                return View("NotFound");
            }

            var shoppingCar = context.ShoppingCar.FirstOrDefault(e => e.UserName.Equals(username));
            var article_ = context.Articles.FirstOrDefault(e => e.Id.Equals(articleId));

            if (shoppingCar == null || article_ == null)
            {
                return View("NotFound");
            }

            context.UserArticle.Add(new UserArticle
            {
                ArticleId = articleId,
                UserName = username
            });

            shoppingCar.TotalPrice += (long)article_.Price;

            context.SaveChanges();

            return RedirectToAction("Details", new { username });
        }

        [HttpPost]
        public IActionResult RemoveFromCar(int? articleId)
        {
            string username = User.Identity.Name;
            var result = context.UserArticle.FirstOrDefault(e => e.ArticleId.Equals(articleId) && e.UserName.Equals(username));
            var shoppingCar = context.ShoppingCar.FirstOrDefault(e => e.UserName.Equals(username));
            var article = context.Articles.FirstOrDefault(e => e.Id.Equals(articleId));

            if (articleId == null || result == null || shoppingCar == null)
            {
                return View("NotFound");
            }

            context.UserArticle.Remove(result);
            shoppingCar.TotalPrice -= (long)article.Price;

            context.SaveChanges();

            return RedirectToAction("Details", new { username = username });
        }

        [HttpGet]
        public IActionResult BuyAll()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuyAll(string username)
        {
            var query = from userArticle in context.UserArticle
                               join article in context.Articles on userArticle.ArticleId equals article.Id
                               select new
                               {
                                   article.Id,
                                   article.Name,
                                   article.Price,
                                   article.Category,
                                   userArticle.UserName,
                                   article.SellerUserName,
                                   article.Sold,
                                   article.Owner
                               };

            query = query.Where(e => e.UserName.Equals(username));

            List<int> Ids = new List<int>();

            query.ToList().ForEach(e => Ids.Add(e.Id));

            var articles = context.Articles.Where(e => Ids.Contains(e.Id));

            foreach (var article in articles)
            {
                article.Sold = true;
                article.Owner = username;
            }

            var toRemove = context.UserArticle.Where(e => e.UserName.Equals(username));

            var shoppingCar = context.ShoppingCar.FirstOrDefault(e => e.UserName.Equals(username));
            shoppingCar.TotalPrice = 0;

            context.UserArticle.RemoveRange(toRemove);

            context.SaveChanges();

            return RedirectToAction("Details", new { username });
        }
    }
}

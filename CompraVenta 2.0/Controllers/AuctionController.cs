using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace CompraVenta.Controllers
{
    [Authorize(Roles = "Client,Admin")]
    public class AuctionController : Controller
    {
        private readonly AppDbContext context;
        public AuctionController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Auction()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Auction(AuctionViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Begin.CompareTo(model.End) == 1)
                {
                    ModelState.AddModelError("Fecha", "La fecha de inicio no puede ser mayor que la fecha de fin.");
                    return View(model);
                }
                if (model.End.Subtract(model.Begin) < new TimeSpan(0, 0, 10, 0))
                {
                    ModelState.AddModelError("Fecha", "La duración de la subasta debe ser mayor o igual a 10 minutos.");
                    return View(model);
                }

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

                Article article = new Article
                {
                    Category = AuctionViewModel.getCategory(model.ACategory),
                    Name = model.AName,
                    SellerUserName = User.Identity.Name,
                    ImageFilePath = uniqueFileName
                };
                context.Articles.Add(article);
                context.SaveChanges();

                var auction = new Auction
                {
                    Title = model.Title,
                    ArticleId = article.Id,
                    StartPrice = model.StartPrice,
                    CurrentPrice = model.StartPrice,
                    Details = model.Details,
                    Begin = model.Begin,
                    End = model.End,
                    SellerUserName = User.Identity.Name,
                };

                context.Auctions.Add(auction);
                context.SaveChanges();

                return RedirectToAction("Auctions", "Auction");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Auctions(double? minPrice, double? maxPrice, string searchText="", string aCategory="All", string state="", int page=1)
        {
            var auctions = from article in context.Articles
                                           join auction in context.Auctions on article.Id equals auction.ArticleId
                                           select new
                                           {
                                               auction.Id,
                                               article.Name,
                                               article.Category,
                                               auction.Title,
                                               auction.Details,
                                               auction.SellerUserName,
                                               auction.CurrentPrice,
                                               auction.Begin,
                                               auction.End,
                                               auction.ArticleId
                                           };

            if (minPrice != null)
            {
                auctions = auctions.Where(e => minPrice <= e.CurrentPrice);
            }
            if (maxPrice != null)
            {
                auctions = auctions.Where(e => e.CurrentPrice <= maxPrice);
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                auctions = auctions
                    .Where(e => e.Title.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                          || e.Details.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                          || e.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                          || e.Category.ToString().Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                          || e.SellerUserName.ToString().Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                    );
            }
            if (!string.IsNullOrEmpty(aCategory) && aCategory.ToLower() != "all")
            {
                auctions = auctions.Where(e => AnnounceViewModel.getCategory(aCategory) == e.Category);
            }
            if (!string.IsNullOrEmpty(state) && state.ToLower() != "all")
            {
                DateTime now = DateTime.Now;

                if (state.ToLower() == "closed")
                {
                    auctions = auctions.Where(e => e.End.CompareTo(now) == -1);
                }
                if (state.ToLower() == "coming")
                {
                    auctions = auctions.Where(e => e.Begin.CompareTo(now) == 1);
                }
                if (state.ToLower() == "running")
                {
                    auctions = auctions.Where(e => e.Begin.CompareTo(now) <= 0 && e.End.CompareTo(now) >= 0);
                }
            }

            int amount = auctions.Count();

            auctions = auctions.OrderByDescending(e => e.Begin).Skip(4 * (page - 1)).Take(4);

            List<Auction> listAuctions = new List<Auction>();

            foreach (var auction in auctions)
            {
                listAuctions.Add(new Auction
                {
                    Id = auction.Id,
                    ArticleId = auction.ArticleId,
                    Title = auction.Title,
                    Details = auction.Details,
                    SellerUserName = auction.SellerUserName,
                    CurrentPrice = auction.CurrentPrice,
                    Begin = auction.Begin,
                    End = auction.End
                });
            }

            return View(new AuctionsViewModel
            {
                Auctions = listAuctions,
                Page = page,
                SearchText = searchText,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                ACategory = aCategory,
                State = state,
                TotalPages = (int)Math.Ceiling((decimal)amount / 4)
            });
        }

        [HttpGet]
        public IActionResult BuyProduct(int articleId, string username)
        {
            return View(new BuyProductViewModel
            {
                ArticleId = articleId,
                Username = username
            });
        }

        [HttpPost]
        public IActionResult BuyProduct(BuyProductViewModel model)
        {
            var auction = context.Auctions.FirstOrDefault(e => e.ArticleId.Equals(model.ArticleId));
            var article = context.Articles.FirstOrDefault(e => e.Id.Equals(auction.ArticleId));

            if (auction == null || model.Username != auction.CurrentOwner || article == null)
            {
                return View("NotFound");
            }

            article.Sold = true;
            context.SaveChanges();

            return RedirectToAction("Auctions", "Auction");
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            var auction = context.Auctions.FirstOrDefault(e => e.Id.Equals(id));

            if (auction == null)
            {
                return View("NotFound");
            }

            return View(ToAuctionViewModel(auction));
        }

        [HttpPost]
        public IActionResult Details(AuctionViewModel model)
        {
            var auction = context.Auctions.FirstOrDefault(e => e.Id.Equals(model.Id));
            if (model.BidAmount <= auction.CurrentPrice)
            {
                return RedirectToAction("Details", model.Id);
            }
            else
            {
                auction.CurrentPrice = model.BidAmount;
                auction.CurrentOwner = User.Identity.Name;
                context.Update(auction);
                context.SaveChanges();
            }
            return RedirectToAction("Details", model.Id);
        }

        /**************************< Utilities >****************************************/

        private AuctionViewModel ToAuctionViewModel(Auction auction)
        {
            var article = context.Articles.FirstOrDefault(e => e.Id.Equals(auction.ArticleId));

            if (article == null)
            {
                return null;
            }

            return new AuctionViewModel
            {
                ArticleId = article.Id,
                ImageFilePath = article.ImageFilePath,
                CurrentPrice = auction.CurrentPrice,
                CurrentOwner = auction.CurrentOwner,
                Id = auction.Id,
                Title = auction.Title,
                ACategory = article.Category.ToString(),
                StartPrice = auction.StartPrice,
                Details = auction.Details,
                AName = article.Name,
                Begin = auction.Begin,
                End = auction.End,
                SellerUserName = auction.SellerUserName,
                Sold = article.Sold
            };
        }        
    }
}

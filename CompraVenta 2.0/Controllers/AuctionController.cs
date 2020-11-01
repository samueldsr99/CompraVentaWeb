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
    [Authorize]
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

                var auction = new Auction
                {
                    Title = model.Title,
                    ACategory = AuctionViewModel.getCategory(model.ACategory),
                    StartPrice = model.StartPrice,
                    CurrentPrice = model.StartPrice,
                    Details = model.Details,
                    AName = model.AName,
                    Begin = model.Begin,
                    End = model.End,
                    SellerUserName = User.Identity.Name,
                    ImageFilePath = uniqueFileName
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
            IQueryable<Auction> auctions = context.Auctions;

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
                          || e.AName.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                          || e.ACategory.ToString().Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                          || e.SellerUserName.ToString().Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
                    );
            }
            if (!string.IsNullOrEmpty(aCategory) && aCategory.ToLower() != "all")
            {
                auctions = auctions.Where(e => AnnounceViewModel.getCategory(aCategory) == e.ACategory);
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

            return View(new AuctionsViewModel
            {
                Auctions = auctions.ToList(),
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

        /**************************< Utility Functions >****************************************/

        private AuctionViewModel ToAuctionViewModel(Auction auction)
        {
            return new AuctionViewModel
            {
                CurrentPrice = auction.CurrentPrice,
                CurrentOwner = auction.CurrentOwner,
                Id = auction.Id,
                Title = auction.Title,
                ACategory = auction.ACategory.ToString(),
                StartPrice = auction.StartPrice,
                Details = auction.Details,
                AName = auction.AName,
                Begin = auction.Begin,
                End = auction.End,
                ImageFilePath = auction.ImageFilePath,
                SellerUserName = auction.SellerUserName
            };
        }        
    }
}

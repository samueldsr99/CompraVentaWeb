using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CompraVenta.Controllers
{
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
                };

                if (model.Begin.CompareTo(model.End) == 1)
                {
                    ModelState.AddModelError("Fecha", "La fecha de inicio no puede ser mayor que la fecha de fin.");
                    return View(model);
                }

                context.Auctions.Add(auction);
                context.SaveChanges();

                return RedirectToAction("Auctions", "Auction");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Auctions()
        {
            List<AuctionViewModel> auctions = ToAuctionViewModel(context.Auctions);
            return View(new AuctionsViewModel
            {
                Auctions = auctions,
                Page = 1,
                SearchText = ""
            });
        }
        [HttpPost]
        public IActionResult Filter(AuctionsViewModel model)
        {
            var list = ToAuctionViewModel(context.Auctions);
            var filter = list.AsEnumerable();
            if (AnnounceViewModel.getCategory(model.ACategory) != ArticleCategory.All)
                filter = FilterByCategory(list, AnnounceViewModel.getCategory(model.ACategory));
            if (model.SearchText != null && model.SearchText != "")
                filter = FilterByText(filter, model.SearchText);
            filter = FilterByPrice(filter, model.MinPrice, model.MaxPrice);
            if (model.State != "All")
                filter = FilterByState(filter, model.State);
            return View("Auctions", new AuctionsViewModel
            {
                Auctions = filter,
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                SearchText = model.SearchText,
            });
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            var auction = context.Auctions.FirstOrDefault(e => e.Id.Equals(id));
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
                SellerUserName = auction.SellerUserName
            };
        }
        private List<AuctionViewModel> ToAuctionViewModel(IEnumerable<Auction> list)
        {
            List<AuctionViewModel> ret = new List<AuctionViewModel>();
            foreach (var auction in list)
            {
                ret.Add(new AuctionViewModel
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
                    SellerUserName = auction.SellerUserName
                });
            }
            return ret;
        }

        public IEnumerable<AuctionViewModel> FilterByCategory(IEnumerable<AuctionViewModel> auctions, ArticleCategory category)
        {
            var ret = auctions.Where(e => e.getCategory() == category);
            return ret;
        }
        public IEnumerable<AuctionViewModel> FilterByText(IEnumerable<AuctionViewModel> auctions, string SearchText)
        {
            return auctions.Where(e =>
                e.Details.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                e.ACategory.ToString().Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                e.AName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                e.SellerUserName.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase) ||
                e.Title.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase)
            );
        }
        public IEnumerable<AuctionViewModel> FilterByPrice(IEnumerable<AuctionViewModel> auctions, double MinPrice, double MaxPrice)
        {
            return auctions.Where(e =>
                e.CurrentPrice >= MinPrice && e.CurrentPrice <= MaxPrice
            );
        }

        public IEnumerable<AuctionViewModel> FilterByState(IEnumerable<AuctionViewModel> auctions, string State)
        {
            if (State == "All") return auctions;

            return auctions.Where(e =>
            {
                string state = "Running";
                TimeSpan r = e.End.Subtract(DateTime.Now);
                if (e.Begin.CompareTo(DateTime.Now) == 1) state = "Coming";
                else if (e.End.CompareTo(DateTime.Now) == -1) state = "Closed";
                return state == State;
            });
        }
    }
}

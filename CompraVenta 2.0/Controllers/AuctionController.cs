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
    }
}

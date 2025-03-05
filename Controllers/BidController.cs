using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CardHaven.Data;
using CardHaven.Models;
using Microsoft.AspNetCore.Identity;

namespace CardHaven.Controllers
{
    public class BidController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUserModel> _userManager;

        public BidController(ApplicationDbContext context, UserManager<ApplicationUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bid
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bids.Include(b => b.Auction).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bid/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidModel = await _context.Bids
                .Include(b => b.Auction)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bidModel == null)
            {
                return NotFound();
            }

            return View(bidModel);
        }

        // GET: Bid/Create
        public IActionResult Create()
        {
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Bid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuctionId, Amount")] BidModel bidModel)
        {
            
            if (ModelState.IsValid)
            {
            //hämta inloggad användare
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            //letar efter första auktionen. Hittas ingen returneras null
            var auction = await _context.Auctions.FirstOrDefaultAsync(auction => auction.Id == bidModel.AuctionId);
            if (auction == null)
            {
                return NotFound();
            }

            //är budet högre än aktuellt bud, om inte skriv ut felmeddelande?
            if(bidModel.Amount <= auction.AskingPrice)
            {
                TempData["ErrorMessage"] = "Budet måste vara högre än aktuellt bud";
                
                return RedirectToAction("Details", "Auction", new { id = bidModel.AuctionId });
            }


            //den som ska bjuda sätts till användarens id
            bidModel.UserId = user.Id;

            //tiden för när budet läggs
            bidModel.PlacedAt = DateTime.Now;

            //lägg till budet i databasen
            _context.Bids.Add(bidModel);

            //uppdatera auktionens aktuella bud
            auction.AskingPrice = bidModel.Amount;
            _context.Auctions.Update(auction);

            //spara ändringar
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Budet är accepterat.";

            //användaren skickas till Details för auction med hjälp av auktionens id
                return RedirectToAction("Details", "Auction", new { id = bidModel.AuctionId });
            }
            
            return View(bidModel);
        }

        // GET: Bid/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidModel = await _context.Bids.FindAsync(id);
            if (bidModel == null)
            {
                return NotFound();
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id", bidModel.AuctionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bidModel.UserId);
            return View(bidModel);
        }

        // POST: Bid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuctionId,UserId,Amount,PlacedAt")] BidModel bidModel)
        {
            if (id != bidModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bidModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidModelExists(bidModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "Id", "Id", bidModel.AuctionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bidModel.UserId);
            return View(bidModel);
        }

        // GET: Bid/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bidModel = await _context.Bids
                .Include(b => b.Auction)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bidModel == null)
            {
                return NotFound();
            }

            return View(bidModel);
        }

        // POST: Bid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bidModel = await _context.Bids.FindAsync(id);
            if (bidModel != null)
            {
                _context.Bids.Remove(bidModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BidModelExists(int id)
        {
            return _context.Bids.Any(e => e.Id == id);
        }
    }
}

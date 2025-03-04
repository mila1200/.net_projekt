using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CardHaven.Data;
using CardHaven.Models;

namespace CardHaven.Controllers
{
    public class AuctionController : Controller
    {
        private readonly ApplicationDbContext _context;
        //läser ut sökvägarna till filsystemet för att kunna lägga bilderna i foldern "Images"
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string wwwRootPath;

        public AuctionController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwRootPath = hostEnvironment.WebRootPath;
        }

        // GET: Auction
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Auctions.Include(a => a.Seller);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Auction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auctionModel = await _context.Auctions
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auctionModel == null)
            {
                return NotFound();
            }

            return View(auctionModel);
        }

        // GET: Auction/Create
        public IActionResult Create()
        {
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Auction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Set,Description,Condition,ImageFile,SellerId,AskingPrice,StartTime,EndTime")] AuctionModel auctionModel)
        {
            if (ModelState.IsValid)
            {
                //kolla efter bild
                if (auctionModel.ImageFile != null)
                {
                    //generera unikt filnamn
                    string fileName = Path.GetFileNameWithoutExtension(auctionModel.ImageFile.FileName);
                    string extension = Path.GetExtension(auctionModel.ImageFile.FileName);

                    auctionModel.ImageName = fileName = fileName.Replace(" ", String.Empty) + DateTime.Now.ToString("yymmssfff") + extension;

                    string path = Path.Combine(wwwRootPath + "/images", fileName);

                    //spara i filsystemet
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await auctionModel.ImageFile.CopyToAsync(fileStream);
                    }
                }
                //standardbild om ingen läggs till
                else
                {
                    auctionModel.ImageName = "empty.jpg";
                }

                _context.Add(auctionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", auctionModel.SellerId);
            return View(auctionModel);
        }

        // GET: Auction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auctionModel = await _context.Auctions.FindAsync(id);
            if (auctionModel == null)
            {
                return NotFound();
            }
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", auctionModel.SellerId);
            return View(auctionModel);
        }

        // POST: Auction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Set,Description,Condition,ImageName,SellerId,AskingPrice,StartTime,EndTime")] AuctionModel auctionModel)
        {
            if (id != auctionModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auctionModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionModelExists(auctionModel.Id))
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
            ViewData["SellerId"] = new SelectList(_context.Users, "Id", "Id", auctionModel.SellerId);
            return View(auctionModel);
        }

        // GET: Auction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auctionModel = await _context.Auctions
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auctionModel == null)
            {
                return NotFound();
            }

            return View(auctionModel);
        }

        // POST: Auction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auctionModel = await _context.Auctions.FindAsync(id);
            if (auctionModel != null)
            {
                _context.Auctions.Remove(auctionModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuctionModelExists(int id)
        {
            return _context.Auctions.Any(e => e.Id == id);
        }
    }
}

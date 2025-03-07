using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CardHaven.Data;
using CardHaven.Models;
using Microsoft.Net.Http.Headers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;


namespace CardHaven.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        private readonly ApplicationDbContext _context;
        //läser ut sökvägarna till filsystemet för att kunna lägga bilderna i foldern "Images"
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string wwwRootPath;
        //för att hämta inloggad användare
        private readonly UserManager<ApplicationUserModel> _userManager;

        public AuctionController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUserModel> userManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwRootPath = hostEnvironment.WebRootPath;
            _userManager = userManager;
        }

        // GET: Auction
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Auctions
            .Include(a => a.Seller)
            .Include(a => a.Bids)
            .OrderByDescending(a => a.EndTime);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Auction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //skicka bud och användare till vyn
            var auctionModel = await _context.Auctions
                .Include(a => a.Seller)
                .Include(a => a.Bids)
                .ThenInclude(b => b.User)
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
        public async Task<IActionResult> Create([Bind("Id,Name,Set,Description,Condition,ImageFile,AskingPrice,EndTime")] AuctionModel auctionModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //sätt sellerId till användarens ID
            auctionModel.SellerId = user.Id;

            //sätt starttime till start av annonsen
            auctionModel.StartTime = DateTime.Now;

            //tidsintervall
            DateTime minEndTime = DateTime.Now.AddDays(1);
            DateTime maxEndTime = DateTime.Now.AddDays(7);

            //kontroll av tidsintervall
            if (auctionModel.EndTime < minEndTime || auctionModel.EndTime > maxEndTime)
            {
                ModelState.AddModelError("EndTime", "Sluttiden måste vara mellan 1 dag och 7 dagar framåt");
            }

            //nullkontroll. Felmeddelande om ingen bild bifogas
            if (auctionModel.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "En bild måste bifogas.");
                return View(auctionModel);
            }

            if (ModelState.IsValid)
            {
                // Skapa unika sökvägar
                string fileName = Path.GetFileNameWithoutExtension(auctionModel.ImageFile.FileName).Replace(" ", String.Empty);
                string extension = Path.GetExtension(auctionModel.ImageFile.FileName);

                //Tidsstämpel
                string timestamp = DateTime.Now.ToString("yymmssfff");

                // Skapa filnamn för orignalbild och webp
                string originalFileName = fileName + timestamp + extension;
                string webpFileName = fileName + timestamp + ".webp";

                string originalPath = Path.Combine(wwwRootPath, "images", originalFileName);
                string webpPath = Path.Combine(wwwRootPath, "images", webpFileName);

                // Spara originalbilden
                using (var fileStream = new FileStream(originalPath, FileMode.Create))
                {
                    await auctionModel.ImageFile.CopyToAsync(fileStream);
                }

                // Konvertera och spara WebP
                using (var image = Image.Load(auctionModel.ImageFile.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(800, 600)
                    }));

                    //kvalitet 80
                    await image.SaveAsync(webpPath, new WebpEncoder() { Quality = 80 });
                }

                //spara webp i databasen
                auctionModel.ImageName = webpFileName;

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Set,Description,Condition,ImageName,SellerId,AskingPrice,StartTime,EndTime,ImageFile")] AuctionModel auctionModel)
        {
            if (id != auctionModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Om ingen ny bild är vald, behåll den gamla bilden
                    if (auctionModel.ImageFile == null)
                    {
                        // AsNotracking för att unvika spårning av två objekt. För att undvika felmeddelande
                        var existingAuction = await _context.Auctions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                        if (existingAuction != null)
                        {   //behåll gamla bilden
                            auctionModel.ImageName = existingAuction.ImageName; 
                        }
                    }
                    else
                    {
                        // Skapa sökvägar för originalbild och webp
                        string fileName = Path.GetFileNameWithoutExtension(auctionModel.ImageFile.FileName).Replace(" ", String.Empty);
                        string extension = Path.GetExtension(auctionModel.ImageFile.FileName);

                        // Tidsstämpel
                        string timestamp = DateTime.Now.ToString("yymmssfff");

                        // Skapa filnamn för originalbild och webp
                        string originalFileName = fileName + timestamp + extension;
                        string webpFileName = fileName + timestamp + ".webp";

                        string originalPath = Path.Combine(_hostEnvironment.WebRootPath, "images", originalFileName);
                        string webpPath = Path.Combine(_hostEnvironment.WebRootPath, "images", webpFileName);

                        // Spara originalbilden
                        using (var fileStream = new FileStream(originalPath, FileMode.Create))
                        {
                            await auctionModel.ImageFile.CopyToAsync(fileStream);
                        }

                        // Konvertera och spara WebP
                        using (var image = Image.Load(auctionModel.ImageFile.OpenReadStream()))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Max,
                                Size = new Size(800, 600)
                            }));

                            // Kvalitet 80 för webp
                            await image.SaveAsync(webpPath, new WebpEncoder() { Quality = 80 });
                        }

                        // Uppdatera ImageName till webp-filnamnet
                        auctionModel.ImageName = webpFileName;
                    }

                    // Uppdatera auktionen i databasen
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

        //hämtar auktioner som skapats av specifik användare
        public async Task<IActionResult> UserAuctions()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                RedirectToAction("Login", "Account");
            }

            var myAuctions = _context.Auctions.Where(auction => auction.SellerId == user.Id).ToList();
            return View(myAuctions);
        }
    }
}

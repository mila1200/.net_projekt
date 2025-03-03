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
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Card
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cards.ToListAsync());
        }

        // GET: Card/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardModel = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardModel == null)
            {
                return NotFound();
            }

            return View(cardModel);
        }

        // GET: Card/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Card/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Set,Condition,Description,ImageName")] CardModel cardModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cardModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cardModel);
        }

        // GET: Card/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardModel = await _context.Cards.FindAsync(id);
            if (cardModel == null)
            {
                return NotFound();
            }
            return View(cardModel);
        }

        // POST: Card/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Set,Condition,Description,ImageName")] CardModel cardModel)
        {
            if (id != cardModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardModelExists(cardModel.Id))
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
            return View(cardModel);
        }

        // GET: Card/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardModel = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardModel == null)
            {
                return NotFound();
            }

            return View(cardModel);
        }

        // POST: Card/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cardModel = await _context.Cards.FindAsync(id);
            if (cardModel != null)
            {
                _context.Cards.Remove(cardModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardModelExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CardHaven.Models;
using CardHaven.Data;
using Microsoft.EntityFrameworkCore;

namespace CardHaven.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {   
        //plocka in info om korten för att kunna skriva ut den infon
        var auctions = await _context.Auctions
        .OrderByDescending(a => a.EndTime)
        .ToListAsync();

        //om tiden gått ut ändras isClosed till true
        foreach (var auction in auctions)
        {
            if (auction.EndTime < DateTime.Now && !auction.IsClosed)
            {
                auction.IsClosed = true;
            }
        }

        await _context.SaveChangesAsync();

        //skicka auktioner till vy
        return View(auctions);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    //för aboutsidan i footer, ändrar sökvägen
    [Route("About")]
    public IActionResult About()
    {
        return View();
    }
}

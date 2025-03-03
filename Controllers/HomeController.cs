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
        .ToListAsync();

        //skicka auktioner till vy
        return View(auctions);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CardHaven.Models;

namespace CardHaven.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
    public DbSet<Auction> Auctions { get; set; }

    public DbSet<Bid> Bids { get; set; }

    public DbSet<Card> Cards { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}

﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CardHaven.Models;

namespace CardHaven.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUserModel>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //refererar till modellerna
    public DbSet<AuctionModel> Auctions { get; set; }

    public DbSet<BidModel> Bids { get; set; }

    public DbSet<TransactionModel> Transactions { get; set; }
}

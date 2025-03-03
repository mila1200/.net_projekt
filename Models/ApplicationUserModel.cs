//förlängnin av användare

using Microsoft.AspNetCore.Identity;

namespace CardHaven.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Användarnamn
        public string? DisplayName { get; set; }

        //Saldo
        public decimal Balance { get; set; }

        //samling med bud
        public ICollection<Bid>? Bids { get; set; }

        //samling med auktioner
        public ICollection<Auction>? Auctions { get; set; }
    }
}
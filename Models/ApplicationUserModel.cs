//förlängnin av användare

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CardHaven.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        //Användarnamn
        public string? DisplayName { get; set; }

        //Saldo
        [Range(0, double.MaxValue, ErrorMessage = "Saldot kan inte vara ett negativt belopp")]
        public decimal Balance { get; set; }

        //samling med bud
        public ICollection<BidModel>? Bids { get; set; }

        //samling med auktioner
        public ICollection<AuctionModel>? Auctions { get; set; }
    }
}
namespace CardHaven.Models;

public class Bid
{
    public int Id {get; set;}

    //auktionsId
    public int AuctionId {get; set;}
    //kopplar till auktionsmodell
    public Auction? Auction {get; set;}

    //koppling till användare
    public string? UserId {get; set;}
    public ApplicationUser? User {get; set;}

    //hur mycket bjuds
    public decimal Amount {get; set;}

    public DateTime PlacedAt {get; set;} = DateTime.Now;
}
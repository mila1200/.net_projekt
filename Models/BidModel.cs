namespace CardHaven.Models;

public class BidModel
{
    public int Id {get; set;}

    //auktionsId
    public int AuctionId {get; set;}
    //kopplar till auktionsmodell
    public AuctionModel? Auction {get; set;}

    //koppling till användare
    public string? UserId {get; set;}
    public ApplicationUserModel? User {get; set;}

    //hur mycket bjuds
    public decimal Amount {get; set;}

    public DateTime PlacedAt {get; set;} = DateTime.Now;
}
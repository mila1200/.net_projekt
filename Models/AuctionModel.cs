namespace CardHaven.Models;

public class AuctionModel
{
    //auktionens id
    public int Id { get; set; }

    //kortets id
    public int CardId {get; set;}

    //kortmodellen
    public CardModel? Card {get; set;}

    //säljarid
    public string? SellerId {get; set;}

    //skapa koppling mellan användare och vem som säljer
    public ApplicationUserModel? Seller {get; set;}

    public decimal StartPrice {get; set;}
    public decimal CurrentPrice {get; set;}

    //starttid och sluttid
    public DateTime StartTime {get; set;}
    public DateTime EndTime {get; set;}

    //koppling till bud
    public ICollection<BidModel>? Bids {get; set;}
}
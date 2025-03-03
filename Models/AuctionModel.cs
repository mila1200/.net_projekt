namespace CardHaven.Models;

public class Auction
{
    //auktionens id
    public int Id { get; set; }

    //kortets id
    public int CardId {get; set;}

    //kortmodellen
    public Card? Card {get; set;}

    //säljarid
    public string? SellerId {get; set;}

    //skapa koppling mellan användare och vem som säljer
    public ApplicationUser? Seller {get; set;}

    public decimal StartPrice {get; set;}
    public decimal CurrentPrice {get; set;}

    //starttid och sluttid
    public DateTime StartTime {get; set;}
    public DateTime EndTime {get; set;}

    //koppling till bud
    public ICollection<Bid>? Bids {get; set;}
}
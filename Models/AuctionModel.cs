using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CardHaven.Models;

public class AuctionModel
{
    //auktionens id
    public int Id { get; set; }

    //kortets namn
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Set {get; set;}

    [Required]
    public string? Description {get; set;}

    //skick ex. Near Mint
    public string? Condition{get; set;}

    //lagra bild i databasen
    public string? ImageName { get; set; }

    //bilden i gränssnittet, (NotMapped = lagras inte i databasen)
    [NotMapped]
    [Required]
    public IFormFile? ImageFile { get; set; }

    //säljarid
    public string? SellerId {get; set;}

    //skapa koppling mellan användare och vem som säljer
    public ApplicationUserModel? Seller {get; set;}

    [Required]
    public int? AskingPrice {get; set;} = 1;
   
    //starttid och sluttid
    public DateTime StartTime {get; set;} = DateTime.Now;
    public DateTime EndTime {get; set;}

    //koppling till bud
    public ICollection<BidModel>? Bids {get; set;}
}
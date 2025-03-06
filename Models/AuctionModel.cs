using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CardHaven.Models;

public class AuctionModel
{
    //auktionens id
    public int Id { get; set; }

    //kortets namn
    
    [Required]
    [Display(Name = "Rubrik")]
    public string? Name { get; set; }

    [Required]
    public string? Set {get; set;}

    [Display(Name = "Beskrivning")]
    [Required]
    public string? Description {get; set;}

    //skick ex. Near Mint
    [Display(Name = "Skick")]
    public string? Condition{get; set;}

    //lagra bild i databasen
    public string? ImageName { get; set; }

    //bilden i gränssnittet, (NotMapped = lagras inte i databasen)
    [NotMapped]
    [Display(Name = "Bifoga bild")]
    public IFormFile? ImageFile { get; set; }

    //säljarid
    public string? SellerId {get; set;}

    //skapa koppling mellan användare och vem som säljer
    public ApplicationUserModel? Seller {get; set;}

    [Required]
    [Display(Name = "Utropspris")]
    public int AskingPrice {get; set;} = 1;
   
    //starttid och sluttid
    public DateTime StartTime {get; set;} = DateTime.Now;

    [Display(Name = "Avslutas")]
    public DateTime EndTime {get; set;}

    //koppling till bud. new List<Bidmodel> skapar en tom lista istälelt för att jag ska behöva hantera ev. null-värden
    public ICollection<BidModel> Bids {get; set;} = new List<BidModel>();

    //avslutad?
    public bool IsClosed { get; set; } = false;
}
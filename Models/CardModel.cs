using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardHaven.Models;

public class Card
{
    public int Id {get; set;}
    
    // Kortets namn ex. Charizard 058
    [Required]
    public string? Name {get; set;}

    //vilket set, ex. Base set
     [Required]
    public string? Set {get; set;}

    //skick ex. Near Mint
     [Required]
    public string? Condition{get; set;}

    [Required]
    public string? Description { get; set; }

    //lagra bild i databasen
    public string? ImageName { get; set; }

    //bilden i gränssnittet, (NotMapped = lagras inte i databasen)
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
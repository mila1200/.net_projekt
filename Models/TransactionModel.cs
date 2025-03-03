namespace CardHaven.Models;

public class Transaction
{
    public int Id {get; set;}

    //koppling till användare
    public string? UserId {get; set;}
    public ApplicationUser? User {get; set;}

    //summa för transaktion
    public decimal Amount {get; set;}

    //beskrivningav köp ex. Köp av Charizard 058
    public string? Description {get; set;}

    public DateTime Date {get; set;}
}
namespace TCGApp.Server.DTO;
using TCGApp.Server.Models;

public class AddCardsToCollection
{
    public int CollectionID { get; set; } // The id of the collection to add the cards to

    public List<CardWithCount> Cards { get; set; } // The list of cards to add
}
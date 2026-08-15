using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGApp.Server.Models
{
    public class CollectionCard
    {
        // This class represents the relationship between a card and a collection, allowing for multiple copies of the same card in a collection.
        [Key]
        public int CollectionCardID { get; set; }
        [ForeignKey("Collection")]
        public int CollectionID { get; set; } // Foreign key to the collection
        public Collection? Collection { get; set; } // Navigation property to the collection
        public int CardID { get; set; }
        public Card? Card { get; set; }
    }
}

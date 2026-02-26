using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGApp.Server.Models
{
    public class CollectionCard
    {
        // This class represents the relationship between a card and a collection, allowing for multiple copies of the same card in a collection.
        [Key]
        public int Id { get; set; }
        public int QuantityInCollection { get; set; } // Number of this card in the collection
        [ForeignKey("Collection")]
        public int CollectionId { get; set; } // Foreign key to the collection
        public Collection Collection { get; set; } // Navigation property to the collection
    }
}

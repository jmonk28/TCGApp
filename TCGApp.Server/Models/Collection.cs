using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGApp.Server.Models
{
    public class Collection
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string CollectionName { get; set; }
        [Required]
        public string CollectionType { get; set; } // e.g. "Magic", "Pokemon", etc.
        public int CardCount { get; set; } // Number of cards in the collection
        public ICollection<CollectionCard>? Cards { get; set; } // Navigation property to the cards in the collection

        [ForeignKey("TCGUser")]
        public int TCGUserId { get; set; } // Foreign key to the owning user
        public TCGUser TCGUser { get; set; } // Navigation property to the owning user
    }
}

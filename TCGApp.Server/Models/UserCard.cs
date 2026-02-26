using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGApp.Server.Models
{
    public class UserCard
    {
        // This class represents the relationship between a user and a card, allowing for tracking how many copies of each card a user owns.
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; } // Number of this card owned by the user

        [ForeignKey("TCGUser")]
        public int TCGUserId { get; set; } // Foreign key to the owning user
        public TCGUser TCGUser { get; set; } // Navigation property to the owning user

        [ForeignKey("Card")]
        public int CardId { get; set; }
        public Card Card { get; set; } // Navigation property to the card details

    }
}

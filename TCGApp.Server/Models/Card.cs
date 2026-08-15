using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGApp.Server.Models
{
    public class Card
    {
        [Key]
        public int CardID { get; set; }
        public string CardName { get; set; } // Name of the card
        public string CardDescription { get; set; } // Flavor text or in-game description of the card
        public string CardGame { get; set; } // e.g. "Magic", "Pokemon", etc.
        public string Rarity { get; set; } // e.g. "Common", "Rare", etc.
        public string CardSet { get; set; } // e.g. "Core Set 2021", "Shadows Over Innistrad", etc.
        public float Price { get; set; } // Market price of the card
        public string SpecialProperties { get; set; } // e.g. "Foil", "First Edition", etc.
        public string InGameProperties { get; set; } // e.g. "Haste", "Flying", etc.
        public string Image { get; set; }
        public string CardType { get; set; } // e.g. "Creature", "Spell", etc.
    }
}

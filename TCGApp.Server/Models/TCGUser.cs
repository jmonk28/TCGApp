using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TCGApp.Server.Models
{
    public class TCGUser
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public DateTime? LastLogin { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public ICollection<Collection>? Collections { get; set; }
    }
}

using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;

namespace TCGApp.Server.Models
{
    public class TCGUser
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LastLogin { get; set; }

        public ICollection<Collection> Collections { get; set; }
    }
}

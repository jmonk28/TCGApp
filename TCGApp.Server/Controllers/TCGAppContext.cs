using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TCGApp.Server.Data
{
    public class TCGAppContext : DbContext
    {
        public TCGAppContext(DbContextOptions<TCGAppContext> options) : base(options)
        {
        }

        public DbSet<TCGApp.Server.Models.TCGUser> TCGUser { get; set; }
        public DbSet<TCGApp.Server.Models.Collection> Collection { get; set; }
        public DbSet<TCGApp.Server.Models.Card> Card { get; set; }
        public DbSet<TCGApp.Server.Models.Collection> CollectionCard { get; set; }
        public DbSet<TCGApp.Server.Models.UserCard> UserCard { get; set; }
    }
}

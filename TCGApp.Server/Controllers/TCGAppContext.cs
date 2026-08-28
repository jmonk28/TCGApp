using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TCGApp.Server.Models;

namespace TCGApp.Server.Data
{
    public class TCGAppContext : DbContext
    {
        public TCGAppContext(DbContextOptions<TCGAppContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CollectionWithCounts>()
                .ToView("CollectionWithCounts")
                .HasKey(c => c.CollectionID);
        }

        public DbSet<TCGApp.Server.Models.TCGUser> TCGUser { get; set; }
        public DbSet<TCGApp.Server.Models.Collection> Collection { get; set; }
        public DbSet<TCGApp.Server.Models.Card> Card { get; set; }
        public DbSet<TCGApp.Server.Models.CollectionCard> CollectionCard { get; set; }
        public DbSet<TCGApp.Server.Models.UserCard> UserCard { get; set; }
        public DbSet<TCGApp.Server.Models.CollectionWithCounts> CollectionWithCounts { get; set; }
    }
}

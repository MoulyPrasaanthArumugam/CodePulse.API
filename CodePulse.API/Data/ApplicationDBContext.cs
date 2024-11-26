using CodePulse.API.Model;
using CodePulse.API.Model.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    //<summary>
    //class : Represents Database and Tables
    //</summary>
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Content>Contents { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Watchlist> WatchList { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<DisLike> Dislike { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configure Watchlist relationships
            modelBuilder.Entity<WishList>()
                .HasOne<IdentityUser>()
                .WithMany() // User can have many watchlist items
                .HasForeignKey(w => w.UserId) // UserId in Watchlist is the foreign key
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Set delete behavior

            modelBuilder.Entity<WishList>()
                .HasOne(w =>w.Content)
                .WithMany() // Content can appear in many watchlists
                .HasForeignKey(w => w.ContentId);

            modelBuilder.Entity<Like>()
                .HasOne(w => w.Content)
                .WithMany() 
                .HasForeignKey(w => w.ContentId);

            modelBuilder.Entity<Like>()
                .HasOne<IdentityUser>()
                .WithMany() 
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<DisLike>()
                .HasOne(w => w.Content)
                .WithMany() 
                .HasForeignKey(w => w.ContentId);

            modelBuilder.Entity<DisLike>()
                .HasOne<IdentityUser>()
                .WithMany() 
                .HasForeignKey(w => w.UserId);
        }
    }
}

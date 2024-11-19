using CodePulse.API.Model.Domain;
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

        public DbSet<WatchList> WatchList { get; set; }
    }
}

using CodePulse.API.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<BlogSpot>BlogSpot { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }

    }
}


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext: IdentityDbContext  //Inheriting IdentityDbContext from Identity.EntityFrameworkCore
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options) 
        {
        }

        //Used to Define Roles
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "f064f08d-3b96-44b2-94e0-45bdbc868ff6";
            var writerRoleId = "44fc55c2-158d-45c1-8f37-359be5ef6f73";

            //Create Reader and Writer Roles
            var roles = new List<IdentityRole>
             {
                 new IdentityRole() {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                 },
                 new IdentityRole() {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                 }
             };

            //Seed the Roles (It makes sures to seed the roles into role table when we run migration)
            builder.Entity<IdentityRole>().HasData(roles);

            //Create an Admin User
            var adminUserId = "a226da08-f00d-4f81-bbfe-3e119fe5c613";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles To Admin

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }

    }

    
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SunScape.Data
{
    //TODO Identity 2. Create a DbContext
    public class ApplicationNpgsqlIdentityDbContext(DbContextOptions<ApplicationNpgsqlIdentityDbContext> options) 
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(@"Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

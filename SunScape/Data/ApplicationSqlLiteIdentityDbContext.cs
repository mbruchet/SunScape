using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SunScape.Data
{
    //TODO Identity 2. Create a DbContext
    public class ApplicationSqlLiteIdentityDbContext(DbContextOptions<ApplicationSqlLiteIdentityDbContext> options) 
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=my.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}

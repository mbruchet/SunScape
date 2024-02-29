using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SunScape.Data
{
    //TODO Identity 2. Create a DbContext
    public class ApplicationSqlServerIdentityDbContext(DbContextOptions<ApplicationSqlServerIdentityDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}

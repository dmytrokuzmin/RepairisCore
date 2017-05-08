using Abp.Zero.EntityFrameworkCore;
using Repairis.Authorization.Roles;
using Repairis.Authorization.Users;
using Repairis.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Repairis.EntityFrameworkCore
{
    public class RepairisDbContext : AbpZeroDbContext<Tenant, Role, User, RepairisDbContext>
    {
        /* Define an IDbSet for each entity of the application */
        
        public RepairisDbContext(DbContextOptions<RepairisDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //...
        }
    }
}

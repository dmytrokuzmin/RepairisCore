using Microsoft.EntityFrameworkCore;

namespace Repairis.EntityFrameworkCore
{
    public static class RepairisDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<RepairisDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }
    }
}
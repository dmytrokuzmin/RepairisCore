using Repairis.Configuration;
using Repairis.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Repairis.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class RepairisDbContextFactory : IDbContextFactory<RepairisDbContext>
    {
        public RepairisDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<RepairisDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            RepairisDbContextConfigurer.Configure(builder, configuration.GetConnectionString(RepairisConsts.ConnectionStringName));
            
            return new RepairisDbContext(builder.Options);
        }
    }
}
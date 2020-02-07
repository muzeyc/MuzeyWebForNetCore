using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MuzeyAngular.Configuration;
using MuzeyAngular.Web;

namespace MuzeyAngular.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class MuzeyAngularDbContextFactory : IDesignTimeDbContextFactory<MuzeyAngularDbContext>
    {
        public MuzeyAngularDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MuzeyAngularDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            MuzeyAngularDbContextConfigurer.Configure(builder, configuration.GetConnectionString(MuzeyAngularConsts.ConnectionStringName));

            return new MuzeyAngularDbContext(builder.Options);
        }
    }
}

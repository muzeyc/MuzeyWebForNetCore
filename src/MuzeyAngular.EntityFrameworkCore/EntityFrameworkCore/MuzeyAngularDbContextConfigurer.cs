using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MuzeyAngular.EntityFrameworkCore
{
    public static class MuzeyAngularDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MuzeyAngularDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MuzeyAngularDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}

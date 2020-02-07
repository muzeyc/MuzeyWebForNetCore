using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MuzeyAngular.Authorization.Roles;
using MuzeyAngular.Authorization.Users;
using MuzeyAngular.MultiTenancy;

namespace MuzeyAngular.EntityFrameworkCore
{
    public class MuzeyAngularDbContext : AbpZeroDbContext<Tenant, Role, User, MuzeyAngularDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public MuzeyAngularDbContext(DbContextOptions<MuzeyAngularDbContext> options)
            : base(options)
        {
        }
    }
}

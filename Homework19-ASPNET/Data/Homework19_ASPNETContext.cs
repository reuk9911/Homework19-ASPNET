using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Homework19_ASPNET;
using Homework19_ASPNET.Auth;

namespace Homework19_ASPNET.Data
{
    public class Homework19_ASPNETContext : IdentityDbContext<User>
    {
        public Homework19_ASPNETContext (DbContextOptions<Homework19_ASPNETContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Homework19_ASPNET.Project> Project { get; set; } = default!;
    }
}

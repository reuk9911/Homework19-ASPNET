using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Homework19_ASPNET;

namespace Homework19_ASPNET.Data
{
    public class Homework19_ASPNETContext : DbContext
    {
        public Homework19_ASPNETContext (DbContextOptions<Homework19_ASPNETContext> options)
            : base(options)
        {
        }

        public DbSet<Homework19_ASPNET.Project> Project { get; set; } = default!;
    }
}

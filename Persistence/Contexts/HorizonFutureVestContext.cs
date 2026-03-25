using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class HorizonFutureVestContext : DbContext
    {
        public HorizonFutureVestContext(DbContextOptions<HorizonFutureVestContext> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<MacroIndicator> MacroIndicators { get; set; }
        public DbSet<ReturnRate> ReturnRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Liskov

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            
        }

    }
}

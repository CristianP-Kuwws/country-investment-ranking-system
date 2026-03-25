using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations
{
    public class ReturnRateEntityConfiguration : IEntityTypeConfiguration<ReturnRate>
    {
        public void Configure(EntityTypeBuilder<ReturnRate> builder)
        {
            #region Basic Configurations
            builder.HasKey(x => x.IdReturnRate);
            builder.ToTable("ReturnRates", t =>
            {
                t.HasCheckConstraint("CK_ReturnRate_MinLessThanMax", "MinReturnRate < MaxReturnRate");
            });
            #endregion

            #region Property Configurations
            builder.Property(rr => rr.MinReturnRate).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(rr => rr.MaxReturnRate).IsRequired().HasColumnType("decimal(18,4)");
            #endregion
        }
    }
}

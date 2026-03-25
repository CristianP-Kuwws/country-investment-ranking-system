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
    public class MacroIndicatorEntityConfiguration : IEntityTypeConfiguration<MacroIndicator>
    {
        public void Configure(EntityTypeBuilder<MacroIndicator> builder)
        {
            #region Basic Configurations
            builder.HasKey(x => x.IdMacroIndicator);
            builder.ToTable("MacroIndicators");
            #endregion

            #region Property Configurations
            builder.Property(mi => mi.Name).IsRequired().HasMaxLength(50);
            builder.HasIndex(mi => mi.Name).IsUnique();
            builder.Property(mi => mi.Weight).IsRequired().HasColumnType("decimal(5,4)");
            builder.Property(mi => mi.IsHighBetter).IsRequired();
            #endregion

            #region Property Relationships
            builder.HasMany<Indicator>(mi => mi.Indicators)
                .WithOne(i => i.MacroIndicator)
                .HasForeignKey(i => i.IdMacroIndicator);
            #endregion
        }
    }
}

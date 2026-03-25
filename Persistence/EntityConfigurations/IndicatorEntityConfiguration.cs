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
    public class IndicatorEntityConfiguration : IEntityTypeConfiguration<Indicator>
    {
        public void Configure(EntityTypeBuilder<Indicator> builder)
        {
            #region Basic Configurations
            builder.HasKey(x => x.IdIndicator);
            builder.ToTable("Indicators");
            #endregion

            #region Property Configurations
            builder.Property(i => i.Value).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(i => i.Year).IsRequired();
            #endregion

            #region Property Relationships
            // Un indicador pertenece a un pais
            builder.HasOne(i => i.Country)
                .WithMany(mi => mi.Indicators)
                .HasForeignKey(i => i.IdCountry)
                .OnDelete(DeleteBehavior.Restrict);

            // Un indicador pertenece a un macroindicador
            builder.HasOne(i => i.MacroIndicator)
                .WithMany(mi => mi.Indicators)
                .HasForeignKey(i => i.IdMacroIndicator)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Unique Constraints
            builder.HasIndex(i => new { i.IdCountry, i.IdMacroIndicator, i.Year }).IsUnique();
            #endregion
        }
    }
}

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
    public class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            #region Basic Configurations
            builder.HasKey(x => x.IdCountry);
            builder.ToTable("Countries");
            #endregion

            #region Property Configurations
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.Property(c => c.ISOCode).IsRequired().HasMaxLength(3);
            builder.HasIndex(c => c.ISOCode).IsUnique();
            #endregion

            #region Property Relationships
            builder.HasMany<Indicator>(c => c.Indicators)
                .WithOne(i => i.Country)
                .HasForeignKey(i => i.IdCountry);
            #endregion
        }
    }
}

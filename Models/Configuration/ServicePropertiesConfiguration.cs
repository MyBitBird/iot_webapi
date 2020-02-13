using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOT.Models.Configuration
{
    public class ServicePropertiesConfiguration : IEntityTypeConfiguration<ServiceProperties>
    {
        public void Configure(EntityTypeBuilder<ServiceProperties> builder)
        {

            builder.Property(e => e.Id)
                .HasDefaultValueSql("newid()");

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasOne(d => d.Service)
                .WithMany(p => p.ServiceProperties)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

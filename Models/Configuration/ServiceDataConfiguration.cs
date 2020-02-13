using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOT.Models.Configuration
{
    public class ServiceDataConfiguration : IEntityTypeConfiguration<ServiceData>
    {
        public void Configure(EntityTypeBuilder<ServiceData> builder)
        {
            builder.Property(e => e.Id)
                .HasDefaultValueSql("newid()");

            builder.Property(e => e.Data)
                .HasMaxLength(500);

            builder.HasOne(sd => sd.ServiceLog)
                .WithMany(sl => sl.ServiceData)
                .HasForeignKey(f => f.ServiceLogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.ServiceProperty)
                .WithMany(p => p.ServiceData)
                .HasForeignKey(d => d.ServicePropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

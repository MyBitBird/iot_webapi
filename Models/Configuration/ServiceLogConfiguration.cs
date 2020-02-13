using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOT.Models.Configuration
{
    public class ServiceLogConfiguration : IEntityTypeConfiguration<ServiceLogs>
    {
        public void Configure(EntityTypeBuilder<ServiceLogs> builder)
        {
            builder.Property(e => e.Id)
                .HasDefaultValueSql("newid()");

            builder.HasOne(d => d.Service)
                .WithMany(p => p.ServiceLogs)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.User)
                .WithMany(p => p.ServiceLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

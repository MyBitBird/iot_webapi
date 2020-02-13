using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOT.Models.Configuration
{
    public class ServiceUsersConfiguration : IEntityTypeConfiguration<ServiceUsers>
    {
        public void Configure(EntityTypeBuilder<ServiceUsers> builder)
        {
            builder.Property(e => e.Id)
                .HasDefaultValueSql("newid()");

            builder.Property(e => e.RegisterDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(d => d.Service)
                .WithMany(p => p.ServiceUsers)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.User)
                .WithMany(p => p.ServiceUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}

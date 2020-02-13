using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOT.Models.Configuration
{
    public class ServicesConfiguration : IEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
            builder.Property(e => e.Id)
                .HasDefaultValueSql("newid()");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasOne(d => d.User)
                .WithMany(p => p.Services)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

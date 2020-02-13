using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOT.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOT.Models.Configuration
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.Property(e => e.Id)
                .HasDefaultValueSql("newid()");

            builder.Property(e => e.Family)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(new Users()
            {
                Id = new Guid("2C256B01-355B-4C66-9B26-B12FF6D8D1C2"),
                Username = "BitBird",
                Password = "BitBird",
                Name = "Meisam",
                Family = "Malekzadeh",
                Status = MyEnums.UserStatus.ACTIVE,
                Type = MyEnums.UserTypes.ADMIN,
                RegisterDate = DateTime.Now
            });
        }
    }
}

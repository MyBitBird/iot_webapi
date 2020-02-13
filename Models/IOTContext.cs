using System;
using IOT.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IOT.Models
{
    public partial class IOTContext : DbContext
    {
        public IOTContext()
        {
        }

        public IOTContext(DbContextOptions<IOTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ServiceData> ServiceData { get; set; }
        public virtual DbSet<ServiceLogs> ServiceLogs { get; set; }
        public virtual DbSet<ServiceProperties> ServiceProperties { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<ServiceUsers> ServiceUsers { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //TODO Set OnDelete and HasConstraintName("service_data_service_logs_fk");

            modelBuilder.ApplyConfiguration(new ServiceDataConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceLogConfiguration());
            modelBuilder.ApplyConfiguration(new ServicePropertiesConfiguration());
            modelBuilder.ApplyConfiguration(new ServicesConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceUsersConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());


        }
    }
}

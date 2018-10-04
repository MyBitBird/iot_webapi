using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using IOT.Models;
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
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("host=localhost;database=IOT;user id=postgres;password=aZamam");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<ServiceData>(entity =>
            {
                entity.ToTable("service_data");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("character varying");

                entity.Property(e => e.ServicePropertyId).HasColumnName("service_property_id");

                entity.HasOne(d => d.ServiceProperty)
                    .WithMany(p => p.ServiceData)
                    .HasForeignKey(d => d.ServicePropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("service_data_service_properties_fk");
            });

            modelBuilder.Entity<ServiceLogs>(entity =>
            {
                entity.ToTable("service_logs");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.LogDate).HasColumnName("log_date");

                entity.Property(e => e.RegisterDate).HasColumnName("register_date");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceLogs)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("service_logs_services_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServiceLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("service_logs_users_fk");
            });

            modelBuilder.Entity<ServiceProperties>(entity =>
            {
                entity.ToTable("service_properties");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasColumnType("character varying");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceProperties)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("service_properties_services_fk");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.ToTable("services");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.RegisterDate).HasColumnName("register_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("character varying");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("services_users_fk");
            });

            modelBuilder.Entity<ServiceUsers>(entity =>
            {
                entity.ToTable("service_users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.RegisterDate).HasColumnName("register_date");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceUsers)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("service_users_services_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServiceUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("service_users_users_fk");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Family)
                    .IsRequired()
                    .HasColumnName("family")
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("character varying");

                entity.Property(e => e.RegisterDate).HasColumnName("register_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("character varying");
            });
        }
    }
}

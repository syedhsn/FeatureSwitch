using System;
using Microsoft.EntityFrameworkCore;

namespace FeatureSwitch.Models
{
    public partial class FeatureSwitchContext : DbContext
    {
        public FeatureSwitchContext()
        {
        }

        public FeatureSwitchContext(DbContextOptions<FeatureSwitchContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FeatureAccess> FeatureAccess { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=D8QT6MH2;Database=FeatureSwitch;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeatureAccess>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.ToTable("FeatureAccess");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.FeatureName)
                    .HasMaxLength(100)
                    .HasColumnName("FeatureName");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("Email");

                entity.Property(e => e.Enable).HasColumnName("Enable");
            });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TransactionSampleWebAPI.AzureEntity
{
    public partial class StudentDbAzureContext : DbContext
    {
        public StudentDbAzureContext()
        {
        }

        public StudentDbAzureContext(DbContextOptions<StudentDbAzureContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TrProjectDetails> TrProjectDetails { get; set; }
        public virtual DbSet<TrProjectMapper> TrProjectMapper { get; set; }
        public virtual DbSet<TrProjectSequenceMapper> TrProjectSequenceMapper { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrProjectDetails>(entity =>
            {
                entity.ToTable("TR_ProjectDetails");
            });

            modelBuilder.Entity<TrProjectMapper>(entity =>
            {
                entity.ToTable("TR_ProjectMapper");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TrProjectMapper)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TR_ProjectMapper_ProjectId");
            });

            modelBuilder.Entity<TrProjectSequenceMapper>(entity =>
            {
                entity.ToTable("TR_ProjectSequenceMapper");

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using InsightLab.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace InsightLab.Web.Data
{
    /// <summary>
    /// EF Core Code-First database context for InsightLab.
    /// Defines the two tables (Experiments, ExperimentParticipants) and the
    /// one-to-many relationship between them.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Experiment> Experiments => Set<Experiment>();

        public DbSet<ExperimentParticipant> ExperimentParticipants => Set<ExperimentParticipant>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Experiment>(entity =>
            {
                entity.HasKey(e => e.ExperimentId);
                entity.Property(e => e.ExperimentName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            modelBuilder.Entity<ExperimentParticipant>(entity =>
            {
                entity.HasKey(p => p.ParticipantId);
                entity.Property(p => p.SessionId).HasMaxLength(50);
                entity.Property(p => p.OrderValue).HasColumnType("decimal(10,2)");

                // One Experiment has many ExperimentParticipants.
                entity.HasOne(p => p.Experiment)
                      .WithMany(e => e.Participants)
                      .HasForeignKey(p => p.ExperimentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Helpful indexes for the Data Explorer filters.
                entity.HasIndex(p => p.ExperimentId);
                entity.HasIndex(p => p.Variant);
                entity.HasIndex(p => p.Converted);
            });
        }
    }
}

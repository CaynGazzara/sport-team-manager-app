using Microsoft.EntityFrameworkCore;
using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchPlayer> MatchPlayers { get; set; }
    public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
    public DbSet<TrainingAttendance> TrainingAttendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Team configuration
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.Property(t => t.SportType).IsRequired().HasMaxLength(50);
            entity.Property(t => t.CoachName).HasMaxLength(100);
            entity.Property(t => t.HomeField).HasMaxLength(100);
            entity.Property(t => t.Colors).HasMaxLength(50);

            entity.HasMany(t => t.Players)
                  .WithOne()
                  .HasForeignKey(p => p.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Player configuration
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Position).IsRequired().HasMaxLength(50);
            entity.Property(p => p.JerseyNumber).HasMaxLength(10);
            entity.Property(p => p.Nationality).HasMaxLength(50);
        });

        // Match configuration
        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Opponent).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Location).HasMaxLength(100);
            entity.Property(m => m.MatchType).HasMaxLength(50);
            entity.Property(m => m.Result).HasMaxLength(20);
        });

        // Seed data
        modelBuilder.Entity<Team>().HasData(
            new Team
            {
                Id = Guid.NewGuid(),
                Name = "Main Team",
                SportType = "Football",
                CoachName = "John Coach",
                HomeField = "City Stadium",
                FoundedDate = new DateTime(2020, 1, 1),
                Colors = "Blue-White",
                CreatedAt = DateTime.UtcNow
            }
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
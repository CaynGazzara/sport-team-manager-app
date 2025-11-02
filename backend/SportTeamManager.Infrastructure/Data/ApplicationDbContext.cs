using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportTeamManager.Domain.Common;
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
        });

        // Player configuration
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Position).IsRequired().HasMaxLength(50);
            entity.Property(p => p.JerseyNumber).HasMaxLength(10);
            entity.Property(p => p.Nationality).HasMaxLength(50);

            // Configurações de precisão para campos decimais
            entity.Property(p => p.Height).HasPrecision(5, 2);
            entity.Property(p => p.Weight).HasPrecision(6, 2);

            // Relationship with Team
            entity.HasOne(p => p.Team)
                  .WithMany(t => t.Players)
                  .HasForeignKey(p => p.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Training configuration - CORREÇÃO DO TIMESPAN
        modelBuilder.Entity<Training>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Location).HasMaxLength(100);
            entity.Property(t => t.Focus).HasMaxLength(50);

            // CORREÇÃO: Converter TimeSpan para string ou usar ticks
            entity.Property(t => t.Duration)
                  .HasConversion(
                      v => v.ToString(), // Converte TimeSpan para string
                      v => TimeSpan.Parse(v) // Converte string para TimeSpan
                  )
                  .HasMaxLength(20);

            // Relationship with Team
            entity.HasOne(t => t.Team)
                  .WithMany(team => team.Trainings)
                  .HasForeignKey(t => t.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Match configuration
        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Opponent).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Location).HasMaxLength(100);
            entity.Property(m => m.MatchType).HasMaxLength(50);
            entity.Property(m => m.Result).HasMaxLength(20);

            // Relationship with Team
            entity.HasOne(m => m.Team)
                  .WithMany(t => t.Matches)
                  .HasForeignKey(m => m.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // MatchPlayer configuration (junction table)
        modelBuilder.Entity<MatchPlayer>(entity =>
        {
            entity.HasKey(mp => mp.Id);
            entity.Property(mp => mp.Position).HasMaxLength(50);

            // Relationships
            entity.HasOne(mp => mp.Match)
                  .WithMany(m => m.MatchPlayers)
                  .HasForeignKey(mp => mp.MatchId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(mp => mp.Player)
                  .WithMany(p => p.MatchPlayers)
                  .HasForeignKey(mp => mp.PlayerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // PlayerStatistic configuration
        modelBuilder.Entity<PlayerStatistic>(entity =>
        {
            entity.HasKey(ps => ps.Id);
            entity.Property(ps => ps.Season).HasMaxLength(20);
            entity.Property(ps => ps.AverageRating).HasPrecision(3, 1);

            // Relationship with Player
            entity.HasOne(ps => ps.Player)
                  .WithMany(p => p.Statistics)
                  .HasForeignKey(ps => ps.PlayerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TrainingAttendance configuration (junction table)
        modelBuilder.Entity<TrainingAttendance>(entity =>
        {
            entity.HasKey(ta => ta.Id);

            // Relationships
            entity.HasOne(ta => ta.Training)
                  .WithMany(t => t.Attendances)
                  .HasForeignKey(ta => ta.TrainingId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ta => ta.Player)
                  .WithMany(p => p.TrainingAttendances)
                  .HasForeignKey(ta => ta.PlayerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed data
        var teamId = Guid.NewGuid();
        modelBuilder.Entity<Team>().HasData(
            new Team
            {
                Id = teamId,
                Name = "Main Team",
                SportType = "Football",
                CoachName = "John Coach",
                HomeField = "City Stadium",
                FoundedDate = new DateTime(2020, 1, 1),
                Colors = "Blue-White",
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed sample players
        modelBuilder.Entity<Player>().HasData(
            new Player
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Age = 25,
                Position = "Forward",
                JerseyNumber = "10",
                BirthDate = new DateTime(1998, 5, 15),
                Height = 1.80m,
                Weight = 75.0m,
                Nationality = "Brazilian",
                JoinDate = new DateTime(2022, 1, 10),
                IsActive = true,
                TeamId = teamId,
                CreatedAt = DateTime.UtcNow
            },
            new Player
            {
                Id = Guid.NewGuid(),
                Name = "Mike Smith",
                Age = 28,
                Position = "Goalkeeper",
                JerseyNumber = "1",
                BirthDate = new DateTime(1995, 3, 20),
                Height = 1.90m,
                Weight = 85.0m,
                Nationality = "English",
                JoinDate = new DateTime(2021, 7, 15),
                IsActive = true,
                TeamId = teamId,
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed sample training
        modelBuilder.Entity<Training>().HasData(
            new Training
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(1),
                Duration = TimeSpan.FromHours(2),
                Location = "Campo Principal",
                Focus = "Technical",
                Notes = "Treino técnico - finalização",
                TeamId = teamId,
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed sample match
        modelBuilder.Entity<Match>().HasData(
            new Match
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(7),
                Opponent = "Rival Team",
                Location = "Estádio Municipal",
                MatchType = "Friendly",
                Result = "Scheduled",
                TeamId = teamId,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
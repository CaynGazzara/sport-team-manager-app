using System.Numerics;

namespace SportTeamManager.Domain.Entities;

public class PlayerStatistic
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public string Season { get; set; } = string.Empty;
    public int MatchesPlayed { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int CleanSheets { get; set; } // For goalkeepers
    public decimal AverageRating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Player Player { get; set; } = null!;
}
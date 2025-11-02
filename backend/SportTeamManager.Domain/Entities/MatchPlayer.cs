using System.Numerics;

namespace SportTeamManager.Domain.Entities;

public class MatchPlayer
{
    public Guid Id { get; set; }
    public Guid MatchId { get; set; }
    public Guid PlayerId { get; set; }
    public bool IsStarter { get; set; }
    public string? Position { get; set; }
    public int? MinutesPlayed { get; set; }
    public int? Goals { get; set; }
    public int? Assists { get; set; }
    public int? YellowCards { get; set; }
    public int? RedCards { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Match Match { get; set; } = null!;
    public virtual Player Player { get; set; } = null!;
}
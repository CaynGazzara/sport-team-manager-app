namespace SportTeamManager.Domain.Entities;

public class Match
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Opponent { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string MatchType { get; set; } = "Friendly"; // Friendly, League, Cup, etc.
    public int? TeamScore { get; set; }
    public int? OpponentScore { get; set; }
    public string Result { get; set; } = "Scheduled"; // Win, Loss, Draw, Scheduled
    public string? Notes { get; set; }
    public Guid TeamId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Team Team { get; set; } = null!;
    public virtual ICollection<MatchPlayer> MatchPlayers { get; set; } = new List<MatchPlayer>();
}
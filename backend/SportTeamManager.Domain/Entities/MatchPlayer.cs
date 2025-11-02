using SportTeamManager.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportTeamManager.Domain.Entities;

public class MatchPlayer : BaseEntity
{
    public Guid Id { get; set; }

    // Foreign Keys
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
    [ForeignKey("MatchId")]
    public virtual Match Match { get; set; } = null!;

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; } = null!;
}
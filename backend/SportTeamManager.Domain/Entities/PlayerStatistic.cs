using SportTeamManager.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportTeamManager.Domain.Entities;

public class PlayerStatistic : BaseEntity
{
    public Guid Id { get; set; }

    // Foreign Key
    public Guid PlayerId { get; set; }

    public string Season { get; set; } = string.Empty;
    public int MatchesPlayed { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int CleanSheets { get; set; }
    public decimal AverageRating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; } = null!;
}
using SportTeamManager.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportTeamManager.Domain.Entities;

public class Player : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Position { get; set; } = string.Empty;
    public string JerseyNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Foreign Key
    public Guid TeamId { get; set; }

    // Navigation properties
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; } = null!;
    public virtual ICollection<PlayerStatistic> Statistics { get; set; } = new List<PlayerStatistic>();
    public virtual ICollection<TrainingAttendance> TrainingAttendances { get; set; } = new List<TrainingAttendance>();
    public virtual ICollection<MatchPlayer> MatchPlayers { get; set; } = new List<MatchPlayer>();
}
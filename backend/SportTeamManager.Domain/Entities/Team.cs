using System.Numerics;
using System.Text.RegularExpressions;

namespace SportTeamManager.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SportType { get; set; } = "Football"; // Football, Basketball, etc.
    public string CoachName { get; set; } = string.Empty;
    public string HomeField { get; set; } = string.Empty;
    public DateTime FoundedDate { get; set; }
    public string Colors { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();
    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
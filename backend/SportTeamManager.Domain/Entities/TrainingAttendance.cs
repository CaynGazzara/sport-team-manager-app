using System.Numerics;

namespace SportTeamManager.Domain.Entities;

public class TrainingAttendance
{
    public Guid Id { get; set; }
    public Guid TrainingId { get; set; }
    public Guid PlayerId { get; set; }
    public bool Attended { get; set; }
    public string? Notes { get; set; }
    public int? Rating { get; set; } // 1-5 rating for performance

    // Navigation properties
    public virtual Training Training { get; set; } = null!;
    public virtual Player Player { get; set; } = null!;
}
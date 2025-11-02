using SportTeamManager.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportTeamManager.Domain.Entities;

public class TrainingAttendance : BaseEntity
{
    public Guid Id { get; set; }

    // Foreign Keys
    public Guid TrainingId { get; set; }
    public Guid PlayerId { get; set; }

    public bool Attended { get; set; }
    public string? Notes { get; set; }
    public int? Rating { get; set; }

    // Navigation properties
    [ForeignKey("TrainingId")]
    public virtual Training Training { get; set; } = null!;

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; } = null!;
}
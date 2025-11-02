namespace SportTeamManager.Domain.Entities;

public class Training
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Focus { get; set; } = string.Empty; // Technical, Tactical, Physical, etc.
    public string? Notes { get; set; }
    public Guid TeamId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Team Team { get; set; } = null!;
    public virtual ICollection<TrainingAttendance> Attendances { get; set; } = new List<TrainingAttendance>();
}
namespace SportTeamManager.Application.DTOs;

public class TrainingDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Focus { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public Guid TeamId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTrainingDto
{
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Focus { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public Guid TeamId { get; set; }
}

public class UpdateTrainingDto
{
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Focus { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class TrainingWithAttendancesDto
{
    public TrainingDto Training { get; set; } = null!;
    public List<TrainingAttendanceDto> Attendances { get; set; } = new();
}

public class TrainingAttendanceDto
{
    public Guid Id { get; set; }
    public Guid TrainingId { get; set; }
    public Guid PlayerId { get; set; }
    public bool Attended { get; set; }
    public string? Notes { get; set; }
    public int? Rating { get; set; }
    public string PlayerName { get; set; } = string.Empty;
}
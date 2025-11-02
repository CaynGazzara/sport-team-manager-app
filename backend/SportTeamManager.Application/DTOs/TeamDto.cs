namespace SportTeamManager.Application.DTOs;

public class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SportType { get; set; } = string.Empty;
    public string CoachName { get; set; } = string.Empty;
    public string HomeField { get; set; } = string.Empty;
    public DateTime FoundedDate { get; set; }
    public string Colors { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTeamDto
{
    public string Name { get; set; } = string.Empty;
    public string SportType { get; set; } = "Football";
    public string CoachName { get; set; } = string.Empty;
    public string HomeField { get; set; } = string.Empty;
    public DateTime FoundedDate { get; set; }
    public string Colors { get; set; } = string.Empty;
}

public class UpdateTeamDto
{
    public string Name { get; set; } = string.Empty;
    public string CoachName { get; set; } = string.Empty;
    public string HomeField { get; set; } = string.Empty;
    public string Colors { get; set; } = string.Empty;
}
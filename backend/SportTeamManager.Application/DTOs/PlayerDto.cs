namespace SportTeamManager.Application.DTOs;

public class PlayerDto
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
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid TeamId { get; set; }
}

public class CreatePlayerDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Position { get; set; } = string.Empty;
    public string JerseyNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public Guid TeamId { get; set; }

}

public class UpdatePlayerDto
{
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string JerseyNumber { get; set; } = string.Empty;
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public bool IsActive { get; set; }
}
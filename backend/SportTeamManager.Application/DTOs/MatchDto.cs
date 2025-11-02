namespace SportTeamManager.Application.DTOs;

public class MatchDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Opponent { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string MatchType { get; set; } = string.Empty;
    public int? TeamScore { get; set; }
    public int? OpponentScore { get; set; }
    public string Result { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public Guid TeamId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateMatchDto
{
    public DateTime Date { get; set; }
    public string Opponent { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string MatchType { get; set; } = string.Empty;
    public int? TeamScore { get; set; }
    public int? OpponentScore { get; set; }
    public string? Notes { get; set; }
    public Guid TeamId { get; set; }
}

public class UpdateMatchDto
{
    public DateTime Date { get; set; }
    public string Opponent { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string MatchType { get; set; } = string.Empty;
    public int? TeamScore { get; set; }
    public int? OpponentScore { get; set; }
    public string Result { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class MatchWithPlayersDto
{
    public MatchDto Match { get; set; } = null!;
    public List<MatchPlayerDto> MatchPlayers { get; set; } = new();
}

public class MatchPlayerDto
{
    public Guid Id { get; set; }
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
    public string PlayerName { get; set; } = string.Empty;
}
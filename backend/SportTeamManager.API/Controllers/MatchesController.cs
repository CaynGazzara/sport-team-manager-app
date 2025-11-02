using Microsoft.AspNetCore.Mvc;
using SportTeamManager.Application.DTOs;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;

namespace SportTeamManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IMatchRepository _matchRepository;

    public MatchesController(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatches()
    {
        var matches = await _matchRepository.GetAllAsync();
        var matchDtos = matches.Select(m => new MatchDto
        {
            Id = m.Id,
            Date = m.Date,
            Opponent = m.Opponent,
            Location = m.Location,
            MatchType = m.MatchType,
            TeamScore = m.TeamScore,
            OpponentScore = m.OpponentScore,
            Result = m.Result,
            Notes = m.Notes,
            TeamId = m.TeamId,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        });

        return Ok(matchDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MatchDto>> GetMatch(Guid id)
    {
        var match = await _matchRepository.GetByIdAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        var matchDto = new MatchDto
        {
            Id = match.Id,
            Date = match.Date,
            Opponent = match.Opponent,
            Location = match.Location,
            MatchType = match.MatchType,
            TeamScore = match.TeamScore,
            OpponentScore = match.OpponentScore,
            Result = match.Result,
            Notes = match.Notes,
            TeamId = match.TeamId,
            CreatedAt = match.CreatedAt,
            UpdatedAt = match.UpdatedAt
        };

        return Ok(matchDto);
    }

    [HttpPost]
    public async Task<ActionResult<MatchDto>> CreateMatch(CreateMatchDto createMatchDto)
    {
        var match = new Match
        {
            Id = Guid.NewGuid(),
            Date = createMatchDto.Date,
            Opponent = createMatchDto.Opponent,
            Location = createMatchDto.Location,
            MatchType = createMatchDto.MatchType,
            TeamScore = createMatchDto.TeamScore,
            OpponentScore = createMatchDto.OpponentScore,
            Result = "Scheduled", // Default value
            Notes = createMatchDto.Notes,
            TeamId = createMatchDto.TeamId,
            CreatedAt = DateTime.UtcNow
        };

        var createdMatch = await _matchRepository.AddAsync(match);

        var matchDto = new MatchDto
        {
            Id = createdMatch.Id,
            Date = createdMatch.Date,
            Opponent = createdMatch.Opponent,
            Location = createdMatch.Location,
            MatchType = createdMatch.MatchType,
            TeamScore = createdMatch.TeamScore,
            OpponentScore = createdMatch.OpponentScore,
            Result = createdMatch.Result,
            Notes = createdMatch.Notes,
            TeamId = createdMatch.TeamId,
            CreatedAt = createdMatch.CreatedAt,
            UpdatedAt = createdMatch.UpdatedAt
        };

        return CreatedAtAction(nameof(GetMatch), new { id = matchDto.Id }, matchDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatch(Guid id, UpdateMatchDto updateMatchDto)
    {
        var match = await _matchRepository.GetByIdAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        match.Date = updateMatchDto.Date;
        match.Opponent = updateMatchDto.Opponent;
        match.Location = updateMatchDto.Location;
        match.MatchType = updateMatchDto.MatchType;
        match.TeamScore = updateMatchDto.TeamScore;
        match.OpponentScore = updateMatchDto.OpponentScore;
        match.Result = updateMatchDto.Result;
        match.Notes = updateMatchDto.Notes;
        match.UpdatedAt = DateTime.UtcNow;

        await _matchRepository.UpdateAsync(match);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatch(Guid id)
    {
        var match = await _matchRepository.GetByIdAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        await _matchRepository.DeleteAsync(match);

        return NoContent();
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetUpcomingMatches()
    {
        var matches = await _matchRepository.GetUpcomingMatchesAsync();
        var matchDtos = matches.Select(m => new MatchDto
        {
            Id = m.Id,
            Date = m.Date,
            Opponent = m.Opponent,
            Location = m.Location,
            MatchType = m.MatchType,
            TeamScore = m.TeamScore,
            OpponentScore = m.OpponentScore,
            Result = m.Result,
            Notes = m.Notes,
            TeamId = m.TeamId,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        });

        return Ok(matchDtos);
    }

    [HttpGet("{id}/players")]
    public async Task<ActionResult<MatchWithPlayersDto>> GetMatchWithPlayers(Guid id)
    {
        var match = await _matchRepository.GetMatchWithPlayersAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        var matchWithPlayersDto = new MatchWithPlayersDto
        {
            Match = new MatchDto
            {
                Id = match.Id,
                Date = match.Date,
                Opponent = match.Opponent,
                Location = match.Location,
                MatchType = match.MatchType,
                TeamScore = match.TeamScore,
                OpponentScore = match.OpponentScore,
                Result = match.Result,
                Notes = match.Notes,
                TeamId = match.TeamId,
                CreatedAt = match.CreatedAt,
                UpdatedAt = match.UpdatedAt
            },
            MatchPlayers = match.MatchPlayers.Select(mp => new MatchPlayerDto
            {
                Id = mp.Id,
                MatchId = mp.MatchId,
                PlayerId = mp.PlayerId,
                IsStarter = mp.IsStarter,
                Position = mp.Position,
                MinutesPlayed = mp.MinutesPlayed,
                Goals = mp.Goals,
                Assists = mp.Assists,
                YellowCards = mp.YellowCards,
                RedCards = mp.RedCards,
                Notes = mp.Notes,
                PlayerName = mp.Player?.Name ?? "Unknown"
            }).ToList()
        };

        return Ok(matchWithPlayersDto);
    }
}
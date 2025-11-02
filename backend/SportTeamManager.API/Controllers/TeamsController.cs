using Microsoft.AspNetCore.Mvc;
using SportTeamManager.Application.DTOs;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;

namespace SportTeamManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamRepository _teamRepository;

    public TeamsController(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
    {
        var teams = await _teamRepository.GetAllAsync();
        var teamDtos = teams.Select(t => new TeamDto
        {
            Id = t.Id,
            Name = t.Name,
            SportType = t.SportType,
            CoachName = t.CoachName,
            HomeField = t.HomeField,
            FoundedDate = t.FoundedDate,
            Colors = t.Colors,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        });

        return Ok(teamDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDto>> GetTeam(Guid id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            return NotFound();
        }

        var teamDto = new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            SportType = team.SportType,
            CoachName = team.CoachName,
            HomeField = team.HomeField,
            FoundedDate = team.FoundedDate,
            Colors = team.Colors,
            CreatedAt = team.CreatedAt,
            UpdatedAt = team.UpdatedAt
        };

        return Ok(teamDto);
    }

    [HttpPost]
    public async Task<ActionResult<TeamDto>> CreateTeam(CreateTeamDto createTeamDto)
    {
        var team = new Team
        {
            Id = Guid.NewGuid(),
            Name = createTeamDto.Name,
            SportType = createTeamDto.SportType,
            CoachName = createTeamDto.CoachName,
            HomeField = createTeamDto.HomeField,
            FoundedDate = createTeamDto.FoundedDate,
            Colors = createTeamDto.Colors,
            CreatedAt = DateTime.UtcNow
        };

        var createdTeam = await _teamRepository.AddAsync(team);

        var teamDto = new TeamDto
        {
            Id = createdTeam.Id,
            Name = createdTeam.Name,
            SportType = createdTeam.SportType,
            CoachName = createdTeam.CoachName,
            HomeField = createdTeam.HomeField,
            FoundedDate = createdTeam.FoundedDate,
            Colors = createdTeam.Colors,
            CreatedAt = createdTeam.CreatedAt,
            UpdatedAt = createdTeam.UpdatedAt
        };

        return CreatedAtAction(nameof(GetTeam), new { id = teamDto.Id }, teamDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(Guid id, UpdateTeamDto updateTeamDto)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            return NotFound();
        }

        team.Name = updateTeamDto.Name;
        team.CoachName = updateTeamDto.CoachName;
        team.HomeField = updateTeamDto.HomeField;
        team.Colors = updateTeamDto.Colors;
        team.UpdatedAt = DateTime.UtcNow;

        await _teamRepository.UpdateAsync(team);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(Guid id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            return NotFound();
        }

        await _teamRepository.DeleteAsync(team);

        return NoContent();
    }

    [HttpGet("{id}/players")]
    public async Task<ActionResult<TeamWithPlayersDto>> GetTeamWithPlayers(Guid id)
    {
        var team = await _teamRepository.GetTeamWithPlayersAsync(id);
        if (team == null)
        {
            return NotFound();
        }

        var teamWithPlayersDto = new TeamWithPlayersDto
        {
            Team = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                SportType = team.SportType,
                CoachName = team.CoachName,
                HomeField = team.HomeField,
                FoundedDate = team.FoundedDate,
                Colors = team.Colors,
                CreatedAt = team.CreatedAt,
                UpdatedAt = team.UpdatedAt
            },
            Players = team.Players.Select(p => new PlayerDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Position = p.Position,
                JerseyNumber = p.JerseyNumber,
                BirthDate = p.BirthDate,
                Height = p.Height,
                Weight = p.Weight,
                Nationality = p.Nationality,
                JoinDate = p.JoinDate,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList()
        };

        return Ok(teamWithPlayersDto);
    }
}
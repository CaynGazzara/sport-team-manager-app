using Microsoft.AspNetCore.Mvc;
using SportTeamManager.Application.DTOs;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using System.Numerics;

namespace SportTeamManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerRepository _playerRepository;

    public PlayersController(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
    {
        var players = await _playerRepository.GetAllAsync();
        var playerDtos = players.Select(p => new PlayerDto
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
        });

        return Ok(playerDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetPlayer(Guid id)
    {
        var player = await _playerRepository.GetByIdAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        var playerDto = new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            Age = player.Age,
            Position = player.Position,
            JerseyNumber = player.JerseyNumber,
            BirthDate = player.BirthDate,
            Height = player.Height,
            Weight = player.Weight,
            Nationality = player.Nationality,
            JoinDate = player.JoinDate,
            IsActive = player.IsActive,
            CreatedAt = player.CreatedAt,
            UpdatedAt = player.UpdatedAt
        };

        return Ok(playerDto);
    }

    [HttpPost]
    public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto)
    {
        var player = new Player
        {
            Id = Guid.NewGuid(),
            Name = createPlayerDto.Name,
            Age = createPlayerDto.Age,
            Position = createPlayerDto.Position,
            JerseyNumber = createPlayerDto.JerseyNumber,
            BirthDate = createPlayerDto.BirthDate,
            Height = createPlayerDto.Height,
            Weight = createPlayerDto.Weight,
            Nationality = createPlayerDto.Nationality,
            JoinDate = createPlayerDto.JoinDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createdPlayer = await _playerRepository.AddAsync(player);

        var playerDto = new PlayerDto
        {
            Id = createdPlayer.Id,
            Name = createdPlayer.Name,
            Age = createdPlayer.Age,
            Position = createdPlayer.Position,
            JerseyNumber = createdPlayer.JerseyNumber,
            BirthDate = createdPlayer.BirthDate,
            Height = createdPlayer.Height,
            Weight = createdPlayer.Weight,
            Nationality = createdPlayer.Nationality,
            JoinDate = createdPlayer.JoinDate,
            IsActive = createdPlayer.IsActive,
            CreatedAt = createdPlayer.CreatedAt,
            UpdatedAt = createdPlayer.UpdatedAt
        };

        return CreatedAtAction(nameof(GetPlayer), new { id = playerDto.Id }, playerDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(Guid id, UpdatePlayerDto updatePlayerDto)
    {
        var player = await _playerRepository.GetByIdAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        player.Name = updatePlayerDto.Name;
        player.Position = updatePlayerDto.Position;
        player.JerseyNumber = updatePlayerDto.JerseyNumber;
        player.Height = updatePlayerDto.Height;
        player.Weight = updatePlayerDto.Weight;
        player.IsActive = updatePlayerDto.IsActive;
        player.UpdatedAt = DateTime.UtcNow;

        await _playerRepository.UpdateAsync(player);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(Guid id)
    {
        var player = await _playerRepository.GetByIdAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        await _playerRepository.DeleteAsync(player);

        return NoContent();
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetActivePlayers()
    {
        var players = await _playerRepository.GetActivePlayersAsync();
        var playerDtos = players.Select(p => new PlayerDto
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
        });

        return Ok(playerDtos);
    }
}
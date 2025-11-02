using Microsoft.AspNetCore.Mvc;
using SportTeamManager.Application.DTOs;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;

namespace SportTeamManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamRepository _teamRepository; // ✅ ADICIONAR esta linha

    public PlayersController(IPlayerRepository playerRepository, ITeamRepository teamRepository) // ✅ ADICIONAR teamRepository
    {
        _playerRepository = playerRepository;
        _teamRepository = teamRepository; // ✅ ADICIONAR esta linha
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
            TeamId = p.TeamId, // ✅ ADICIONAR esta linha
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
            TeamId = player.TeamId, // ✅ ADICIONAR esta linha
            CreatedAt = player.CreatedAt,
            UpdatedAt = player.UpdatedAt
        };

        return Ok(playerDto);
    }

    [HttpPost]
    public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto)
    {
        try
        {
            // ✅ NORMALIZAR o TeamId para maiúsculo (corrigindo o erro de conversão)
            var teamIdString = createPlayerDto.TeamId.ToString();
            var normalizedTeamId = teamIdString.ToUpperInvariant();

            Console.WriteLine($"🎯 TeamId recebido do frontend: {createPlayerDto.TeamId}");
            Console.WriteLine($"🎯 TeamId normalizado: {normalizedTeamId}");

            // Verificar se o time existe
            var teamIdGuid = Guid.Parse(normalizedTeamId);
            var teamExists = await _teamRepository.ExistsAsync(teamIdGuid);
            if (!teamExists)
            {
                Console.WriteLine($"❌ Time não encontrado: {normalizedTeamId}");
                return BadRequest($"Time com ID {normalizedTeamId} não encontrado");
            }

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
                TeamId = teamIdGuid, // ✅ Usar GUID normalizado
                CreatedAt = DateTime.UtcNow
            };

            Console.WriteLine($"✅ Criando jogador para o time: {normalizedTeamId}");

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
                TeamId = createdPlayer.TeamId, // ✅ ADICIONAR esta linha
                CreatedAt = createdPlayer.CreatedAt,
                UpdatedAt = createdPlayer.UpdatedAt
            };

            return CreatedAtAction(nameof(GetPlayer), new { id = playerDto.Id }, playerDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao criar jogador: {ex.Message}");
            Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
            return StatusCode(500, "Erro interno do servidor");
        }
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
            TeamId = p.TeamId, // ✅ ADICIONAR esta linha
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });

        return Ok(playerDtos);
    }
}
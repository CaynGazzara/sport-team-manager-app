using SportTeamManager.Domain.Entities;
using System.Numerics;

namespace SportTeamManager.Application.Interfaces;

public interface IPlayerRepository : IRepository<Player>
{
    Task<IEnumerable<Player>> GetActivePlayersAsync();
    Task<IEnumerable<Player>> GetPlayersByPositionAsync(string position);
    Task<Player?> GetPlayerWithStatsAsync(Guid id);
}
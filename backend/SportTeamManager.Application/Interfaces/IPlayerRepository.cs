using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Application.Interfaces;

public interface IPlayerRepository : IRepository<Player>
{
    Task<IEnumerable<Player>> GetActivePlayersAsync();
    Task<IEnumerable<Player>> GetPlayersByPositionAsync(string position);
    Task<Player?> GetPlayerWithStatsAsync(Guid id);
    Task<IEnumerable<Player>> GetPlayersByTeamAsync(Guid teamId);
    Task<IEnumerable<Player>> SearchPlayersAsync(string searchTerm);
    Task<Player?> GetPlayerByJerseyNumberAsync(string jerseyNumber);
    Task<bool> JerseyNumberExistsAsync(string jerseyNumber, Guid? excludePlayerId = null);
}
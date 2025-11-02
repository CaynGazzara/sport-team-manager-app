using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Application.Interfaces;

public interface IMatchPlayerRepository : IRepository<MatchPlayer>
{
    Task<IEnumerable<MatchPlayer>> GetByMatchAsync(Guid matchId);
    Task<IEnumerable<MatchPlayer>> GetByPlayerAsync(Guid playerId);
    Task<MatchPlayer?> GetByMatchAndPlayerAsync(Guid matchId, Guid playerId);
    Task<IEnumerable<MatchPlayer>> GetStartersByMatchAsync(Guid matchId);
    Task<int> GetGoalsByPlayerAsync(Guid playerId, string? season = null);
    Task<int> GetAssistsByPlayerAsync(Guid playerId, string? season = null);
    Task<bool> PlayerExistsInMatchAsync(Guid matchId, Guid playerId);
}
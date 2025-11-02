using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Application.Interfaces;

public interface IPlayerStatisticRepository : IRepository<PlayerStatistic>
{
    Task<PlayerStatistic?> GetByPlayerAndSeasonAsync(Guid playerId, string season);
    Task<IEnumerable<PlayerStatistic>> GetByPlayerAsync(Guid playerId);
    Task<IEnumerable<PlayerStatistic>> GetBySeasonAsync(string season);
    Task<PlayerStatistic?> GetTopScorerAsync(string season);
    Task<PlayerStatistic?> GetTopAssisterAsync(string season);
    Task<decimal> GetAverageRatingAsync(Guid playerId, string? season = null);
    Task<bool> SeasonExistsForPlayerAsync(Guid playerId, string season);
}
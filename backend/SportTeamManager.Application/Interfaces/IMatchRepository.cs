using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Application.Interfaces;

public interface IMatchRepository : IRepository<Match>
{
    Task<IEnumerable<Match>> GetMatchesByTeamAsync(Guid teamId);
    Task<IEnumerable<Match>> GetMatchesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Match>> GetUpcomingMatchesAsync(int daysAhead = 7);
    Task<IEnumerable<Match>> GetMatchesByResultAsync(string result);
    Task<Match?> GetMatchWithPlayersAsync(Guid id);
    Task<Match?> GetMatchWithStatisticsAsync(Guid id);
    Task<IEnumerable<Match>> GetMatchesByOpponentAsync(string opponent);
}
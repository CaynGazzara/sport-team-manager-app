using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Application.Interfaces;

public interface ITeamRepository : IRepository<Team>
{
    Task<Team?> GetTeamWithPlayersAsync(Guid id);
    Task<Team?> GetTeamWithMatchesAsync(Guid id);
    Task<Team?> GetTeamWithTrainingsAsync(Guid id);
    Task<IEnumerable<Team>> GetTeamsBySportTypeAsync(string sportType);
    Task<Team?> GetTeamByNameAsync(string name);
    Task<bool> TeamNameExistsAsync(string name, Guid? excludeTeamId = null);
}
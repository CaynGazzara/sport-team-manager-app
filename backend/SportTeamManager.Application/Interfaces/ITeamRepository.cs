using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Application.Interfaces;

public interface ITeamRepository : IRepository<Team>
{
    Task<Team?> GetTeamWithPlayersAsync(Guid id);
    Task<Team?> GetTeamWithMatchesAsync(Guid id);
}
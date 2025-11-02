using Microsoft.EntityFrameworkCore;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using SportTeamManager.Infrastructure.Data;

namespace SportTeamManager.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Team?> GetByIdAsync(Guid id)
    {
        return await _context.Teams.FindAsync(id);
    }

    public async Task<IEnumerable<Team>> GetAllAsync()
    {
        return await _context.Teams.ToListAsync();
    }

    public async Task<Team> AddAsync(Team entity)
    {
        _context.Teams.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Team entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Team entity)
    {
        _context.Teams.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Teams.AnyAsync(t => t.Id == id);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Teams.CountAsync();
    }

    public async Task<Team?> GetTeamWithPlayersAsync(Guid id)
    {
        return await _context.Teams
            .Include(t => t.Players.Where(p => p.IsActive))
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Team?> GetTeamWithMatchesAsync(Guid id)
    {
        return await _context.Teams
            .Include(t => t.Matches)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Team?> GetTeamWithTrainingsAsync(Guid id)
    {
        return await _context.Teams
            .Include(t => t.Trainings)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Team>> GetTeamsBySportTypeAsync(string sportType)
    {
        return await _context.Teams
            .Where(t => t.SportType == sportType)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Team?> GetTeamByNameAsync(string name)
    {
        return await _context.Teams
            .FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<bool> TeamNameExistsAsync(string name, Guid? excludeTeamId = null)
    {
        if (excludeTeamId.HasValue)
        {
            return await _context.Teams
                .AnyAsync(t => t.Name == name && t.Id != excludeTeamId.Value);
        }

        return await _context.Teams.AnyAsync(t => t.Name == name);
    }
}
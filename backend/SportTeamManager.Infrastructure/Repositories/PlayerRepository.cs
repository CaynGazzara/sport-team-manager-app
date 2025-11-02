using Microsoft.EntityFrameworkCore;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using SportTeamManager.Infrastructure.Data;

namespace SportTeamManager.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> GetByIdAsync(Guid id)
    {
        return await _context.Players.FindAsync(id);
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<Player> AddAsync(Player entity)
    {
        _context.Players.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Player entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Player entity)
    {
        _context.Players.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Players.AnyAsync(p => p.Id == id);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Players.CountAsync();
    }

    public async Task<IEnumerable<Player>> GetActivePlayersAsync()
    {
        return await _context.Players
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Player>> GetPlayersByPositionAsync(string position)
    {
        return await _context.Players
            .Where(p => p.Position == position && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Player?> GetPlayerWithStatsAsync(Guid id)
    {
        return await _context.Players
            .Include(p => p.Statistics)
            .Include(p => p.Team)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Player>> GetPlayersByTeamAsync(Guid teamId)
    {
        return await _context.Players
            .Where(p => p.TeamId == teamId && p.IsActive)
            .OrderBy(p => p.Position)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Player>> SearchPlayersAsync(string searchTerm)
    {
        return await _context.Players
            .Where(p => p.Name.Contains(searchTerm) ||
                       p.Position.Contains(searchTerm) ||
                       p.JerseyNumber.Contains(searchTerm))
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Player?> GetPlayerByJerseyNumberAsync(string jerseyNumber)
    {
        return await _context.Players
            .FirstOrDefaultAsync(p => p.JerseyNumber == jerseyNumber && p.IsActive);
    }

    public async Task<bool> JerseyNumberExistsAsync(string jerseyNumber, Guid? excludePlayerId = null)
    {
        if (excludePlayerId.HasValue)
        {
            return await _context.Players
                .AnyAsync(p => p.JerseyNumber == jerseyNumber &&
                              p.Id != excludePlayerId.Value &&
                              p.IsActive);
        }

        return await _context.Players
            .AnyAsync(p => p.JerseyNumber == jerseyNumber && p.IsActive);
    }
}
using Microsoft.EntityFrameworkCore;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using SportTeamManager.Infrastructure.Data;

namespace SportTeamManager.Infrastructure.Repositories;

public class MatchPlayerRepository : IMatchPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public MatchPlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MatchPlayer?> GetByIdAsync(Guid id)
    {
        return await _context.MatchPlayers
            .Include(mp => mp.Player)
            .Include(mp => mp.Match)
            .FirstOrDefaultAsync(mp => mp.Id == id);
    }

    public async Task<IEnumerable<MatchPlayer>> GetAllAsync()
    {
        return await _context.MatchPlayers
            .Include(mp => mp.Player)
            .Include(mp => mp.Match)
            .ToListAsync();
    }

    public async Task<MatchPlayer> AddAsync(MatchPlayer entity)
    {
        _context.MatchPlayers.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(MatchPlayer entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MatchPlayer entity)
    {
        _context.MatchPlayers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.MatchPlayers.AnyAsync(mp => mp.Id == id);
    }

    public async Task<int> CountAsync()
    {
        return await _context.MatchPlayers.CountAsync();
    }

    public async Task<IEnumerable<MatchPlayer>> GetByMatchAsync(Guid matchId)
    {
        return await _context.MatchPlayers
            .Where(mp => mp.MatchId == matchId)
            .Include(mp => mp.Player)
            .OrderBy(mp => mp.IsStarter ? 0 : 1)
            .ThenBy(mp => mp.Position)
            .ToListAsync();
    }

    public async Task<IEnumerable<MatchPlayer>> GetByPlayerAsync(Guid playerId)
    {
        return await _context.MatchPlayers
            .Where(mp => mp.PlayerId == playerId)
            .Include(mp => mp.Match)
            .OrderByDescending(mp => mp.Match.Date)
            .ToListAsync();
    }

    public async Task<MatchPlayer?> GetByMatchAndPlayerAsync(Guid matchId, Guid playerId)
    {
        return await _context.MatchPlayers
            .Include(mp => mp.Player)
            .Include(mp => mp.Match)
            .FirstOrDefaultAsync(mp => mp.MatchId == matchId && mp.PlayerId == playerId);
    }

    public async Task<IEnumerable<MatchPlayer>> GetStartersByMatchAsync(Guid matchId)
    {
        return await _context.MatchPlayers
            .Where(mp => mp.MatchId == matchId && mp.IsStarter)
            .Include(mp => mp.Player)
            .OrderBy(mp => mp.Position)
            .ToListAsync();
    }

    public async Task<int> GetGoalsByPlayerAsync(Guid playerId, string? season = null)
    {
        var query = _context.MatchPlayers
            .Where(mp => mp.PlayerId == playerId && mp.Goals.HasValue);

        if (!string.IsNullOrEmpty(season))
        {
            query = query.Where(mp => mp.Match.Date.Year.ToString() == season);
        }

        return await query.SumAsync(mp => mp.Goals ?? 0);
    }

    public async Task<int> GetAssistsByPlayerAsync(Guid playerId, string? season = null)
    {
        var query = _context.MatchPlayers
            .Where(mp => mp.PlayerId == playerId && mp.Assists.HasValue);

        if (!string.IsNullOrEmpty(season))
        {
            query = query.Where(mp => mp.Match.Date.Year.ToString() == season);
        }

        return await query.SumAsync(mp => mp.Assists ?? 0);
    }

    public async Task<bool> PlayerExistsInMatchAsync(Guid matchId, Guid playerId)
    {
        return await _context.MatchPlayers
            .AnyAsync(mp => mp.MatchId == matchId && mp.PlayerId == playerId);
    }
}
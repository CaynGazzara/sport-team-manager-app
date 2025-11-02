using Microsoft.EntityFrameworkCore;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using SportTeamManager.Infrastructure.Data;

namespace SportTeamManager.Infrastructure.Repositories;

public class PlayerStatisticRepository : IPlayerStatisticRepository
{
    private readonly ApplicationDbContext _context;

    public PlayerStatisticRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerStatistic?> GetByIdAsync(Guid id)
    {
        return await _context.PlayerStatistics
            .Include(ps => ps.Player)
            .FirstOrDefaultAsync(ps => ps.Id == id);
    }

    public async Task<IEnumerable<PlayerStatistic>> GetAllAsync()
    {
        return await _context.PlayerStatistics
            .Include(ps => ps.Player)
            .ToListAsync();
    }

    public async Task<PlayerStatistic> AddAsync(PlayerStatistic entity)
    {
        _context.PlayerStatistics.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(PlayerStatistic entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PlayerStatistic entity)
    {
        _context.PlayerStatistics.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.PlayerStatistics.AnyAsync(ps => ps.Id == id);
    }

    public async Task<int> CountAsync()
    {
        return await _context.PlayerStatistics.CountAsync();
    }

    public async Task<PlayerStatistic?> GetByPlayerAndSeasonAsync(Guid playerId, string season)
    {
        return await _context.PlayerStatistics
            .Include(ps => ps.Player)
            .FirstOrDefaultAsync(ps => ps.PlayerId == playerId && ps.Season == season);
    }

    public async Task<IEnumerable<PlayerStatistic>> GetByPlayerAsync(Guid playerId)
    {
        return await _context.PlayerStatistics
            .Where(ps => ps.PlayerId == playerId)
            .OrderByDescending(ps => ps.Season)
            .ToListAsync();
    }

    public async Task<IEnumerable<PlayerStatistic>> GetBySeasonAsync(string season)
    {
        return await _context.PlayerStatistics
            .Where(ps => ps.Season == season)
            .Include(ps => ps.Player)
            .OrderByDescending(ps => ps.Goals)
            .ToListAsync();
    }

    public async Task<PlayerStatistic?> GetTopScorerAsync(string season)
    {
        return await _context.PlayerStatistics
            .Where(ps => ps.Season == season)
            .Include(ps => ps.Player)
            .OrderByDescending(ps => ps.Goals)
            .FirstOrDefaultAsync();
    }

    public async Task<PlayerStatistic?> GetTopAssisterAsync(string season)
    {
        return await _context.PlayerStatistics
            .Where(ps => ps.Season == season)
            .Include(ps => ps.Player)
            .OrderByDescending(ps => ps.Assists)
            .FirstOrDefaultAsync();
    }

    public async Task<decimal> GetAverageRatingAsync(Guid playerId, string? season = null)
    {
        var query = _context.PlayerStatistics
            .Where(ps => ps.PlayerId == playerId && ps.AverageRating > 0);

        if (!string.IsNullOrEmpty(season))
        {
            query = query.Where(ps => ps.Season == season);
        }

        return await query.AverageAsync(ps => ps.AverageRating);
    }

    public async Task<bool> SeasonExistsForPlayerAsync(Guid playerId, string season)
    {
        return await _context.PlayerStatistics
            .AnyAsync(ps => ps.PlayerId == playerId && ps.Season == season);
    }
}
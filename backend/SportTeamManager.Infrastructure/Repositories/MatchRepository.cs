using Microsoft.EntityFrameworkCore;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using SportTeamManager.Infrastructure.Data;

namespace SportTeamManager.Infrastructure.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly ApplicationDbContext _context;

    public MatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Match?> GetByIdAsync(Guid id)
    {
        return await _context.Matches.FindAsync(id);
    }

    public async Task<IEnumerable<Match>> GetAllAsync()
    {
        return await _context.Matches
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }

    public async Task<Match> AddAsync(Match entity)
    {
        _context.Matches.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Match entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Match entity)
    {
        _context.Matches.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Matches.AnyAsync(m => m.Id == id);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Matches.CountAsync();
    }

    public async Task<IEnumerable<Match>> GetMatchesByTeamAsync(Guid teamId)
    {
        return await _context.Matches
            .Where(m => m.TeamId == teamId)
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Match>> GetMatchesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Matches
            .Where(m => m.Date >= startDate && m.Date <= endDate)
            .OrderBy(m => m.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Match>> GetUpcomingMatchesAsync(int daysAhead = 7)
    {
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(daysAhead);

        return await _context.Matches
            .Where(m => m.Date >= startDate && m.Date <= endDate)
            .OrderBy(m => m.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Match>> GetMatchesByResultAsync(string result)
    {
        return await _context.Matches
            .Where(m => m.Result == result)
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }

    public async Task<Match?> GetMatchWithPlayersAsync(Guid id)
    {
        return await _context.Matches
            .Include(m => m.MatchPlayers)
                .ThenInclude(mp => mp.Player)
            .Include(m => m.Team)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Match?> GetMatchWithStatisticsAsync(Guid id)
    {
        return await _context.Matches
            .Include(m => m.MatchPlayers)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Match>> GetMatchesByOpponentAsync(string opponent)
    {
        return await _context.Matches
            .Where(m => m.Opponent.Contains(opponent))
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }
}
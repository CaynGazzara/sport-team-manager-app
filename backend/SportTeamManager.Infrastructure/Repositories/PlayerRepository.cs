using Microsoft.EntityFrameworkCore;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using SportTeamManager.Infrastructure.Data;
using System.Numerics;

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

    public async Task<IEnumerable<Player>> GetActivePlayersAsync()
    {
        return await _context.Players.Where(p => p.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Player>> GetPlayersByPositionAsync(string position)
    {
        return await _context.Players
            .Where(p => p.Position == position && p.IsActive)
            .ToListAsync();
    }

    public async Task<Player?> GetPlayerWithStatsAsync(Guid id)
    {
        return await _context.Players
            .Include(p => p.Statistics)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
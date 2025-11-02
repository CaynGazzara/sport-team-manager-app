using Microsoft.EntityFrameworkCore;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using SportTeamManager.Infrastructure.Data;

namespace SportTeamManager.Infrastructure.Repositories;

public class TrainingRepository : ITrainingRepository
{
    private readonly ApplicationDbContext _context;

    public TrainingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Training?> GetByIdAsync(Guid id)
    {
        return await _context.Trainings.FindAsync(id);
    }

    public async Task<IEnumerable<Training>> GetAllAsync()
    {
        return await _context.Trainings
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<Training> AddAsync(Training entity)
    {
        _context.Trainings.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Training entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Training entity)
    {
        _context.Trainings.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Trainings.AnyAsync(t => t.Id == id);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Trainings.CountAsync();
    }

    public async Task<IEnumerable<Training>> GetTrainingsByTeamAsync(Guid teamId)
    {
        return await _context.Trainings
            .Where(t => t.TeamId == teamId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Training>> GetTrainingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Trainings
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .OrderBy(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Training>> GetUpcomingTrainingsAsync(int daysAhead = 7)
    {
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(daysAhead);

        return await _context.Trainings
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .OrderBy(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Training>> GetTrainingsByFocusAsync(string focus)
    {
        return await _context.Trainings
            .Where(t => t.Focus == focus)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<Training?> GetTrainingWithAttendancesAsync(Guid id)
    {
        return await _context.Trainings
            .Include(t => t.Attendances)
                .ThenInclude(ta => ta.Player)
            .Include(t => t.Team)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> GetAttendanceCountAsync(Guid trainingId)
    {
        return await _context.TrainingAttendances
            .CountAsync(ta => ta.TrainingId == trainingId && ta.Attended);
    }
}
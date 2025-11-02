using Microsoft.EntityFrameworkCore;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;
using SportTeamManager.Infrastructure.Data;

namespace SportTeamManager.Infrastructure.Repositories;

public class TrainingAttendanceRepository : ITrainingAttendanceRepository
{
    private readonly ApplicationDbContext _context;

    public TrainingAttendanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TrainingAttendance?> GetByIdAsync(Guid id)
    {
        return await _context.TrainingAttendances
            .Include(ta => ta.Player)
            .Include(ta => ta.Training)
            .FirstOrDefaultAsync(ta => ta.Id == id);
    }

    public async Task<IEnumerable<TrainingAttendance>> GetAllAsync()
    {
        return await _context.TrainingAttendances
            .Include(ta => ta.Player)
            .Include(ta => ta.Training)
            .ToListAsync();
    }

    public async Task<TrainingAttendance> AddAsync(TrainingAttendance entity)
    {
        _context.TrainingAttendances.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(TrainingAttendance entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TrainingAttendance entity)
    {
        _context.TrainingAttendances.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.TrainingAttendances.AnyAsync(ta => ta.Id == id);
    }

    public async Task<int> CountAsync()
    {
        return await _context.TrainingAttendances.CountAsync();
    }

    public async Task<IEnumerable<TrainingAttendance>> GetByTrainingAsync(Guid trainingId)
    {
        return await _context.TrainingAttendances
            .Where(ta => ta.TrainingId == trainingId)
            .Include(ta => ta.Player)
            .OrderBy(ta => ta.Player.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<TrainingAttendance>> GetByPlayerAsync(Guid playerId)
    {
        return await _context.TrainingAttendances
            .Where(ta => ta.PlayerId == playerId)
            .Include(ta => ta.Training)
            .OrderByDescending(ta => ta.Training.Date)
            .ToListAsync();
    }

    public async Task<TrainingAttendance?> GetByTrainingAndPlayerAsync(Guid trainingId, Guid playerId)
    {
        return await _context.TrainingAttendances
            .Include(ta => ta.Player)
            .Include(ta => ta.Training)
            .FirstOrDefaultAsync(ta => ta.TrainingId == trainingId && ta.PlayerId == playerId);
    }

    public async Task<double> GetAttendanceRateAsync(Guid playerId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.TrainingAttendances
            .Where(ta => ta.PlayerId == playerId);

        if (startDate.HasValue)
        {
            query = query.Where(ta => ta.Training.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(ta => ta.Training.Date <= endDate.Value);
        }

        var totalTrainings = await query.CountAsync();
        var attendedTrainings = await query.CountAsync(ta => ta.Attended);

        return totalTrainings > 0 ? (double)attendedTrainings / totalTrainings * 100 : 0;
    }

    public async Task<int> GetPresentCountAsync(Guid trainingId)
    {
        return await _context.TrainingAttendances
            .CountAsync(ta => ta.TrainingId == trainingId && ta.Attended);
    }

    public async Task<int> GetAbsentCountAsync(Guid trainingId)
    {
        return await _context.TrainingAttendances
            .CountAsync(ta => ta.TrainingId == trainingId && !ta.Attended);
    }

    public async Task<bool> PlayerHasAttendanceAsync(Guid trainingId, Guid playerId)
    {
        return await _context.TrainingAttendances
            .AnyAsync(ta => ta.TrainingId == trainingId && ta.PlayerId == playerId);
    }
}
using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Application.Interfaces;

public interface ITrainingRepository : IRepository<Training>
{
    Task<IEnumerable<Training>> GetTrainingsByTeamAsync(Guid teamId);
    Task<IEnumerable<Training>> GetTrainingsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Training>> GetUpcomingTrainingsAsync(int daysAhead = 7);
    Task<IEnumerable<Training>> GetTrainingsByFocusAsync(string focus);
    Task<Training?> GetTrainingWithAttendancesAsync(Guid id);
    Task<int> GetAttendanceCountAsync(Guid trainingId);
}
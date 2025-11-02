using SportTeamManager.Domain.Entities;

namespace SportTeamManager.Application.Interfaces;

public interface ITrainingAttendanceRepository : IRepository<TrainingAttendance>
{
    Task<IEnumerable<TrainingAttendance>> GetByTrainingAsync(Guid trainingId);
    Task<IEnumerable<TrainingAttendance>> GetByPlayerAsync(Guid playerId);
    Task<TrainingAttendance?> GetByTrainingAndPlayerAsync(Guid trainingId, Guid playerId);
    Task<double> GetAttendanceRateAsync(Guid playerId, DateTime? startDate = null, DateTime? endDate = null);
    Task<int> GetPresentCountAsync(Guid trainingId);
    Task<int> GetAbsentCountAsync(Guid trainingId);
    Task<bool> PlayerHasAttendanceAsync(Guid trainingId, Guid playerId);
}
using Microsoft.AspNetCore.Mvc;
using SportTeamManager.Application.DTOs;
using SportTeamManager.Application.Interfaces;
using SportTeamManager.Domain.Entities;

namespace SportTeamManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingsController : ControllerBase
{
    private readonly ITrainingRepository _trainingRepository;

    public TrainingsController(ITrainingRepository trainingRepository)
    {
        _trainingRepository = trainingRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingDto>>> GetTrainings()
    {
        var trainings = await _trainingRepository.GetAllAsync();
        var trainingDtos = trainings.Select(t => new TrainingDto
        {
            Id = t.Id,
            Date = t.Date,
            Duration = t.Duration,
            Location = t.Location,
            Focus = t.Focus,
            Notes = t.Notes,
            TeamId = t.TeamId,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        });

        return Ok(trainingDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingDto>> GetTraining(Guid id)
    {
        var training = await _trainingRepository.GetByIdAsync(id);
        if (training == null)
        {
            return NotFound();
        }

        var trainingDto = new TrainingDto
        {
            Id = training.Id,
            Date = training.Date,
            Duration = training.Duration,
            Location = training.Location,
            Focus = training.Focus,
            Notes = training.Notes,
            TeamId = training.TeamId,
            CreatedAt = training.CreatedAt,
            UpdatedAt = training.UpdatedAt
        };

        return Ok(trainingDto);
    }

    [HttpPost]
    public async Task<ActionResult<TrainingDto>> CreateTraining(CreateTrainingDto createTrainingDto)
    {
        var training = new Training
        {
            Id = Guid.NewGuid(),
            Date = createTrainingDto.Date,
            Duration = createTrainingDto.Duration,
            Location = createTrainingDto.Location,
            Focus = createTrainingDto.Focus,
            Notes = createTrainingDto.Notes,
            TeamId = createTrainingDto.TeamId,
            CreatedAt = DateTime.UtcNow
        };

        var createdTraining = await _trainingRepository.AddAsync(training);

        var trainingDto = new TrainingDto
        {
            Id = createdTraining.Id,
            Date = createdTraining.Date,
            Duration = createdTraining.Duration,
            Location = createdTraining.Location,
            Focus = createdTraining.Focus,
            Notes = createdTraining.Notes,
            TeamId = createdTraining.TeamId,
            CreatedAt = createdTraining.CreatedAt,
            UpdatedAt = createdTraining.UpdatedAt
        };

        return CreatedAtAction(nameof(GetTraining), new { id = trainingDto.Id }, trainingDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTraining(Guid id, UpdateTrainingDto updateTrainingDto)
    {
        var training = await _trainingRepository.GetByIdAsync(id);
        if (training == null)
        {
            return NotFound();
        }

        training.Date = updateTrainingDto.Date;
        training.Duration = updateTrainingDto.Duration;
        training.Location = updateTrainingDto.Location;
        training.Focus = updateTrainingDto.Focus;
        training.Notes = updateTrainingDto.Notes;
        training.UpdatedAt = DateTime.UtcNow;

        await _trainingRepository.UpdateAsync(training);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTraining(Guid id)
    {
        var training = await _trainingRepository.GetByIdAsync(id);
        if (training == null)
        {
            return NotFound();
        }

        await _trainingRepository.DeleteAsync(training);

        return NoContent();
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<TrainingDto>>> GetUpcomingTrainings()
    {
        var trainings = await _trainingRepository.GetUpcomingTrainingsAsync();
        var trainingDtos = trainings.Select(t => new TrainingDto
        {
            Id = t.Id,
            Date = t.Date,
            Duration = t.Duration,
            Location = t.Location,
            Focus = t.Focus,
            Notes = t.Notes,
            TeamId = t.TeamId,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        });

        return Ok(trainingDtos);
    }

    [HttpGet("{id}/attendances")]
    public async Task<ActionResult<TrainingWithAttendancesDto>> GetTrainingWithAttendances(Guid id)
    {
        var training = await _trainingRepository.GetTrainingWithAttendancesAsync(id);
        if (training == null)
        {
            return NotFound();
        }

        var trainingWithAttendancesDto = new TrainingWithAttendancesDto
        {
            Training = new TrainingDto
            {
                Id = training.Id,
                Date = training.Date,
                Duration = training.Duration,
                Location = training.Location,
                Focus = training.Focus,
                Notes = training.Notes,
                TeamId = training.TeamId,
                CreatedAt = training.CreatedAt,
                UpdatedAt = training.UpdatedAt
            },
            Attendances = training.Attendances.Select(ta => new TrainingAttendanceDto
            {
                Id = ta.Id,
                TrainingId = ta.TrainingId,
                PlayerId = ta.PlayerId,
                Attended = ta.Attended,
                Notes = ta.Notes,
                Rating = ta.Rating,
                PlayerName = ta.Player?.Name ?? "Unknown"
            }).ToList()
        };

        return Ok(trainingWithAttendancesDto);
    }
}
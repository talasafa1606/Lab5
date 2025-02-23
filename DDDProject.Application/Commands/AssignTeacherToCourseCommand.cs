using MediatR;

namespace DDDProject.Application.Commands;
public class AssignTeacherToCourseCommand : IRequest<bool>
{
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
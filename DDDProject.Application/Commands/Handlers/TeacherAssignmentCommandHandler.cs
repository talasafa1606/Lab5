using DDDProject.Domain.Entities;
using DDDProject.Persistence.Data;
using MediatR;

namespace DDDProject.Application.Commands.Handlers;
public class TeacherAssignmentCommandHandler : IRequestHandler<TeacherAssignmentCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public TeacherAssignmentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(TeacherAssignmentCommand request, CancellationToken cancellationToken)
    {
        var teacherCourse = new TeacherCourse
        {
            TeacherId = request.TeacherId,
            CourseId = request.CourseId
        };

        var timeSlot = new TimeSlot
        {
            TeacherId = request.TeacherId,
            CourseId = request.CourseId,
            DayOfWeek = request.TimeSlot.DayOfWeek,
            StartTime = request.TimeSlot.StartTime,
            EndTime = request.TimeSlot.EndTime
        };

        _context.TeacherCourses.Add(teacherCourse);
        _context.TimeSlots.Add(timeSlot);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
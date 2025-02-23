namespace DDDProject.Application.Commands.Handlers;
using DDDProject.Application.Commands;
using DDDProject.Domain.Entities;
using DDDProject.Persistence.Data;
using MediatR;

public class AssignTeacherToCourseCommandHandler : IRequestHandler<AssignTeacherToCourseCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public AssignTeacherToCourseCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AssignTeacherToCourseCommand request, CancellationToken cancellationToken)
    {
        var timeSlot = new TimeSlot
        {
            TeacherId = request.TeacherId,
            CourseId = request.CourseId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        var teacherCourse = new TeacherCourse
        {
            TeacherId = request.TeacherId,
            CourseId = request.CourseId
        };

        _context.TimeSlots.Add(timeSlot);
        _context.TeacherCourses.Add(teacherCourse);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
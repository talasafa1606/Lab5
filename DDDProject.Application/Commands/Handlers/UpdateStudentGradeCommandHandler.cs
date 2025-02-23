using DDDProject.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DDDProject.Application.Commands.Handlers;

public class UpdateStudentGradeCommandHandler : IRequestHandler<UpdateStudentGradeCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public UpdateStudentGradeCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateStudentGradeCommand request, CancellationToken cancellationToken)
    {
        var studentCourse = await _context.StudentCourses
            .FirstOrDefaultAsync(sc => sc.StudentId == request.StudentId && sc.CourseId == request.CourseId);
        
        if (studentCourse == null) throw new Exception("Student is not enrolled in this course");

        studentCourse.Grade = request.Grade;

        var student = await _context.Students
            .Include(s => s.StudentCourses)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId);

        var averageGrade = student.StudentCourses
            .Where(sc => sc.Grade.HasValue)
            .Average(sc => sc.Grade.Value);

        student.GradeAverage = averageGrade;
        student.CanApplyToFrance = averageGrade > 15;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
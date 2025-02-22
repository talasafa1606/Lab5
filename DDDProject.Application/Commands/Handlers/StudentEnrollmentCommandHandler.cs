using DDDProject.Domain.Entities;
using DDDProject.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DDDProject.Application.Commands.Handlers;

public class StudentEnrollmentCommandHandler : IRequestHandler<StudentEnrollmentCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public StudentEnrollmentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(StudentEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FindAsync(request.CourseId);
        if (course == null) throw new Exception("Course not found");

        if (DateTime.UtcNow < course.EnrollmentStartDate || DateTime.UtcNow > course.EnrollmentEndDate)
            throw new Exception("Enrollment is not open for this course");

        var currentEnrollments = await _context.StudentCourses.CountAsync(sc => sc.CourseId == request.CourseId);
        if (currentEnrollments >= course.MaxStudents)
            throw new Exception("Course is full");

        var enrollment = new StudentCourse
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId
        };

        _context.StudentCourses.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
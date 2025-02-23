using DDDProject.Persistence.Data;
using MediatR;
using DDDProject.Application.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDDProject.Application.Handlers
{
    public class SetStudentGradeCommandHandler : IRequestHandler<SetStudentGradeCommand, Unit> // Ensure Unit is the return type
    {
        private readonly ApplicationDbContext _context;

        public SetStudentGradeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetStudentGradeCommand request, CancellationToken cancellationToken)
        {
            var student = await _context.Students.FindAsync(request.StudentId);
            var course = await _context.Courses.FindAsync(request.CourseId);

            if (student == null || course == null)
            {
                throw new Exception("Student or Course not found");
            }

            var studentCourse = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == request.StudentId && sc.CourseId == request.CourseId);

            if (studentCourse == null)
            {
                throw new Exception("Student is not enrolled in the course");
            }

            studentCourse.Grade = request.Grade;

            student.AddGrade(request.Grade);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
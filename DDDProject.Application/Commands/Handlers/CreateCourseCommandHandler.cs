using DDDProject.Application.Commands;
using DDDProject.Domain.Entities;
using DDDProject.Persistence.Data;
using MediatR;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateCourseCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            MaxStudents = request.MaxStudents,
            EnrollmentStartDate = request.EnrollmentStartDate,
            EnrollmentEndDate = request.EnrollmentEndDate,
            CreatedByAdminId = request.AdminId
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);
        return course.Id;
    }
}
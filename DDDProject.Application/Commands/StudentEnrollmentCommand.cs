using MediatR;

namespace DDDProject.Application.Commands;

public class StudentEnrollmentCommand : IRequest<bool>
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
}
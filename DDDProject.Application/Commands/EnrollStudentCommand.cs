using MediatR;

namespace DDDProject.Application.Commands;

public class EnrollStudentCommand : IRequest<bool>
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
}
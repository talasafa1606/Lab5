using MediatR;
namespace DDDProject.Application.Commands;

public class CreateCourseCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public int MaxStudents { get; set; }
    public DateTimeOffset EnrollmentStartDate { get; set; }
    public DateTimeOffset EnrollmentEndDate { get; set; }
    public Guid AdminId { get; set; }
}
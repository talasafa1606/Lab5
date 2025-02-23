namespace DDDProject.Application.Commands;
using MediatR;
public class UpdateStudentGradeCommand : IRequest<bool>
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public decimal Grade { get; set; }
}
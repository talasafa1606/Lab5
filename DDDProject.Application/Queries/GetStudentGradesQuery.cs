using MediatR;

namespace DDDProject.Application.Queries;

public record GetStudentGradesQuery(Guid StudentId) : IRequest<List<StudentGradeDto>>;

public class StudentGradeDto
{
    public string CourseName { get; set; }
    public decimal Grade { get; set; }
    public decimal GradeAverage { get; set; }
    public bool CanApplyToFrance { get; set; }
}
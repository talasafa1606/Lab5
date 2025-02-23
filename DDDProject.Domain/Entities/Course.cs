namespace DDDProject.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int MaxStudents { get; set; }
    public DateTimeOffset EnrollmentStartDate { get; set; }
    public DateTimeOffset EnrollmentEndDate { get; set; }
    public Guid CreatedByAdminId { get; set; }
    public Admin CreatedByAdmin { get; set; }
    public ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
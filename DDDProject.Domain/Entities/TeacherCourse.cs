namespace DDDProject.Domain.Entities;

public class TeacherCourse
{
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    
    public Teacher Teacher { get; set; }
    public Course Course { get; set; }
}
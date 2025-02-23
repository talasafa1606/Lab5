namespace DDDProject.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal GradeAverage { get; set; }
    public bool CanApplyToFrance { get; set; }
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    
    public void AddGrade(decimal grade)
    {
        var totalGrades = StudentCourses.Sum(sc => sc.Grade); 
        GradeAverage = (decimal)(totalGrades / StudentCourses.Count);

        CanApplyToFrance = GradeAverage > 15;
    }
}
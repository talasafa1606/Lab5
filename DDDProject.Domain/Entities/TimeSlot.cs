namespace DDDProject.Domain.Entities;

public class TimeSlot
{
    public Guid Id { get; set; }
    public Guid TeacherId { get; set; }
    public Guid? CourseId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    
    public Teacher Teacher { get; set; }
    public Course Course { get; set; }
}
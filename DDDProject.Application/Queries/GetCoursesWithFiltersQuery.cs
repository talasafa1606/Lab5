namespace DDDProject.Application.Queries;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int MaxStudents { get; set; }
    public DateTimeOffset EnrollmentStartDate { get; set; }
    public DateTimeOffset EnrollmentEndDate { get; set; }
    public ICollection<TimeSlotDto> TimeSlots { get; set; }
}

public class TimeSlotDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string TeacherName { get; set; }
}
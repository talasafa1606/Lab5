public class Teacher
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
}

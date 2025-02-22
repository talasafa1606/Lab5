public class Admin
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Course> CreatedCourses { get; set; } = new List<Course>();
}

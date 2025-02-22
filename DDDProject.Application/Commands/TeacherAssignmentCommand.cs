using DDDProject.Domain.Entities;
using MediatR;

namespace DDDProject.Application.Commands;

public class TeacherAssignmentCommand : IRequest<bool>
{
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    
    public TimeSlot TimeSlot { get; set; }
}
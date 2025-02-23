using System;
using MediatR;

namespace DDDProject.Application.Commands
{
    public class SetStudentGradeCommand : IRequest, IRequest<Unit>
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public decimal Grade { get; set; }
    }
}
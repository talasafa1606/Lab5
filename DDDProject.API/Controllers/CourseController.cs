using MediatR;
using Microsoft.AspNetCore.Mvc;
using DDDProject.Application.Commands;

namespace DDDProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand command)
        {
            var courseId = await _mediator.Send(command);
            return Ok(courseId);
        }
        
        [HttpPost("assign")]
        public async Task<IActionResult> AssignTeacher(TeacherAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent(StudentEnrollmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPut("grade")]
        public async Task<IActionResult> UpdateGrade(UpdateStudentGradeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
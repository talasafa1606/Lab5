using MediatR;
using Microsoft.AspNetCore.Mvc;
using DDDProject.Application.Commands;
using DDDProject.Application.Queries;
using DDDProject.Infrastructure.Caching;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using DDDProject.Persistence.Data;

namespace DDDProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;
        private readonly IRedisCachingService _redisCacheService;

        public CourseController(IRedisCachingService redisCacheService, ApplicationDbContext context)
        {
            _redisCacheService = redisCacheService;
            _context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand command)
        {
            var courseId = await _mediator.Send(command);
            return Ok(courseId);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignTeacher([FromBody] TeacherAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent([FromBody] StudentEnrollmentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("grade")]
        public async Task<IActionResult> UpdateGrade([FromBody] UpdateStudentGradeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            const string cacheKey = "courses";

            var cachedCourses = await _redisCacheService.GetAsync<List<CourseDto>>(cacheKey);
            if (cachedCourses != null)
            {
                return Ok(cachedCourses);
            }

            var courses = _context.Courses
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    MaxStudents = c.MaxStudents,
                    EnrollmentStartDate = c.EnrollmentStartDate,
                    EnrollmentEndDate = c.EnrollmentEndDate
                })
                .ToList();

            await _redisCacheService.SetAsync(cacheKey, courses);
            return Ok(courses);
        }
    
        [HttpPost("assign-teacher")]
        public async Task<IActionResult> AssignTeacherToCourse([FromBody] AssignTeacherToCourseCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

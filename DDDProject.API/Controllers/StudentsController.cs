using DDDProject.Application.Commands;
using DDDProject.Application.Queries;
using DDDProject.Infrastructure.Caching;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace DDDProject.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRedisCachingService _redisCacheService;

    public StudentsController(IMediator mediator, IRedisCachingService redisCacheService)
    {
        _mediator = mediator;
        _redisCacheService = redisCacheService;

    }

    [HttpPost("enroll")]
    public async Task<IActionResult> Enroll(EnrollStudentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}/grades")]
    public async Task<ActionResult<List<StudentGradeDto>>> GetGrades(Guid id)
    {
        string cacheKey = $"student_grades_{id}";

        var cachedGrades = await _redisCacheService.GetAsync<List<StudentGradeDto>>(cacheKey);
        if (cachedGrades != null)
        {
            return Ok(cachedGrades);
        }

        var query = new GetStudentGradesQuery(id);
        var result = await _mediator.Send(query);

        await _redisCacheService.SetAsync(cacheKey, result);
        return Ok(result);
    }
}
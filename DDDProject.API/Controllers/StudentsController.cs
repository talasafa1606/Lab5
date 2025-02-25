using DDDProject.Application.Commands;
using DDDProject.Application.Queries;
using DDDProject.Common.Resources;
using DDDProject.Infrastructure.Caching;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Localization;

namespace DDDProject.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRedisCachingService _redisCacheService;
    private readonly IStringLocalizer _localizer;

    public StudentsController(IMediator mediator, IRedisCachingService redisCacheService, IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _redisCacheService = redisCacheService;
        _localizer = localizer;
    }

    [HttpPost("enroll")]
    public async Task<IActionResult> Enroll(EnrollStudentCommand command)
    {
        var result = await _mediator.Send(command);

        if (result == null)
        {
            return BadRequest(new { message = _localizer["EnrollmentFailed"] });
        }

        return Ok(new { message = _localizer["EnrollmentSuccess"], data = result });
    }

    [HttpGet("{id}/grades")]
    public async Task<ActionResult<List<StudentGradeDto>>> GetGrades(Guid id)
    {
        try
        {
            string cacheKey = $"student_grades_{id}";

            var cachedGrades = await _redisCacheService.GetAsync<List<StudentGradeDto>>(cacheKey);
            if (cachedGrades != null)
            {
                return Ok(new { message = _localizer["GradesCached"], grades = cachedGrades });
            }

            var query = new GetStudentGradesQuery(id);
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                return NotFound(new { message = _localizer["GradesNotFound"] });
            }

            await _redisCacheService.SetAsync(cacheKey, result);

            return Ok(new { message = _localizer["GradesRetrieved"], grades = result });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetGrades: {ex.Message}");
            return StatusCode(500, new { message = _localizer["ServerError"], details = ex.Message });
        }
    }

}

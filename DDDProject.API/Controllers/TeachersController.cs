using DDDProject.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DDDProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeachersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("set-grade")]
        public async Task<IActionResult> SetGradeForStudent([FromBody] SetStudentGradeCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok("Grade updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
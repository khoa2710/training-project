using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TrainingProject.Data;
using TrainingProject.Data.Entities;
using TrainingProject.Models.Dtos;
using TrainingProject.Models.SubmitModels;
using TrainingProject.Services.Abstractions;

namespace TrainingProject.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request, ct);
            if (result is null)
                return Unauthorized(new { message = "Username or password is incorrect. Please try again" });

            return Ok(result);
        }
    }
}

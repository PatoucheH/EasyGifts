using EasyGiftsBackend.Application.Interfaces;
using EasyGiftsBackend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EasyGiftsBackend.Api.Controllers
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

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.Register(registerDto);
                return new OkObjectResult("User registered successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _authService.Login(loginDto);
                return Ok(new
                {
                    success = true,
                    message = "login successful",
                    user
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
        }
    }
}

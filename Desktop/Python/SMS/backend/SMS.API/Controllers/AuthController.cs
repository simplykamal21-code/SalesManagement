using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;
using SMS.API.Services.Interfaces;

namespace SMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
   
    public AuthController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        var (token, expiresAt) = _tokenService.GenerateToken(user);
        return Ok(new LoginResponseDto(token, user.Name, user.Username, expiresAt));
    }
}

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // Inject AuthService
    public AuthController(IAuthService authService) => _authService = authService;

    // Register a new doctor
    [HttpPost("register-doctor")]
    public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegisterDto dto)
    {
        var result = await _authService.RegisterDoctor(dto);
        if (result == null) return BadRequest("Username already exists");
        return Ok("Doctor registered");
    }

    // Login doctor and return JWT token
    [HttpPost("login-doctor")]
    public async Task<IActionResult> LoginDoctor([FromBody] DoctorLoginDto dto)
    {
        var token = await _authService.LoginDoctor(dto);
        if (token == null) return Unauthorized("Invalid credentials");
        return Ok(new { token });
    }
}

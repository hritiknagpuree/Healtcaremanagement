using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IDoctorRepository _doctorRepo;
    private readonly IConfiguration _config;

    public AuthService(IDoctorRepository doctorRepo, IConfiguration config)
    {
        _doctorRepo = doctorRepo;
        _config = config;
    }

    public async Task<string> RegisterDoctor(DoctorRegisterDto dto)
    {
        // Prevent registration if username already exists
        if (await _doctorRepo.DoctorExists(dto.Username)) return null;

        // Securely hash the password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var doctor = new Doctor
        {
            Name = dto.Name,
            Email = dto.Email,
            Username = dto.Username,
            PasswordHash = passwordHash,
            FullName = dto.FullName
        };

        await _doctorRepo.AddDoctor(doctor);
        return "Registered";
    }

    public async Task<string> LoginDoctor(DoctorLoginDto dto)
    {
        // Validate user credentials
        var user = await _doctorRepo.GetByUsername(dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        // Create JWT claims for the user
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Doctor")
        };

        // Create security key and credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Generate JWT token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

namespace webcnAPI.V1.Controllers;
using Microsoft.AspNetCore.Mvc;
using webcnAPI.Service;
using webcnAPI.Domain;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;


[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly NinjaService _ninjaService;

    public AuthenticationController(NinjaService ninjaService)
    {
        _ninjaService = ninjaService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserCredentials credentials)
    {
        if (credentials == null)
        {
            return BadRequest("Login request is null");
        }

        var loggedInUser = await _ninjaService.LoginUser(credentials.Username, credentials.Password);

        if (loggedInUser != null)
        {
            // Generate a JWT for authentication
            var token = GenerateToken(loggedInUser);
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid email or password");

        
    }

    [HttpGet("protected")]
    [Authorize]
    public IActionResult ProtectedRoute()
    {
            // Accessible only for authenticated users
        return Ok("This is a protected route!");
    }    

    private object GenerateToken(Ninja user)
    {
        // Use a library like System.IdentityModel.Tokens.Jwt to generate JWT
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("email", user.Email),
            new Claim("grade", user.Grade.ToString()),
            // Add additional claims if needed
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: "junks",
            audience: "junks",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1), // Set token expiration time
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    private string HashPassword(string passwordHash)
    {
        // Use a library like BCrypt.Net or Argon2.NET for password hashing
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hash = BCrypt.Net.BCrypt.HashPassword(passwordHash, salt);
        return hash;
           
    }
}


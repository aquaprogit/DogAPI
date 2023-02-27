using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using DogAPI.BLL.Services.Interfaces;
using DogAPI.Common.DTO;
using DogAPI.Common.Models;
using DogAPI.DAL.Entities;
using DogAPI.DAL.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DogAPI.BLL.Services;
public class AuthService : IAuthService
{
    private readonly UserRepository _repository;
    private readonly IPasswordHasher _hasher;
    private readonly JwtSettings _settings;

    public AuthService(UserRepository repo, IPasswordHasher hasher, IOptions<JwtSettings> settings)
    {
        _repository = repo;
        _hasher = hasher;
        _settings = settings.Value;
    }
    public async Task<AuthSuccessDTO> LoginAsync(LoginUserDTO user)
    {
        string hashedPassword = _hasher.HashPassword(user.Password);
        var existingUser = await _repository.FindByLoginAsync(user.Login);

        if (existingUser == null)
            throw new KeyNotFoundException(user.Login);

        if (existingUser.PasswordHash != hashedPassword)
            throw new UnauthorizedAccessException(user.Login);

        return new AuthSuccessDTO(GenerateJwtToken(existingUser));

    }
    public async Task<AuthSuccessDTO> RegisterAsync(RegisterUserDTO user)
    {
        string hashedPassword = _hasher.HashPassword(user.Password);
        var existingUser = await _repository.FindByLoginAsync(user.Login);

        if (existingUser != null)
            throw new InvalidOperationException(user.Login);

        var newUser = new User() {
            Login = user.Login,
            PasswordHash = hashedPassword,
        };
        _repository.Add(newUser);

        return new AuthSuccessDTO(GenerateJwtToken(newUser));
    }
    private string GenerateJwtToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }
}

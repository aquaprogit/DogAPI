using DogAPI.BLL.Services.Interfaces;

namespace DogAPI.BLL.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}

using SMS.API.Models;

namespace SMS.API.Services.Interfaces;

public interface ITokenService
{
    (string token, DateTime expiresAt) GenerateToken(User user);
}

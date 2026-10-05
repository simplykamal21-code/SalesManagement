namespace SMS.API.DTOs;

public record LoginRequestDto(string Username, string Password);

public record LoginResponseDto(string Token, string Name, string Username, DateTime ExpiresAt);

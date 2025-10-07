using BolsoCheio.Business.DTOs;

namespace BolsoCheio.Business.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);
        Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto registerRequest);
        Task<UserDto?> GetUserProfileAsync(int userId);
        Task<UserDto?> UpdateProfileAsync(int userId, UpdateProfileDto updateProfile);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePassword);
        Task<bool> ValidateTokenAsync(string token);
        string GenerateJwtToken(UserDto user);
    }
}
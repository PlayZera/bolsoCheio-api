using BolsoCheio.Business.DTOs;
using BolsoCheio.Business.Interfaces;
using BolsoCheio.Data.Interfaces;
using BolsoCheio.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BolsoCheio.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _userRepository.ValidateUserAsync(loginRequest.Email, loginRequest.Password);
            
            if (user == null || !user.IsActive)
                return null;

            await _userRepository.UpdateLastLoginAsync(user.Id);

            var userDto = MapToDto(user);
            var token = GenerateJwtToken(userDto);
            var expiresAt = DateTime.UtcNow.AddHours(24); // Token válido por 24 horas

            return new LoginResponseDto
            {
                Token = token,
                User = userDto,
                ExpiresAt = expiresAt,
                TokenType = "Bearer"
            };
        }

        public async Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto registerRequest)
        {
            // Verificar se o email já existe
            if (await _userRepository.EmailExistsAsync(registerRequest.Email))
                return null;

            var user = new User
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                PasswordHash = registerRequest.Password, // Será hasheado no repositório
                Phone = registerRequest.Phone,
                MonthlyIncome = registerRequest.MonthlyIncome,
                Currency = "BRL",
                IsActive = true,
                EmailVerified = false
            };

            var createdUser = await _userRepository.CreateAsync(user);
            var userDto = MapToDto(createdUser);
            var token = GenerateJwtToken(userDto);
            var expiresAt = DateTime.UtcNow.AddHours(24);

            return new LoginResponseDto
            {
                Token = token,
                User = userDto,
                ExpiresAt = expiresAt,
                TokenType = "Bearer"
            };
        }

        public async Task<UserDto?> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> UpdateProfileAsync(int userId, UpdateProfileDto updateProfile)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            user.Name = updateProfile.Name;
            user.Phone = updateProfile.Phone;
            user.MonthlyIncome = updateProfile.MonthlyIncome;
            user.Currency = updateProfile.Currency;
            user.ProfileImageUrl = updateProfile.ProfileImageUrl;

            var updatedUser = await _userRepository.UpdateAsync(user);
            return MapToDto(updatedUser);
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            // Verificar senha atual
            if (!BCrypt.Net.BCrypt.Verify(changePassword.CurrentPassword, user.PasswordHash))
                return false;

            // Atualizar senha
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "minha-chave-secreta-super-segura-para-jwt-tokens-bolso-cheio-app");
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public string GenerateJwtToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "minha-chave-secreta-super-segura-para-jwt-tokens-bolso-cheio-app");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                MonthlyIncome = user.MonthlyIncome,
                Currency = user.Currency,
                EmailVerified = user.EmailVerified,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }
    }
}
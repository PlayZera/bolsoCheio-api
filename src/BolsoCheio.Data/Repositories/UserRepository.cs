using BolsoCheio.Data.Interfaces;
using BolsoCheio.Data.Models;

namespace BolsoCheio.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        // Simulando um banco de dados em memória
        private static List<User> _users = new List<User>
        {
            new User 
            { 
                Id = 1, 
                Name = "João Silva", 
                Email = "joao@email.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Phone = "11999999999",
                MonthlyIncome = 5000.00m,
                EmailVerified = true
            },
            new User 
            { 
                Id = 2, 
                Name = "Maria Santos", 
                Email = "maria@email.com", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Phone = "11888888888",
                MonthlyIncome = 3500.00m,
                EmailVerified = true
            }
        };

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Task.FromResult(_users);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return await Task.FromResult(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return await Task.FromResult(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var exists = _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return await Task.FromResult(exists);
        }

        public async Task<User> CreateAsync(User entity)
        {
            entity.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(entity.PasswordHash);
            _users.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<User> UpdateAsync(User entity)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == entity.Id);
            if (existingUser != null)
            {
                existingUser.Name = entity.Name;
                existingUser.Email = entity.Email;
                existingUser.Phone = entity.Phone;
                existingUser.MonthlyIncome = entity.MonthlyIncome;
                existingUser.Currency = entity.Currency;
                existingUser.ProfileImageUrl = entity.ProfileImageUrl;
                existingUser.IsActive = entity.IsActive;
                existingUser.UpdatedAt = DateTime.UtcNow;
            }
            return await Task.FromResult(existingUser ?? entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var exists = _users.Any(u => u.Id == id);
            return await Task.FromResult(exists);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
            }
            await Task.CompletedTask;
        }
    }
}
using BolsoCheio.Data.Models;

namespace BolsoCheio.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> ValidateUserAsync(string email, string password);
        Task UpdateLastLoginAsync(int userId);
    }
}
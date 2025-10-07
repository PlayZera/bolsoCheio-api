using System.ComponentModel.DataAnnotations;

namespace BolsoCheio.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [StringLength(15)]
        public string? Phone { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime? LastLoginAt { get; set; }
        
        // Campos específicos para controle financeiro
        public decimal MonthlyIncome { get; set; } = 0;
        
        [StringLength(3)]
        public string Currency { get; set; } = "BRL";
        
        public bool EmailVerified { get; set; } = false;
        
        public string? ProfileImageUrl { get; set; }
    }
}
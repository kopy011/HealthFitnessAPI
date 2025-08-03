using System.ComponentModel.DataAnnotations;
using HealthFitnessAPI.Constants.Enums;
namespace HealthFitnessAPI.Entities
{
    public class User : AbstractEntity
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? DisplayName { get; set; }
        [Required]
        public Gender Gender { get; set; }
    }
}

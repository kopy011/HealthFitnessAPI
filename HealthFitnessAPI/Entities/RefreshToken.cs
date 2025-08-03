using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthFitnessAPI.Entities
{
    public class RefreshToken : AbstractEntity
    {
        [MaxLength(250)] public string? Token { get; set; }
        public DateTime Expiry { get; set; }
        public int UserId { get; set; }
    }
}
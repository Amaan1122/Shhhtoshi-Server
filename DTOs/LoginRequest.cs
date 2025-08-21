using System.ComponentModel.DataAnnotations;

namespace TalentMatch_AI.DTOs
{
    public class LoginRequest
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}

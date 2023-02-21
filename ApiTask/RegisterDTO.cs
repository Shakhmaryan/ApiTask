using System.ComponentModel.DataAnnotations;

namespace ApiTask
{
    public class RegisterDTO
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace CodePulse.API.Model.DTO
{
    public class LoginRequestDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required (ErrorMessage ="Please enter your password")]
        public string Password { get; set; }
    }
}

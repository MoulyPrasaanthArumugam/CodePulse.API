using System.ComponentModel.DataAnnotations;

namespace CodePulse.API.Model.DTO
{
    public class RegisterRequestDto
    {
        [RegularExpression(@"^(?!.*[_.]{2})[a-zA-Z0-9._]{5,20}(?<![_.])$", ErrorMessage = "Invalid username. Must be 5-20 characters long, and cannot start or end with periods or underscores")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must have at least 8 characters, including uppercase, lowercase, number, and special character.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

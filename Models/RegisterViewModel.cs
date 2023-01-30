using System.ComponentModel.DataAnnotations;

namespace fullstack_todo_app.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MinLength(5, ErrorMessage = "Username can be min 5 characters.")]
        [MaxLength(20, ErrorMessage = "Username can be max 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(5, ErrorMessage = "Password can be min 5 characters.")]
        [MaxLength(20, ErrorMessage = "Password can be max 20 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Re-Password is required.")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}

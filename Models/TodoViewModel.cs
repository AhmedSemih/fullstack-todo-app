using System.ComponentModel.DataAnnotations;

namespace fullstack_todo_app.Models
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Todo is required.")]
        [MaxLength(50, ErrorMessage = "Todo must be max 50 character.")]
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; } = false;
        public DateTime? Deadline { get; set; }
    }
}

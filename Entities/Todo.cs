using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fullstack_todo_app.Entities
{
    [Table("Todos")]
    public class Todo
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? Deadline { get; set; }
        public bool Status { get; set; } = true;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}

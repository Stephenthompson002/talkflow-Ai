using System.ComponentModel.DataAnnotations;

namespace TalkFlow.Api.Models
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string CustomerId { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System.ComponentModel.DataAnnotations;

namespace TalkFlow.Api.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ConversationId { get; set; } = Guid.NewGuid(); // FIX: Add default value
        public string Sender { get; set; } = ""; // customer | bot | agent
        public string Content { get; set; } = "";
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
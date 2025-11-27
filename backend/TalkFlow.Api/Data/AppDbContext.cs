using Microsoft.EntityFrameworkCore;
using TalkFlow.Api.Models;

namespace TalkFlow.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
        
        public DbSet<Conversation> Conversations { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Force PascalCase table and column names
            modelBuilder.Entity<Conversation>().ToTable("Conversations");
            modelBuilder.Entity<Conversation>().Property(c => c.Id).HasColumnName("Id");
            modelBuilder.Entity<Conversation>().Property(c => c.Title).HasColumnName("Title");
            modelBuilder.Entity<Conversation>().Property(c => c.CustomerId).HasColumnName("CustomerId");
            modelBuilder.Entity<Conversation>().Property(c => c.CreatedAt).HasColumnName("CreatedAt");
            
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<Message>().Property(m => m.Id).HasColumnName("Id");
            modelBuilder.Entity<Message>().Property(m => m.ConversationId).HasColumnName("ConversationId");
            modelBuilder.Entity<Message>().Property(m => m.Sender).HasColumnName("Sender");
            modelBuilder.Entity<Message>().Property(m => m.Content).HasColumnName("Content");
            modelBuilder.Entity<Message>().Property(m => m.SentAt).HasColumnName("SentAt");
        }
    }
}
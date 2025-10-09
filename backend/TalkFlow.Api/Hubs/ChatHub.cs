using Microsoft.AspNetCore.SignalR;
using TalkFlow.Api.Data;
using TalkFlow.Api.Models;

namespace TalkFlow.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _db;
        public ChatHub(AppDbContext db) { _db = db; }

        public async Task SendMessage(Guid conversationId, string sender, string message)
        {
            var msg = new Message { ConversationId = conversationId, Sender = sender, Content = message };
            _db.Messages.Add(msg);
            await _db.SaveChangesAsync();
            await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", msg);
        }

        public Task JoinConversation(Guid conversationId) => Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        public Task LeaveConversation(Guid conversationId) => Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
    }
}

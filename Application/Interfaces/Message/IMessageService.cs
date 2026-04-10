using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ViewModels.Messages;

namespace Application.Interfaces.Message
{
    public interface IMessageService
    {
        Task SendMessageAsync(SaveMessageViewModel vm);
        Task<List<MessageViewModel>> GetMessagesByPropertyAsync(string propertyId, string senderId, string receiverId);
        Task<List<ChatViewModel>> GetChatsForUserAsync(string userId);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Message;
using Application.ViewModels.Messages;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _genericRepository;
        private readonly IMessageRepository _messagRepository;
        private readonly IMapper _mapper;

        public MessageService (IGenericRepository<Message> genericRepository,
                                IMessageRepository messageRepository,                  
                                 IMapper mapper)
        {
            _genericRepository = genericRepository;
            _messagRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task SendMessageAsync(SaveMessageViewModel vm)
        {
            var message = _mapper.Map<Message>(vm);

            await _genericRepository.AddAsync(message);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<List<MessageViewModel>> GetMessagesByPropertyAsync(string propertyId, string senderId, string receiverId)
        {
            var messages = await _genericRepository
                            .FindAsync(m => 
                                m.PropertyId == propertyId &&
                                ((m.SenderId == senderId && m.ReceiverId == receiverId) ||
                                (m.SenderId == receiverId && m.ReceiverId == senderId)));

            return _mapper.Map<List<MessageViewModel>>(messages.OrderBy(m => m.DateSend));
        }

        public async Task<List<ChatViewModel>> GetChatsForUserAsync(string userId)
        {
            var chats = await _messagRepository.GetChatsForUserAsync(userId);

            var result = chats.Select(m => new ChatViewModel // Aquí uso Select manual en lugar de AutoMapper porque ChatViewModel tiene LastMessage y DateLastMessage que no mapean directamente a los campos de Message (Content y DateSend).
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                PropertyId = m.PropertyId,
                LastMessage = m.Content,
                DateLastMessage = m.DateSend
            }).ToList();

            return result;
        }
    }
}
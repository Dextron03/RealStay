using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Identity.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Message>> GetChatsForUserAsync(string userId)
        {
            var message = await _dbContext.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.DateSend)
                .ToListAsync();

            var chats = message
                .GroupBy(m => new
                {
                    m.PropertyId,
                    Pair = string.Compare(m.SenderId, m.ReceiverId) < 0
                        ? $"{m.SenderId}-{m.ReceiverId}"
                        : $"{m.ReceiverId}-{m.SenderId}"
                }).Select(g => g.First())
                .ToList();
            
            return chats;
        }       
    }
}
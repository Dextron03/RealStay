using System;

namespace Domain.Entities
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SenderId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public string PropertyId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DateSend { get; set; } = DateTime.Now;

        // Navigation Property
        public Property? Property { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels.Messages
{
    public class MessageViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required, MinLength(1), MaxLength(250)]
        public string Content { get; set; }
        
        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }
        
        [Required]
        public string PropertyId { get; set; }
        
        [Required]
        public DateTime DateSend { get; set; }
    }
}
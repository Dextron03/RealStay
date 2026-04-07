using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.ViewModels.Messages
{
    public class ChatViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string ReceiverId { get; set; }
        
        [Required]
        public string SenderId { get; set; }
        
        [Required]
        public string PropertyId { get; set; }

        [Required]
        public string LastMessage { get; set; }
        
        [Required]
        public DateTime DateLastMessage { get; set; }


    }
}
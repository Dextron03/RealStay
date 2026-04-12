using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels.Messages
{
    public class SaveMessageViewModel
    {
        public string? Id { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }

        [Required, MinLength(1)]
        public string Content { get; set; } = string.Empty;
        public string? PropertyId { get; set; }

    }
}
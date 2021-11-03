using System;
using System.Collections.Generic;

namespace RequestService.Dto
{
    public class RequestDto
    {
        public Guid Id { get; set; }

        public string Status { get; set; }

        public UserDto Author { get; set; }

        public UserDto Assignee { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public List<DocumentDto> Attachments { get; set; }
    }
}

using System;

namespace RequestService.Dto
{
    public class DocumentDto
    {
        public Guid Id { get; set; }

        public UserDto Assignee { get; set; }

        public bool IsConfirmed { get; set; }

        public string Url { get; set; }
    }
}

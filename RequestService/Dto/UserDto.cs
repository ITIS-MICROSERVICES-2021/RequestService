using System;

namespace RequestService.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public int TelegramId { get; set; }

        public bool IsActive { get; set; }

        public DateTime NextConfirmAt { get; set; }

        public string Role { get; set; }

        public Guid BossId { get; set; }

        public Guid DepartmentId { get; set; }
    }
}

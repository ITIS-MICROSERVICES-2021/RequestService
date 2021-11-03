using System.Collections.Generic;
using System.ComponentModel;

namespace RequestService.Dto
{
    public class VacationStatusDto : RequestDto
    {
        public string Step { get; set; }

        public List<UserDto> Colleagues { get; set; }
    }
}

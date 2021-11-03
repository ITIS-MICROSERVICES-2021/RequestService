using System;
using System.Threading.Tasks;
using RedisIO.Services;
using RequestService.Dto;

namespace RequestService.Services
{
    public class HandleVacationRequestService
    {
        private readonly IRedisIOService _redis;

        public HandleVacationRequestService(IRedisIOService redis)
        {
            _redis = redis;
        }

        public async Task Handle(object req = null)
        {
            var request = new
            {
                Step = "draft",
                From = "user1",
                To = "user2",
                Id = Guid.NewGuid()
            };

            var requestFromRedis = await _redis.GetAsync<VacationRequestDto>(request.Id.ToString());

            var isValid = requestFromRedis != null && requestFromRedis.Step == request.Step;

            if (isValid)
            {
                // send mq request to userservice
            }
            else
            {
                // .
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using RedisIO.Services;
using RequestService.Dto;

namespace RequestService.Services
{
    public class HandleVacationStatusService
    {
        private readonly IRedisIOService _redis;

        public HandleVacationStatusService(IRedisIOService redis)
        {
            _redis = redis;
        }

        public async Task Handle(object req = null)
        {
            var request = new
            {
                Step = "boss-request-generation",
                Id = Guid.NewGuid()
            };

            var requestFromRedis = await _redis.GetAsync<VacationStatusDto>(request.Id.ToString());

            var isValid = requestFromRedis != null && requestFromRedis.Step == request.Step;

            if (isValid)
            {
                var bossRequest = GenerateBossRequest(requestFromRedis);
                // send bossRequest request to mq
            }
            else
            {
                // .
            }
        }

        private string GenerateBossRequest(VacationStatusDto vacationStatus)
        {
            throw new NotImplementedException();
        }
    }
}

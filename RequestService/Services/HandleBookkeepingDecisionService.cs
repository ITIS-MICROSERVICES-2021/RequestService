using System;
using System.Threading.Tasks;
using RedisIO.Services;

namespace RequestService.Services
{
    public class HandleBookkeepingDecisionService
    {
        private readonly IRedisIOService _redis;
        public HandleBookkeepingDecisionService(IRedisIOService redis)
        {
            _redis = redis;
        }
        public async Task Handle(object req = null)
        {
            var request = new
            {
                Step = "review-accounting",
                Result = "approve",
                Id = Guid.NewGuid()
            };

            var requestFromRedis = await _redis.GetAsync<dynamic>(request.Id.ToString());

            var isValid = requestFromRedis != null && requestFromRedis.step == request.Step;

            if (isValid)
            {
                // place updated request to MQ
            }
            else
            {
                // place request with error result
            }
        }
    }
}
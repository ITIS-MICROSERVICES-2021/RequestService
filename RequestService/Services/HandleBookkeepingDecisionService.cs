using System;
using System.Threading.Tasks;
using CoreDTO.Redis.Vacation;
using RabbitMQ.Services;
using RedisIO.Services;

namespace RequestService.Services
{
    public class HandleBookkeepingDecisionService
    {
        private readonly IRedisIOService _redis;
        private readonly IRabbitMQService _rabbitMqService;

        public HandleBookkeepingDecisionService(IRedisIOService redis, IRabbitMQService rabbitMqService)
        {
            _redis = redis;
            _rabbitMqService = rabbitMqService;
        }
        public async Task Handle(VacationRequestDto request)
        {
            /*var request = new
            {
                Step = "review-accounting",
                Result = "approve",
                Id = Guid.NewGuid()
            };*/

            var requestFromRedis = await _redis.GetAsync<VacationRequestDto>(request.Id.ToString());

            var isValid = requestFromRedis != null && requestFromRedis.Status == request.Status;

            if (isValid)
            {
                request.Status = VacationRequestStatus.BossReview;
                await _redis.AddAsync(request.Id.ToString(), request);
                _rabbitMqService.Publish(request, "vacation", "templateService");
            }
            else
            {
                // place request with error result
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using CoreDTO.Redis.Vacation;
using RabbitMQ.Client;
using RabbitMQ.Services;
using RedisIO.Services;
using VacationRequestDto = CoreDTO.Redis.Vacation.VacationRequestDto;

namespace RequestService.Services
{
    public class HandleVacationRequestService
    {
        private readonly IRedisIOService _redis;
        private readonly IRabbitMQService _rabbitMqService;

        public HandleVacationRequestService(IRedisIOService redis, IRabbitMQService rabbitMqService)
        {
            _redis = redis;
            _rabbitMqService = rabbitMqService;
        }

        public async Task Handle(VacationRequestDto request)
        {
            /*var request = new VacationRequestDto()
            {
                Status = VacationRequestStatus.Started,
                Author = "user 1",
                StartAt = DateTimeOffset.Now,
                EndAt = DateTimeOffset.Now + TimeSpan.FromDays(7),
                Payload = new VacationPayload {DateFrom = DateTimeOffset.Now, DateTo = DateTimeOffset.Now + TimeSpan.FromDays(7)},
                Id = Guid.NewGuid()
            };*/

            var requestFromRedis = await _redis.GetAsync<VacationRequestDto>(request.Id.ToString());

            var isValid = requestFromRedis != null && requestFromRedis.Status == request.Status;

            if (isValid)
            {
                request.Status = VacationRequestStatus.BookKeepReview;
                await _redis.AddAsync(request.Id.ToString(), request);
                _rabbitMqService.Publish(request, ExchangeType.Direct, "userService");
            }
            else
            {
                // 
            }
        }
    }
}

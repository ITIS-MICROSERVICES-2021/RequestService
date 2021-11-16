using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDTO.Redis;
using CoreDTO.Redis.Vacation;
using RabbitMQ.Services;
using RedisIO.Services;

namespace RequestService.Services
{
    public class HandleBookkeepingRequestService
    {
        private readonly IRedisIOService _redisIoService;
        private readonly IRabbitMQService _rabbitMqService;

        public HandleBookkeepingRequestService(IRedisIOService redisIoService, IRabbitMQService rabbitMqService)
        {
            _redisIoService = redisIoService;
            _rabbitMqService = rabbitMqService;
        }

        public async Task PrepareRequestBookkeeping(VacationRequestDto request)
        {
            /*var input = new VacationRequestDto
            {
                Attachments = new Dictionary<string, byte[]>() {{"Docks", Array.Empty<byte>()}},
                Author = "Rabot'aga",
                Status = VacationRequestStatus.BossReview,
                Id = Guid.NewGuid()
            };*/

            var requestFromRedis = await _redisIoService.GetAsync<VacationRequestDto>(request.Id.ToString());

            var isValid = requestFromRedis != null && requestFromRedis.Id == request.Id &&
                          requestFromRedis.Status == request.Status && string.IsNullOrEmpty(request.RejectReason);

            if (isValid)
            {
                request.Status = VacationRequestStatus.Finished;
            }
            else
            {
                request.Status = VacationRequestStatus.Rejected;
            }
            
            await _redisIoService.AddAsync(request.Id.ToString(), request);
            _rabbitMqService.Publish(request, "BookKeeping", "templateService");
        }
    }
}
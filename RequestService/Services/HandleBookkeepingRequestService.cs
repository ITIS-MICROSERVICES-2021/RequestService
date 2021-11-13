using System;
using System.Threading.Tasks;
using RedisIO.Services;

namespace RequestService.Services
{
    public class HandleBookkeepingRequestService
    {
        private readonly RedisIOService _redisIoService;

        public HandleBookkeepingRequestService(RedisIOService redisIoService)
        {
            _redisIoService = redisIoService;
        }

        public async Task PrepareRequestBookkeeping(Object request)
        {
            var input = new
            {
                Docks = "Doc",
                From = "Rabot'aga",
                To = "Nachal'nik",
                RequestStatus = "на ревью у рука",
                RequestId = Guid.NewGuid()
            };

            var requestFromRedis = await _redisIoService.GetAsync<dynamic>(input.RequestId.ToString());

            var isValid = requestFromRedis != null && requestFromRedis.Id == input.RequestId &&
                          requestFromRedis.Status == input.RequestStatus;

            if (isValid)
            {
                // ..
            }
            else
            {
                // ..
            }
        }
    }
}
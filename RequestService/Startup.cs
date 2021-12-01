using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDTO.Redis.Vacation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using RabbitMQ.Extensions;
using RabbitMQ.Services;
using RedisIO.Converter;
using RedisIO.ServicesExtensions;
using RequestService.Services;
using StackExchange.Redis;

namespace RequestService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRabbitMQ();
            services.AddRedisIO<JsonRedisConverter>(builder =>
                builder
                    .UseJsonConverter()
                    .UseConfiguration(new ConfigurationOptions()
                    {
                        //Тут надо какой - то хост, взял пока из доков редиса
                        EndPoints = { "localhost:6379" }
                    }));
            services.AddTransient<HandleVacationRequestService>();
            services.AddLogging();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "RequestService", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRabbitMQService rabbitMqService,
            HandleVacationRequestService handleVacationRequestService, ILogger<HandleVacationRequestService> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RequestService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            ConfigureRabbitMq(rabbitMqService, handleVacationRequestService, logger);
        }

        private void ConfigureRabbitMq(IRabbitMQService rabbitMqService,
            HandleVacationRequestService handleVacationRequestService, ILogger<HandleVacationRequestService> logger)
        {
            rabbitMqService.Subscribe<VacationRequestDto>(handleVacationRequestService.Handle, ExchangeType.Direct,
                "request_service", logger);
        }
    }
}
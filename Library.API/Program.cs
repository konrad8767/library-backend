using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            CreateHostBuilder(args)
                .ConfigureServices(ConfigureMassTransit)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void ConfigureMassTransit(HostBuilderContext hostContext, IServiceCollection services)
        {
            IConfigurationSection rabbitConfig = hostContext.Configuration.GetSection("Rabbit");
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq(
                    (context, cfg) =>
                    {
                        cfg.Host(
                            rabbitConfig["HostName"],
                            rabbitConfig["VirtualHost"],
                            h => { h.Username(rabbitConfig["Username"]); h.Password(rabbitConfig["Password"]); });
                        cfg.ConfigureEndpoints(context);
                    });
            });
        }
    }
}

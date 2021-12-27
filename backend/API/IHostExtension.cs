using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MediatR;
using API;
using Persistence;
using Application.Seed;
using Microsoft.EntityFrameworkCore;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static IHost SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured during Database migration");
                }

                try
                {
                    var mapper = services.GetRequiredService<IMapper>();
                    var mediator = services.GetRequiredService<IMediator>();
                    mediator.Send(new SeedCommand()).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured during Database seed");
                }
            }
            return host;
        }
    }

}
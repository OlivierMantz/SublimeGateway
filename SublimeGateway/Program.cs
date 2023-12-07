using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Kubernetes;

namespace SublimeGateway
{
    //Tutorial:https://code-maze.com/dotnetcore-secure-microservices-jwt-ocelot/
    //With Auth: https://auth0.com/blog/implementing-api-gateway-in-aspnet-core-with-ocelot/#Aside--Securing-ASP-NET-Core-with-Auth0
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddOcelot(builder.Configuration).AddCacheManager(settings => settings.WithDictionaryHandle());
            //.AddKubernetes();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapControllers();

            app.UseOcelot().Wait();

            app.Run();
        }
    }
}
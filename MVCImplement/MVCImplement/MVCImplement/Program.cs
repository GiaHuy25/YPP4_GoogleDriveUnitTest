using Microsoft.Extensions.DependencyInjection;
using MVCImplement.Controllers;
using MVCImplement.Data;
using MVCImplement.Models;
using MVCImplement.Repositories;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.NewsService;
using MVCImplement.Services.UserService;

namespace MVCImplement
{
    class Program
    {
        private static readonly IServiceProvider _serviceProvider;
        
        static Program()
        {
            var services = new ServiceCollection()
                .AddSingleton<NewsDb>()
                .AddSingleton<IRepository<NewsModel>, Repository<NewsModel>>()
                .AddScoped<INewsService, NewsService>()
                .AddScoped<IAuthenService, AuthenService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<Controller>(sp => new Controller(
                    sp.GetRequiredService<INewsService>(),
                    sp.GetRequiredService<IAuthenService>()
                ));

            _serviceProvider = services.BuildServiceProvider();
        }

        static async Task Main(string[] args)
        {
            var controller = _serviceProvider.GetRequiredService<Controller>();

            var server = new HttpServer("http://localhost:8080/");
            server._router.AddRoute("/news", async context => await controller.Index(new HttpContextWrapper(context)));

            await server.StartAsync();
        }

    }
}
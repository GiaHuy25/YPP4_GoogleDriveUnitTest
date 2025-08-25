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
                .AddSingleton<IRepository<News>, Repository<News>>()
                .AddScoped<INewsService, NewsService>()
                .AddScoped<IAuthenService, AuthenService>()
                .AddSingleton<IUserService>(UserService.Instance)
                .AddTransient<Controller>(sp => new Controller(
                    sp.GetRequiredService<INewsService>(),
                    sp.GetRequiredService<IAuthenService>(),
                    sp.GetRequiredService<IUserService>()
                ));

            _serviceProvider = services.BuildServiceProvider();
        }

        static async Task Main(string[] args)
        {
            var controller = _serviceProvider.GetRequiredService<Controller>();

            var server = new HttpServer("http://localhost:8080/");

            // Add route for the News Index
            server._router.AddRoute("/news", async context => await controller.Index(new HttpContextWrapper(context)));

            // Add route for the GetUserInfo method
            server._router.AddRoute("/user/", async context => await controller.GetUserInfo(new HttpContextWrapper(context)));

            // Add route for the Get method with ID
            server._router.AddRoute("/news/get/", async context =>
            {
                var path = context.Request.Url.AbsolutePath;
                if (path.StartsWith("/news/get/") && int.TryParse(path.Substring(10), out var id))
                {
                    await controller.Get(new HttpContextWrapper(context), id);
                }
                else
                {
                    context.Response.StatusCode = 404;
                    await context.Response.OutputStream.WriteAsync(
                        System.Text.Encoding.UTF8.GetBytes("Not Found"));
                }
            });

            await server.StartAsync();
        }

    }
}
using Microsoft.Extensions.DependencyInjection;
using MVCImplement.Controllers;
using MVCImplement.Data;
using MVCImplement.Dtos;
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

                // Register controllers
                .AddTransient<NewsController>(sp => new NewsController(
                    sp.GetRequiredService<INewsService>(),
                    sp.GetRequiredService<IAuthenService>()
                ))
                .AddTransient<UserController>(sp => new UserController(
                    sp.GetRequiredService<IUserService>(),
                    sp.GetRequiredService<IAuthenService>()
                ));

            _serviceProvider = services.BuildServiceProvider();
        }

        static async Task Main(string[] args)
        {
            var newsController = _serviceProvider.GetRequiredService<NewsController>();
            var userController = _serviceProvider.GetRequiredService<UserController>();

            var server = new HttpServer("http://localhost:8080/");

            // News routes
            server._router.AddRoute("/news", async context =>
                await newsController.GetAll(new HttpContextWrapper(context)));

            server._router.AddRoute("/news/get/", async context =>
            {
                var path = context.Request.Url.AbsolutePath;
                if (path.StartsWith("/news/get/", StringComparison.OrdinalIgnoreCase))
                {
                    var idStr = path.Substring("/news/get/".Length).Trim('/');
                    if (int.TryParse(idStr, out var id))
                        await newsController.Get(new HttpContextWrapper(context), id);
                    else
                    {
                        var responseWrapper = new HttpResponseWrapper(context.Response);
                        await newsController.WriteResponse(responseWrapper, "{\"error\":\"Invalid ID\"}", 400);

                    }

                }
            });

            server._router.AddRoute("/news/create", async context =>
            {
                // TODO: parse body -> NewsDto
                var dto = new NewsDto { Title = "Sample", Content = "Content" };
                await newsController.Post(new HttpContextWrapper(context), dto);
            });

            // User routes
            server._router.AddRoute("/user/info", async context =>
                await userController.GetUserInfo(new HttpContextWrapper(context)));


            await server.StartAsync();
        }
    }
}
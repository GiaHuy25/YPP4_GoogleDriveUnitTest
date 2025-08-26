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
                .AddSingleton<IRepository<Users>, Repository<Users>>()
                .AddScoped<INewsService, NewsService>()
                .AddScoped<IAuthenService, AuthenService>()
                .AddScoped<IUserService, UserService>()

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
            server._router.AddRoute("/users", async context =>
                await userController.GetAllUsers(new HttpContextWrapper(context)));

            server._router.AddRoute("/users/", async context =>
            {
                var path = context.Request.Url.AbsolutePath;
                var idStr = path.Substring("/users/".Length).Trim('/');
                if (int.TryParse(idStr, out var id))
                    await userController.GetUserById(new HttpContextWrapper(context), id);
                else
                {
                    var responseWrapper = new HttpResponseWrapper(context.Response);
                    await userController.WriteResponse(responseWrapper, "{\"error\":\"Invalid ID\"}", 400);
                }
            });

            server._router.AddRoute("/users/add", async context =>
            {
                // TODO: parse body -> UserDto
                var dto = new UserDto { Username = "testuser", Email = "test@example.com", FullName = "Test User" };
                await userController.AddUser(new HttpContextWrapper(context), dto);
            });

            server._router.AddRoute("/users/update", async context =>
            {
                // TODO: parse body -> UserDto
                var dto = new UserDto { Id = 1, Username = "updateduser", Email = "updated@example.com", FullName = "Updated User" };
                await userController.UpdateUser(new HttpContextWrapper(context), dto);
            });

            server._router.AddRoute("/users/delete/{id}", async context =>
            {
                var path = context.Request.Url.AbsolutePath;
                var idStr = path.Substring("/users/delete/".Length).Trim('/');
                if (int.TryParse(idStr, out var id))
                    await userController.DeleteUser(new HttpContextWrapper(context), id);
                else
                {
                    var responseWrapper = new HttpResponseWrapper(context.Response);
                    await userController.WriteResponse(responseWrapper, "{\"error\":\"Invalid ID\"}", 400);
                }
            });

            await server.StartAsync();
        }
    }
}
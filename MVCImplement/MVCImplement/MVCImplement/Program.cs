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

            // Root route
            server._router.AddRoute("/", async context =>
            {
                string htmlFilePath = @"E:\BBV\YPP4\YPP4_GoogleDriveUnitTest\MVCImplement\MVCImplement\MVCImplement\View\index.html";
                Console.WriteLine($"Checking file: {htmlFilePath}, Exists: {File.Exists(htmlFilePath)}");
                if (File.Exists(htmlFilePath))
                {
                    var htmlContent = File.ReadAllText(htmlFilePath);
                    await WriteResponse(new HttpResponseWrapper(context.Response), htmlContent, 200, "text/html");
                }
                else
                {
                    await WriteResponse(new HttpResponseWrapper(context.Response), "HTML file not found at " + htmlFilePath, 404, "text/plain");
                }
            });
            // News routes
            server._router.AddRoute("/news", async context =>
                await newsController.GetAll(new HttpContextWrapper(context)));

            server._router.AddDynamicRoute("/news/{id}", async (idStr, context) =>
            {
                var wrapper = new HttpContextWrapper(context);
                if (int.TryParse(idStr, out var id))
                    await newsController.Get(wrapper, id);
                else
                {
                    var response = new HttpResponseWrapper(context.Response);
                    await new BaseController().WriteResponse(response, "{\"error\":\"Invalid ID\"}", 400);
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

            server._router.AddDynamicRoute("/users/{id}", async (idStr, context) =>
            {
                var wrapper = new HttpContextWrapper(context);
                if (int.TryParse(idStr, out var id))
                    await userController.GetUserById(wrapper, id);
                else
                {
                    var response = new HttpResponseWrapper(context.Response);
                    await new BaseController().WriteResponse(response, "{\"error\":\"Invalid ID\"}", 400);
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

            server._router.AddDynamicRoute("/users/delete/{id}", async (idStr, context) =>
            {
                var wrapper = new HttpContextWrapper(context);
                if (int.TryParse(idStr, out var id))
                    await userController.DeleteUser(wrapper, id);
                else
                {
                    var response = new HttpResponseWrapper(context.Response);
                    await new BaseController().WriteResponse(response, "{\"error\":\"Invalid ID\"}", 400);
                }
            });

            await server.StartAsync();
        }

        // Helper method to write response
        private static async Task WriteResponse(HttpResponseWrapper response, string content, int statusCode, string contentType)
        {
            response.StatusCode = statusCode;
            response.ContentType = contentType;
            await response.OutputStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(content));
            await response.OutputStream.FlushAsync();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using MVCImplement.Controllers;
using MVCImplement.Data;
using MVCImplement.Models;
using MVCImplement.Repositories;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.NewsService;
using MVCImplement.Services.UserService;
using System.Text.Json;
using System.Text;
using System.Net;

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
                try
                {
                    var path = context.Request.Url.AbsolutePath;
                    Console.WriteLine($"Request received in route handler: {path} at {DateTime.Now}");
                    if (path.StartsWith("/news/get/", StringComparison.OrdinalIgnoreCase))
                    {
                        string idStr = path.Length > 10 ? path.Substring(10).Trim('/') : ""; // Lấy phần sau /news/get/, bỏ /
                        if (int.TryParse(idStr, out var id))
                        {
                            Console.WriteLine($"Handling GET news with ID: {id} at {DateTime.Now}");
                            try
                            {
                                await controller.Get(new HttpContextWrapper(context), id); // Gọi Get với id
                                Console.WriteLine($"Route handler: Successfully called controller.Get for ID {id} at {DateTime.Now}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error calling controller.Get for ID {id}: {ex.Message}, StackTrace: {ex.StackTrace} at {DateTime.Now}");
                                if (!context.Response.OutputStream.CanWrite) return;
                                context.Response.StatusCode = 500;
                                context.Response.ContentType = "application/json";
                                var error = JsonSerializer.Serialize(new { error = "Internal server error" }, new JsonSerializerOptions
                                {
                                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                    WriteIndented = true
                                });
                                var buffer = Encoding.UTF8.GetBytes(error);
                                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                                await context.Response.OutputStream.FlushAsync();
                                //context.Response.Close();
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = 400; // Bad Request thay vì 404 cho ID không hợp lệ
                            context.Response.ContentType = "application/json";
                            var error = JsonSerializer.Serialize(new { error = "Invalid ID format" }, new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                WriteIndented = true
                            });
                            var buffer = Encoding.UTF8.GetBytes(error);
                            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                            await context.Response.OutputStream.FlushAsync();
                            Console.WriteLine($"Route handler: Sent 400 response for invalid ID format at {DateTime.Now}");
                            //context.Response.Close();
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.ContentType = "application/json";
                        var error = JsonSerializer.Serialize(new { error = "Route not found" }, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = true
                        });
                        var buffer = Encoding.UTF8.GetBytes(error);
                        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        await context.Response.OutputStream.FlushAsync();
                        Console.WriteLine($"Route handler: Sent 404 response for invalid route at {DateTime.Now}");
                        //context.Response.Close();
                    }
                }
                catch (HttpListenerException ex)
                {
                    Console.WriteLine($"HttpListenerException in Router: {ex.Message}, ErrorCode: {ex.ErrorCode} at {DateTime.Now}");
                    if (!context.Response.OutputStream.CanWrite) return;
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var error = JsonSerializer.Serialize(new { error = "Network connection error" }, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    });
                    var buffer = Encoding.UTF8.GetBytes(error);
                    await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    await context.Response.OutputStream.FlushAsync();
                    //context.Response.Close();
                }
            });
            await server.StartAsync();
        }
    }
}
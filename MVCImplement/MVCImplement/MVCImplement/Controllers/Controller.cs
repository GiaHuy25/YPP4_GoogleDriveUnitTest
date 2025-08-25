using MVCImplement.Dtos;
using MVCImplement.Models;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.NewsService;
using MVCImplement.Services.UserService;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MVCImplement.Controllers
{
    public class Controller
    {
        private readonly INewsService _newsService;
        private readonly IAuthenService _authService;
        private static Controller? _instance;
        private readonly IUserService _userService;

        public static Controller Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("Controller instance must be initialized via DI or set manually.");
                }
                return _instance;
            }
        }
        public IAuthenService GetAuthenService() => _authService;

        public INewsService GetNewsService() => _newsService;
        public Controller(INewsService newsService, IAuthenService authService, IUserService userService)
        {
            _newsService = newsService;
            _authService = authService;
            _userService = userService;
            if (_instance == null) _instance = this;
        }

        public async Task Index(IHttpContextWrapper context)
        {
            try
            {
                var news = _newsService.GetAllNews();
                Console.WriteLine($"News count: {news.Count}"); 
                context.Response.ContentType = "application/json"; 

                var json = JsonSerializer.Serialize(news, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
                    WriteIndented = true 
                });

                using var writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8);
                await writer.WriteAsync(json);
                await writer.FlushAsync(); 
                context.Response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                var error = JsonSerializer.Serialize(new { error = ex.Message });
                using var writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8);
                await writer.WriteAsync(error);
                await writer.FlushAsync();
                context.Response.Close();
            }
        }


        
        private string RenderNewsView(List<NewsDto> news)
        {
            var sb = new StringBuilder();
            sb.Append("<html><body><h1>News List</h1><ul>");
            foreach (var item in news)
            {
                sb.Append($"<li>{item.Title}: {item.Content} (Created: {item.CreatedAt})</li>");
            }
            sb.Append("</ul></body></html>");
            return sb.ToString();
        }

        public async Task GetUserInfo(IHttpContextWrapper context)
        {
            var items = context.GetType().GetProperty("Items")?.GetValue(context) as NameValueCollection;
            var username = items?["username"] ?? "Unknown";
            var userInfo = _userService.GetUserInfo(username);
            context.Response.ContentType = "text/html";
            context.Response.StatusCode = 200;
            using var writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8);
            await writer.WriteLineAsync(userInfo);
            context.Response.Close();
        }

        public async Task Get(IHttpContextWrapper context, int id)
        {
            Console.WriteLine($"Controller.Get called with ID: {id} at {DateTime.Now}");
            try
            {
                var news = _newsService.GetNewsById(id);
                if (news == null)
                {
                    Console.WriteLine($"Controller.Get: News with ID {id} not found at {DateTime.Now}");
                    var error = JsonSerializer.Serialize(new { error = "News not found" }, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    });
                    await WriteResponse(context.Response, error, 404, "application/json");
                    return;
                }

                Console.WriteLine($"Controller.Get: Found news with ID {id}, Title: {news.Title} at {DateTime.Now}");
                var json = JsonSerializer.Serialize(news, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
                await WriteResponse(context.Response, json, 200, "application/json");
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine($"HttpListenerException in Get: {ex.Message}, ErrorCode: {ex.ErrorCode}, Time: {DateTime.Now}");
                if (!context.Response.OutputStream.CanWrite)
                {
                    Console.WriteLine("OutputStream cannot write in Get at {DateTime.Now}");
                    return;
                }
                var error = JsonSerializer.Serialize(new { error = "Network connection error" }, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
                await WriteResponse(context.Response, error, 500, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get: {ex.Message}, StackTrace: {ex.StackTrace}, Time: {DateTime.Now}");
                if (!context.Response.OutputStream.CanWrite)
                {
                    Console.WriteLine("OutputStream cannot write in Get at {DateTime.Now}");
                    return;
                }
                var error = JsonSerializer.Serialize(new { error = ex.Message }, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
                await WriteResponse(context.Response, error, 500, "application/json");
            }
        }

        private async Task WriteResponse(IHttpResponseWrapper response, string content, int statusCode, string contentType = "application/json")
        {
            try
            {
                if (!response.OutputStream.CanWrite)
                {
                    Console.WriteLine("OutputStream cannot write in WriteResponse at {DateTime.Now}");
                    return;
                }
                response.StatusCode = statusCode;
                response.ContentType = contentType;
                var buffer = Encoding.UTF8.GetBytes(content);
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                await response.OutputStream.FlushAsync();
                Console.WriteLine($"WriteResponse: Sent {buffer.Length} bytes with status {statusCode} at {DateTime.Now}");
                response.Close();
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine($"HttpListenerException in WriteResponse: {ex.Message}, ErrorCode: {ex.ErrorCode}, Time: {DateTime.Now}");
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"ObjectDisposedException in WriteResponse: {ex.Message}, Time: {DateTime.Now}");
            }
        }

        public async Task Post(IHttpContextWrapper context, NewsDto newNews)
        {
            try
            {
                var news = new News
                {
                    Title = newNews.Title,
                    Content = newNews.Content,
                    CreatedAt = DateTime.Now
                };

                _newsService.AddNews(news);
                await WriteResponse(context.Response, "News created successfully", 201);
            }
            catch (Exception ex)
            {
                await WriteResponse(context.Response, $"Error: {ex.Message}", 500);
            }
        }

        public async Task Put(IHttpContextWrapper context, int id, NewsDto updatedNews)
        {
            var existingNews = _newsService.GetNewsById(id);
            if (existingNews == null)
            {
                await WriteResponse(context.Response, "News not found", 404);
                return;
            }

            try
            {
                var newsToUpdate = new News
                {
                    Id = existingNews.Id,
                    Title = updatedNews.Title,
                    Content = updatedNews.Content,
                    CreatedAt = existingNews.CreatedAt
                };

                _newsService.UpdateNews(newsToUpdate);

                await WriteResponse(context.Response, "News updated successfully", 200);
            }
            catch (Exception ex)
            {
                await WriteResponse(context.Response, $"Error: {ex.Message}", 500);
            }
        }

        public async Task Delete(IHttpContextWrapper context, int id)
        {
            var existingNews = _newsService.GetNewsById(id);
            if (existingNews == null)
            {
                await WriteResponse(context.Response, "News not found", 404);
                return;
            }

            try
            {
                _newsService.DeleteNews(id);
                await WriteResponse(context.Response, "News deleted successfully", 200);
            }
            catch (Exception ex)
            {
                await WriteResponse(context.Response, $"Error: {ex.Message}", 500);
            }
        }
    }
}

using MVCImplement.Dtos;
using MVCImplement.Models;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.NewsService;
using MVCImplement.Services.UserService;
using System.Collections.Specialized;
using System.Text;

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
            var news = _newsService.GetAllNews();
            context.Response.ContentType = "text/plain";
            var content = RenderNewsView(news);
            using var writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8);
            foreach (var item in news)
            {
                await writer.WriteLineAsync(item.Title);
            }
            context.Response.Close();
        }


        private Task WriteResponse(IHttpResponseWrapper response, string content, int statusCode) // Removed async
        {
            response.StatusCode = statusCode;
            response.ContentType = "text/html";
            var buffer = Encoding.UTF8.GetBytes(content);
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.Close();
            return Task.CompletedTask; // Explicitly return a completed task
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
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 200;
            using var writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8);
            await writer.WriteLineAsync(userInfo);
            context.Response.Close();
        }

        public async Task Get(IHttpContextWrapper context, int id)
        {
            var news = _newsService.GetNewsById(id);
            if (news == null)
            {
                await WriteResponse(context.Response, "News not found", 404);
                return;
            }

            var content = RenderNewsView(new List<NewsDto> { news });
            await WriteResponse(context.Response, content, 200);
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

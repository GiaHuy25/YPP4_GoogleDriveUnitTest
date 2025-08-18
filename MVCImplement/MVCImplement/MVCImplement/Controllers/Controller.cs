using MVCImplement.Dtos;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.NewsService;
using System.Text;

namespace MVCImplement.Controllers
{
    public class Controller
    {
        private readonly INewsService _newsService;
        private readonly IAuthenService _authService;
        private static Controller? _instance; // Nullable to fix CS8618

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
        public Controller(INewsService newsService, IAuthenService authService)
        {
            _newsService = newsService;
            _authService = authService;
            if (_instance == null) _instance = this;
        }

        public async Task Index(IHttpContextWrapper context)
        {
            if (!_authService.Authenticate("user", "pass"))
            {
                await WriteResponse(context.Response, "Unauthorized", 401);
                return;
            }

            var news = _newsService.GetAllNews();
            var viewContent = RenderNewsView(news);
            await WriteResponse(context.Response, viewContent, 200);
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
    }
}

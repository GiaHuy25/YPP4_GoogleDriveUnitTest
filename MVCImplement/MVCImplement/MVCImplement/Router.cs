using MVCImplement.Controllers;
using System.Net;
using System.Text.Json;
using System.Text;

namespace MVCImplement
{
    public class Router
    {
        private readonly Dictionary<string, Func<HttpListenerContext, Task>> _routes = new Dictionary<string, Func<HttpListenerContext, Task>>();

        public void AddRoute(string path, Func<HttpListenerContext, Task> handler)
        {
            _routes[path] = handler;
        }

        public async Task HandleRequest(HttpListenerContext context)
        {
            var path = context.Request.Url.AbsolutePath;
            Console.WriteLine($"Request received: {path} at {DateTime.Now}");

            try
            {
                if (path.StartsWith("/news/get/", StringComparison.OrdinalIgnoreCase) && _routes.ContainsKey("/news/get/"))
                {
                    Console.WriteLine($"Handling dynamic route: /news/get/ at {DateTime.Now}");
                    await _routes["/news/get/"](context);
                    return;
                }

                if (_routes.TryGetValue(path, out var handler))
                {
                    Console.WriteLine($"Handling route: {path} at {DateTime.Now}");
                    await handler(context);
                    return;
                }

                Console.WriteLine($"Route not found: {path} at {DateTime.Now}");
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
                context.Response.Close();
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine($"HttpListenerException in HandleRequest: {ex.Message}, ErrorCode: {ex.ErrorCode}, Time: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HandleRequest: {ex.Message}, StackTrace: {ex.StackTrace}, Time: {DateTime.Now}");
            }
        }
    }
}

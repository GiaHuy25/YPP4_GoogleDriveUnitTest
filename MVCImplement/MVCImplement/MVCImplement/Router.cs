using System.Net;
using System.Text;
using System.Text.Json;

namespace MVCImplement
{
    public class Router
    {
        private readonly Dictionary<string, Func<HttpListenerContext, Task>> _routes = new Dictionary<string, Func<HttpListenerContext, Task>>();
        private readonly Dictionary<string, Func<string, HttpListenerContext, Task>> _dynamicRoutes = new Dictionary<string, Func<string, HttpListenerContext, Task>>();

        public void AddRoute(string path, Func<HttpListenerContext, Task> handler)
        {
            _routes[path] = handler;
        }

        public void AddDynamicRoute(string path, Func<string, HttpListenerContext, Task> handler)
        {
            _dynamicRoutes[path] = handler;
        }

        public async Task HandleRequest(HttpListenerContext context)
        {
            var path = context.Request.Url.AbsolutePath;
            Console.WriteLine($"Request received: {path} at {DateTime.Now}");

            try
            {
                // Check static routes
                if (_routes.TryGetValue(path, out var handler))
                {
                    Console.WriteLine($"Handling route: {path} at {DateTime.Now}");
                    await handler(context);
                    return;
                }

                // Check dynamic routes
                foreach (var dynamicRoute in _dynamicRoutes)
                {
                    var basePath = dynamicRoute.Key.Replace("{id}", "").TrimEnd('/');
                    if (path.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
                    {
                        var idStr = path.Substring(basePath.Length).Trim('/');
                        if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out _))
                        {
                            Console.WriteLine($"Handling dynamic route: {dynamicRoute.Key} with id {idStr} at {DateTime.Now}");
                            await dynamicRoute.Value(idStr, context);
                            return;
                        }
                    }
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
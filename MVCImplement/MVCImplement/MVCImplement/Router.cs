using System.Net;

namespace MVCImplement
{
    public class Router
    {
        private readonly Dictionary<string, Func<HttpListenerContext, Task>> _routes = new();

        public void AddRoute(string path, Func<HttpListenerContext, Task> handler)
        {
            _routes[path] = handler;
        }

        public async Task HandleRequest(HttpListenerContext context)
        {
            var path = context.Request.Url.AbsolutePath;
            if (_routes.TryGetValue(path, out var handler))
            {
                await handler(context);
            }
            else if (path.StartsWith("/user/") && path.Length > 6) // Check for /user/ followed by username
            {
                var username = path.Substring(6); // Extract username after /user/
                var wrapper = new HttpContextWrapper(context); // Create wrapper
                wrapper.Items["username"] = username; // Set username in wrapper's Items
                if (_routes.TryGetValue("/user/", out var userHandler))
                {
                    // Pass the original context, but the wrapper is used in the controller via Program.cs
                    await userHandler(context);
                }
                else
                {
                    context.Response.StatusCode = 404;
                    await WriteResponse(context, "Not Found");
                }
            }
            else
            {
                context.Response.StatusCode = 404;
                await WriteResponse(context, "Not Found");
            }
        }

        private static async Task WriteResponse(HttpListenerContext context, string content)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
}

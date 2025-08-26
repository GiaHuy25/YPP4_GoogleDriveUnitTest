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
    public class BaseController
    {
        public async Task WriteResponse(IHttpResponseWrapper response, string content, int statusCode, string contentType = "application/json")
        {
            try
            {
                if (!response.OutputStream.CanWrite)
                    return;

                response.StatusCode = statusCode;
                response.ContentType = contentType;
                var buffer = Encoding.UTF8.GetBytes(content);
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                await response.OutputStream.FlushAsync();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WriteResponse error: {ex.Message}");
            }
        }
    }
}

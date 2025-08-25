using System.Collections.Specialized;
using System.Net;

namespace MVCImplement
{
    public interface IHttpContextWrapper
    {
        IHttpResponseWrapper Response { get; }
        NameValueCollection Items { get; }
    }

    public class HttpContextWrapper : IHttpContextWrapper
    {
        private readonly HttpListenerContext _context;

        public HttpContextWrapper(HttpListenerContext context)
        {
            _context = context;
        }

        public IHttpResponseWrapper Response => new HttpResponseWrapper(_context.Response);

        public NameValueCollection Items => _context.Request.QueryString;
    }
}

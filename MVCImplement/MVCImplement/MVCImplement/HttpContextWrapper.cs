using System.Collections.Specialized;
using System.Net;

namespace MVCImplement
{
    public interface IHttpContextWrapper
    {
        IHttpResponseWrapper Response { get; }
    }

    public class HttpContextWrapper : IHttpContextWrapper
    {
        private readonly HttpListenerContext _context;
        private NameValueCollection _items;

        public HttpContextWrapper(HttpListenerContext context)
        {
            _context = context;
            _items = new NameValueCollection();
        }

        public IHttpResponseWrapper Response => new HttpResponseWrapper(_context.Response);
        public HttpListenerRequest Request => _context.Request;
        public NameValueCollection Items => _items;
    }

}

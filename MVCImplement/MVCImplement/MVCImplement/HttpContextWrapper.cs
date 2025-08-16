using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MVCImplement
{
    public interface IHttpContextWrapper
    {
        IHttpResponseWrapper Response { get; }
    }

    public class HttpContextWrapper : IHttpContextWrapper
    {
        private readonly HttpListenerContext _context;

        public HttpContextWrapper(HttpListenerContext context)
        {
            _context = context;
        }

        public IHttpResponseWrapper Response => new HttpResponseWrapper(_context.Response);
    }

}

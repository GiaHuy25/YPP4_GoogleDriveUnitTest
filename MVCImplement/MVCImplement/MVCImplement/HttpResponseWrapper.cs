using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MVCImplement
{
    public interface IHttpResponseWrapper
    {
        int StatusCode { get; set; }
        Stream OutputStream { get; }
        string ContentType { get; set; }
        void Close();
    }

    public class HttpResponseWrapper : IHttpResponseWrapper
    {
        private readonly HttpListenerResponse _response;

        public HttpResponseWrapper(HttpListenerResponse response)
        {
            _response = response;
        }

        public int StatusCode
        {
            get => _response.StatusCode;
            set => _response.StatusCode = value;
        }

        public Stream OutputStream => _response.OutputStream;

        public string ContentType
        {
            get => _response.ContentType;
            set => _response.ContentType = value;
        }

        public void Close() => _response.Close();
    }

}

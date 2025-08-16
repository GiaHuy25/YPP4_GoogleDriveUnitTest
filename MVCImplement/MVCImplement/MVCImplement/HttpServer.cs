using MVCImplement;
using System.Net;

public class HttpServer
{
    private readonly HttpListener _listener;
    public readonly Router _router;

    public HttpServer(string prefix)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add(prefix);
        _router = new Router();
    }

    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine("Server started on http://localhost:8080/");
        while (true)
        {
            var context = await _listener.GetContextAsync();
            await _router.HandleRequest(context);
        }
    }
}
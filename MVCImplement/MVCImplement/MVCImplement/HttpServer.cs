using MVCImplement;
using System.Net;

public class HttpServer
{
    private readonly HttpListener _listener = new HttpListener();
    public readonly Router _router = new Router();
    private readonly string _url;

    public HttpServer(string url)
    {
        _url = url;
        _listener.Prefixes.Add(url);
    }

    public async Task StartAsync()
    {
        try
        {
            _listener.Start();
            Console.WriteLine($"Server started at {_url} at {DateTime.Now}");
            while (true)
            {
                var context = await _listener.GetContextAsync();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _router.HandleRequest(context);
                    }
                    catch (HttpListenerException ex)
                    {
                        Console.WriteLine($"HttpListenerException in HandleRequest: {ex.Message}, ErrorCode: {ex.ErrorCode}, Time: {DateTime.Now}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in HandleRequest: {ex.Message}, StackTrace: {ex.StackTrace}, Time: {DateTime.Now}");
                    }
                });
            }
        }
        catch (HttpListenerException ex)
        {
            Console.WriteLine($"HttpListenerException in StartAsync: {ex.Message}, ErrorCode: {ex.ErrorCode}, Time: {DateTime.Now}");
        }
    }
}
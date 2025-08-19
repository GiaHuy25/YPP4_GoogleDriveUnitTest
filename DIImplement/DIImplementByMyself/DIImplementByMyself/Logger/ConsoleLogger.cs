namespace DIImplementByMyself.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void log(string message)
        {
            Console.WriteLine($"[LOG]: {message}");
        }
    }
}
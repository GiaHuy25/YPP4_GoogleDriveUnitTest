using DIImplementByMyself;
using DIImplementByMyself.Logger;
using DIImplementByMyself.Worker;
class Program
{
    static void Main(string[] args)
    {
        var container = new SimpleContainer();
        container.Register<ILogger, ConsoleLogger>(Lifetime.Singleton);
        container.Register<IWorker, Worker>(Lifetime.Transient);
        var worker = container.Resolve<IWorker>();
        worker.DoWork();
        var anotherWorker = container.Resolve<IWorker>();
        anotherWorker.DoWork();

    }
}
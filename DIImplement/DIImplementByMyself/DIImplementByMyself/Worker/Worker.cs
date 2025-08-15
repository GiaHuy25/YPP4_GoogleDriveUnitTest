using DIImplementByMyself.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIImplementByMyself.Worker
{
    public class Worker : IWorker
    {
        private readonly ILogger _logger;

        public Worker(ILogger logger)
        {
            _logger = logger;
        }

        public void DoWork()
        {
            _logger.log("Worker is doing work.");
            System.Threading.Thread.Sleep(1000);
            _logger.log("Worker has finished work.");
        }
    }
}

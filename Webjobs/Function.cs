using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webjobs
{
    public class Function
    {
        public static void ProcessOrder([QueueTrigger("orders")] Order order, ILogger logger)
        {
            logger.LogInformation($"Order Has Been Received for {order.ResturantName}");
        }


        public static void TimerTrigger([TimerTrigger("0 */2 * * * *", RunOnStartup = true)] TimerInfo info, ILogger logger)
        {
            logger.LogInformation($"Trigger Ran at {DateTime.Now}");
        }
    }

    public class Order
    {
        public string ResturantName { get; set; }

        public string Price { get; set; }

        public bool IsDiscounted { get; set; }
    }
}

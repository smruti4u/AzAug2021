using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HttpTriggerFunctionApp
{
    public static class IOBindings
    {
        [FunctionName("IOBindings")]
        [return: Table("FunctionIOTable", Connection = "AzureWebJobsStorage")]
        public static TableEntity Run([TimerTrigger("0 */2 * * * *", RunOnStartup = true)]TimerInfo myTimer, [Blob("functionbucket/function.json",
            FileAccess.Read, Connection = "AzureWebJobsStorage")] Stream blob,
            ILogger log)
        {
            StreamReader reader = new StreamReader(blob);
            JObject content = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
            return new TableEntity { FileContent = content };

        }
    }

    public class TableEntity
    {
        public TableEntity()
        {
            this.PartitionKey = "IOBindings";
            this.RowKey = new Guid().ToString();
        }
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public JObject FileContent { get; set; }
    }
}

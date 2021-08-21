using System;
using System.Collections.Generic;
using System.Text;

namespace HttpTriggerFunctionApp.Models
{
    public class TodoItems
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime Created { get; set; }
    }
}

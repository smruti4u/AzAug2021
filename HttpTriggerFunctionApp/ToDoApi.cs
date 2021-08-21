using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HttpTriggerFunctionApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HttpTriggerFunctionApp
{
    public static class ToDoApi
    {
        static List<TodoItems> items = new List<TodoItems>();

        [FunctionName("GetTodos")]
        public static async Task<IActionResult> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequest req,
            ILogger log)
        {
            var result = items.Where(x => x.IsCompleted == false)?.ToList();
            return new OkObjectResult(result);
        }

        [FunctionName("Create")]
        public static async Task<IActionResult> Create(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks")] HttpRequest req,
    ILogger log)
        {
            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateTodoModel>(reqBody);

            TodoItems newItem = new TodoItems();
            newItem.Id = Guid.NewGuid().ToString();
            newItem.IsCompleted = false;
            newItem.Description = input.Description;

            items.Add(newItem);

            return new OkObjectResult(newItem);

        }

        [FunctionName("GetTodoById")]
        public static async Task<IActionResult> GetTodoById(
[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks/{id}")] HttpRequest req,
ILogger log, string id)
        {
            var currentItem = items.Where(x => x.Id == id).FirstOrDefault();
            if(currentItem == null)
            {
                return new NotFoundObjectResult($"Item with Id {id} not present in db");
            }

            return new OkObjectResult(currentItem);
        }

        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
[HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "tasks/{id}")] HttpRequest req,
ILogger log, string id)
        {
            var currentItem = items.Where(x => x.Id == id).FirstOrDefault();
            if (currentItem == null)
            {
                return new NotFoundObjectResult($"Item with Id {id} not present in db");
            }

            currentItem.IsCompleted = true;
            return new OkObjectResult(currentItem);
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace FunctionApp2.Searches
{
    interface ISearchFunction
    {
        IActionResult Searchhero([HttpTrigger(AuthorizationLevel.Function, new[] { "get" }, Route = "search/{name}")] HttpRequest req, string name);
    }
}
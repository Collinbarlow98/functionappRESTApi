using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;

namespace FunctionApp2.Sidekicks
{
    interface ISidekickFunction
    {
        Task<IActionResult> AddSidekickAsync([HttpTrigger(AuthorizationLevel.Function, new[] { "post" }, Route = "addSidekick")] HttpRequest req);
        IActionResult DeleteSidekick([HttpTrigger(AuthorizationLevel.Function, new[] { "delete" }, Route = "sidekick/{id}")] HttpRequest req, int id);
        IActionResult GetHeroSidekicks([HttpTrigger(AuthorizationLevel.Function, new[] { "get" }, Route = "hero/{id}/sidekicks")] HttpRequest req, int id);
        IActionResult GetSidekick([HttpTrigger(AuthorizationLevel.Function, new[] { "get" }, Route = "sidekick/{id}")] HttpRequest req, int id);
        IActionResult Getsidekicks([HttpTrigger(AuthorizationLevel.Function, new[] { "get" }, Route = "sidekicks")] HttpRequest req);
        Task<IActionResult> UpdateSidekick([HttpTrigger(AuthorizationLevel.Function, new[] { "put" }, Route = "sidekick/{id}")] HttpRequest req, int id);
    }
}
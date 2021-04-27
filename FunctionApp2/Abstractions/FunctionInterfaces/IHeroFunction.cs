using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;

namespace FunctionApp2.Heroes
{
    interface IHeroFunction
    {
        Task<IActionResult> AddHero([HttpTrigger(AuthorizationLevel.Function, new[] { "post" }, Route = "add")] HttpRequest req);
        IActionResult DeleteHero([HttpTrigger(AuthorizationLevel.Function, new[] { "delete" }, Route = "hero/{id}")] HttpRequest req, int id);
        IActionResult GetHero([HttpTrigger(AuthorizationLevel.Function, new[] { "get" }, Route = "hero/{id}")] HttpRequest req, int id);
        IActionResult GetHeroes([HttpTrigger(AuthorizationLevel.Function, new[] { "get" }, Route = "heroes")] HttpRequest req);
        Task<IActionResult> UpdateHero([HttpTrigger(AuthorizationLevel.Function, new[] { "put" }, Route = "hero/{id}")] HttpRequest req, int id);
    }
}
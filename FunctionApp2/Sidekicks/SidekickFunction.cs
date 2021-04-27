using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using FunctionApp2.QueryInterfaces;
using System.IO;
using FunctionApp2.Models;
using Newtonsoft.Json;

namespace FunctionApp2.Sidekicks
{
    class SidekickFunction : ISidekickFunction
    {
        private readonly ISidekickSqlQueries _sidekickSqlQueries;

        public SidekickFunction(ISidekickSqlQueries sidekickSqlQueries)
        {
            this._sidekickSqlQueries = sidekickSqlQueries;
        }

        [FunctionName("addSidekickFunction")]
        public async Task<IActionResult> AddSidekickAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "addSidekick")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TohSidekick>(requestBody);
            var sidekick = new TohSidekick() { Name = input.Name, Likes = 1, HeroId = input.HeroId };
            var newSidekick = _sidekickSqlQueries.addSidekick(sidekick);

            return new OkObjectResult(newSidekick);
        }
            
        [FunctionName("getSidekicksFunction")]
        public IActionResult Getsidekicks([HttpTrigger(AuthorizationLevel.Function, "get", Route = "sidekicks")] HttpRequest req)
        {
            var sidekicks = _sidekickSqlQueries.getSidekicks();

            return new OkObjectResult(sidekicks);
        }

        [FunctionName("getHeroSidekicksFunction")]
        public IActionResult GetHeroSidekicks([HttpTrigger(AuthorizationLevel.Function, "get", Route = "hero/{id}/sidekicks")] HttpRequest req, int id)
        {
            var sidekicks = _sidekickSqlQueries.getHeroSidekicks(id);

            return new OkObjectResult(sidekicks);
        }

        [FunctionName("getSidekickFunction")]
        public IActionResult GetSidekick([HttpTrigger(AuthorizationLevel.Function, "get", Route = "sidekick/{id}")] HttpRequest req, int id)
        {
            var sidekick = _sidekickSqlQueries.getSidekick(id);

            return new OkObjectResult(sidekick);
        }

        [FunctionName("deleteSidekickFunction")]
        public IActionResult DeleteSidekick([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "sidekick/{id}")] HttpRequest req, int id)
        {
            _sidekickSqlQueries.deleteSidekick(id);

            return new OkResult();
        }

        [FunctionName("updateSidekickFunction")]
        public async Task<IActionResult> UpdateSidekick([HttpTrigger(AuthorizationLevel.Function, "put", Route = "sidekick/{id}")] HttpRequest req, int id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TohSidekick>(requestBody);

            var sidekick = _sidekickSqlQueries.updateSidekick(id, updated);

            return new OkObjectResult(sidekick);
        }
    }
}

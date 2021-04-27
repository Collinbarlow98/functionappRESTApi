using System.IO;
using System.Threading.Tasks;
using FunctionApp2.Models;
using FunctionApp2.QueryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace FunctionApp2.Heroes
{
    public class HeroFunction : IHeroFunction
    {
        private readonly IHeroSqlQueries _heroSqlQueries;

        public HeroFunction(IHeroSqlQueries heroSqlQueries)
        {
            this._heroSqlQueries = heroSqlQueries;
        }

        [FunctionName("addHeroFunction")]
        public async Task<IActionResult> AddHero([HttpTrigger(AuthorizationLevel.Function, "post", Route = "add")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TohHero>(requestBody);
            var hero = new TohHero() { Name = input.Name, Likes = 1 };
            var newHero = _heroSqlQueries.addHero(hero);

            return new OkObjectResult(newHero);
        }

        [FunctionName("getHeroesFunction")]
        public IActionResult GetHeroes([HttpTrigger(AuthorizationLevel.Function, "get", Route = "heroes")] HttpRequest req)
        {
            var heroes = _heroSqlQueries.getHeroes();

            return new OkObjectResult(heroes);
        }

        [FunctionName("getHeroFunction")]
        public IActionResult GetHero([HttpTrigger(AuthorizationLevel.Function, "get", Route = "hero/{id}")] HttpRequest req, int id)
        {
            var hero = _heroSqlQueries.getHero(id);

            return new OkObjectResult(hero);
        }

        [FunctionName("updateHeroFunction")]
        public async Task<IActionResult> UpdateHero([HttpTrigger(AuthorizationLevel.Function, "put", Route = "hero/{id}")] HttpRequest req, int id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TohHero>(requestBody);

            var hero = _heroSqlQueries.updateHero(id, updated);

            return new OkObjectResult(hero);
        }

        [FunctionName("deleteHeroFunction")]
        public IActionResult DeleteHero([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "hero/{id}")] HttpRequest req, int id)
        {
            _heroSqlQueries.deleteHero(id);

            return new OkResult();
        }
    }
}

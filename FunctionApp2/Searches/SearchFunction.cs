using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Collections.Generic;
using FunctionApp2.Models;
using Azure.Search.Documents.Models;

namespace FunctionApp2.Searches
{
    class SearchFunction : ISearchFunction
    {
        private readonly Search _search;

        public SearchFunction(Search search)
        {
            this._search = search;
        }

        [FunctionName("searchFunction")]
        public IActionResult Searchhero([HttpTrigger(AuthorizationLevel.Function, "get", Route = "search/{name}")] HttpRequest req, string name)
        {
                var results = _search.SearchHeroes(name);
                var heroes = new List<TohHero>();

                foreach (SearchResult<TohHero> result in results.GetResults())
                {
                    heroes.Add(result.Document);
                }
                return new OkObjectResult(heroes);
        }
    }
}

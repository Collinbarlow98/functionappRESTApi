using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FunctionApp2.Heroes;
using FunctionApp2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NLog.Fluent;

namespace FunctionApp2.Tests.HeroTests
{
    class HeroTest
    {
        private HeroFunction _heroFunction;
        public async void AddHeroTest(HeroFunction heroFunction)
        {
            this._heroFunction = heroFunction;
            var req = new TohHero()
            {
                Name = "Flame Boy",
                Likes = 1
            };
            var query = new Dictionary<String, StringValues>();
            var body = JsonConvert.SerializeObject(req, Formatting.Indented);
            var result = await _heroFunction.AddHero(HttpRequestSetup(query, body));

            var resultObject = (OkObjectResult)result;

            var resultResponse = new TohHero()
            {
                Name = "Flame Boy",
                Likes = 1
            };
            var resultBody = JsonConvert.SerializeObject(resultResponse, Formatting.Indented);
            Assert.AreEqual(resultBody, resultObject.Value);
        }

        private HttpRequest HttpRequestSetup(Dictionary<string, StringValues> query, string body)
        {
            throw new NotImplementedException();
        }
    }
}

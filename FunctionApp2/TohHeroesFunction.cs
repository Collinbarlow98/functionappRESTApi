using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using ToHApi.Models;
using System.Data.SqlClient;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace ApiFunctionAppFrontEnd
{
    public class TohHeroesFunction
    {
        readonly string str = Environment.GetEnvironmentVariable("sqldb_connection");

        [FunctionName("addHeroFunction")]
        public async Task<IActionResult> AddHero(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "add")] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TohHero>(requestBody);
            var hero = new TohHero() { Name = input.Name, Likes = 1};
            var text = $"INSERT INTO heroes VALUES (@Id, @Name, @Likes)";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", System.Data.SqlDbType.VarChar) { Value = hero.Id },
                    new SqlParameter("@Name", System.Data.SqlDbType.VarChar) { Value = hero.Name },
                    new SqlParameter("@Likes", System.Data.SqlDbType.Int) { Value = hero.Likes }
                };
                cmd.Parameters.AddRange(param.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return new OkObjectResult(hero);
                }
            }
        }

        [FunctionName("getHeroesFunction")]
        public IActionResult GetHeroes(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "heroes")] HttpRequest req)
        {
            var text = "SELECT * FROM heroes";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var Heroes = new List<TohHero>();
                    while (reader.Read())
                    {
                        var hero = new TohHero
                        {
                            Id = (string)reader[0],
                            Name = (string)reader[1],
                            Likes = (int)reader[2]
                        };
                        Heroes.Add(hero);
                    }
                    return new OkObjectResult(Heroes);
                }
            }
        }

        [FunctionName("getHeroFunction")]
        public IActionResult GetHero(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "hero/{id}")] HttpRequest req, string id)
        {
            var text = "Select * FROM heroes WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Id";
                param.Value = id;
                cmd.Parameters.Add(param);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var hero = new TohHero();
                    while (reader.Read())
                    {
                        hero.Id = (string)reader[0];
                        hero.Name = (string)reader[1];
                        hero.Likes = (int)reader[2];
                    }
                    return new OkObjectResult(hero);
                }
            }
        }

        [FunctionName("updateHeroFunction")]
        public async Task<IActionResult> UpdateHero(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "hero/{id}")] HttpRequest req, string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TohHero>(requestBody);
            var text = $"UPDATE heroes SET Name = @Name WHERE Id = @Id";
            using (SqlConnection conn = new SqlConnection(str))
            {

                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", System.Data.SqlDbType.VarChar) { Value = id },
                    new SqlParameter("@Name", System.Data.SqlDbType.VarChar) { Value = updated.Name }
                };
                cmd.Parameters.AddRange(param.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var hero = new TohHero
                    {
                        Id = id,
                        Name = updated.Name,
                        Likes = updated.Likes
                    };
                    return new OkObjectResult(hero);
                }
            }
        }

        [FunctionName("deleteHeroFunction")]
        public IActionResult DeleteHero(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "hero/{id}")] HttpRequest req, string id)
        {
            var text = $"DELETE FROM heroes WHERE Id = @Id";
            using (SqlConnection conn = new SqlConnection(str))
            {

                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Id";
                param.Value = id;
                cmd.Parameters.Add(param);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return new OkResult();
                }
            }
        }

        [FunctionName("searchHeroesFunction")]
        public IActionResult Searchhero(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "search/{name}")] HttpRequest req, string name)
        {
            var results = FunctionApp2.Models.Search.Main(name);

            var heroes = new List<TohHero>();

            foreach (SearchResult<TohHero> result in results.GetResults())
            {
                heroes.Add(result.Document);
            }

            return new OkObjectResult(heroes);

            /*var text = $"SELECT * FROM heroes WHERE CHARINDEX(@Name, NAME) > 0";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Name";
                param.Value = name;
                cmd.Parameters.Add(param);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var Heroes = new List<TohHero>();
                    while (reader.Read())
                    {
                        var hero = new TohHero
                        {
                            Id = (string)reader[0],
                            Name = (string)reader[1],
                            Likes = (int)reader[2]
                        };
                        Heroes.Add(hero);
                    }
                    return new OkObjectResult(Heroes);
                }
            }*/
        }

        // Sidekicks

        [FunctionName("addSidekickFunction")]
        public async Task<IActionResult> AddSidekick(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "addSidekick")] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TohSidekick>(requestBody);
            var sidekick = new TohSidekick() { Name = input.Name, Likes = 1, Hero = input.Hero };
            var text = $"INSERT INTO sidekicks VALUES (@Id, @Name, @Likes, @Hero)";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", System.Data.SqlDbType.VarChar) { Value = sidekick.Id },
                    new SqlParameter("@Name", System.Data.SqlDbType.VarChar) { Value = sidekick.Name },
                    new SqlParameter("@Likes", System.Data.SqlDbType.Int) { Value = sidekick.Likes },
                    new SqlParameter("@Hero", System.Data.SqlDbType.VarChar) { Value = sidekick.Hero }
                };
                cmd.Parameters.AddRange(param.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return new OkObjectResult(sidekick);
                }
            }
        }

        [FunctionName("getSidekicksFunction")]
        public IActionResult Getsidekicks(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "sidekicks")] HttpRequest req)
        {
            var text = "SELECT * FROM sidekicks";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var Sidekicks = new List<TohSidekick>();
                    while (reader.Read())
                    {
                        var sidekick = new TohSidekick
                        {
                            Id = (string)reader[0],
                            Name = (string)reader[1],
                            Likes = (int)reader[2]
                        };
                        Sidekicks.Add(sidekick);
                    }
                    return new OkObjectResult(Sidekicks);
                }
            }
        }

        [FunctionName("getHeroSidekicksFunction")]
        public IActionResult GetHeroSidekicks(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "hero/{id}/sidekicks")] HttpRequest req, string id)
        {
            var text = "SELECT * FROM sidekicks WHERE Hero = @Id";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Id";
                param.Value = id;
                cmd.Parameters.Add(param);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var Sidekicks = new List<TohSidekick>();
                    while (reader.Read())
                    {
                        var sidekick = new TohSidekick
                        {
                            Id = (string)reader[0],
                            Name = (string)reader[1],
                            Likes = (int)reader[2]
                        };
                        Sidekicks.Add(sidekick);
                    }
                    return new OkObjectResult(Sidekicks);
                }
            }
        }

        [FunctionName("getSidekickFunction")]
        public IActionResult GetSidekick(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "sidekick/{id}")] HttpRequest req, string id)
        {
            var text = "Select * FROM sidekicks WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Id";
                param.Value = id;
                cmd.Parameters.Add(param);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var sidekick = new TohSidekick();
                    while (reader.Read())
                    {
                        sidekick.Id = (string)reader[0];
                        sidekick.Name = (string)reader[1];
                        sidekick.Likes = (int)reader[2];
                        sidekick.Hero = (string)reader[3];
                    }
                    return new OkObjectResult(sidekick);
                }
            }
        }

        [FunctionName("deleteSidekickFunction")]
        public IActionResult DeleteSidekick(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "sidekick/{id}")] HttpRequest req, string id)
        {
            var text = $"DELETE FROM sidekicks WHERE Id = @Id";
            using (SqlConnection conn = new SqlConnection(str))
            {

                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Id";
                param.Value = id;
                cmd.Parameters.Add(param);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return new OkResult();
                }
            }
        }

        [FunctionName("updateSidekickFunction")]
        public async Task<IActionResult> UpdateSidekick(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "sidekick/{id}")] HttpRequest req, string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TohHero>(requestBody);
            var text = $"UPDATE sidekicks SET Name = @Name WHERE Id = @Id";
            using (SqlConnection conn = new SqlConnection(str))
            {

                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", System.Data.SqlDbType.VarChar) { Value = id },
                    new SqlParameter("@Name", System.Data.SqlDbType.VarChar) { Value = updated.Name }
                };
                cmd.Parameters.AddRange(param.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var sidekick = new TohSidekick
                    {
                        Id = id,
                        Name = updated.Name,
                        Likes = updated.Likes
                    };
                    return new OkObjectResult(sidekick);
                }
            }
        }

    }
}


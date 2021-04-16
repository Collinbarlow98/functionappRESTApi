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

namespace ApiFunctionAppFrontEnd
{
    public class TohHeroesFunction
    {
        string str = Environment.GetEnvironmentVariable("sqldb_connection");

        [FunctionName("addHeroFunction")]
        public async Task<IActionResult> addHero(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "add")] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TohHero>(requestBody);
            var hero = new TohHero() { Name = input.Name, Likes = 1 };
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
        public IActionResult getHeroes(
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
        public IActionResult getHero(
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
        public async Task<IActionResult> updateHero(
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
        public IActionResult deleteHero(
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
        public IActionResult searchhero(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "search/{name}")] HttpRequest req, string name)
        {
            var text = $"SELECT * FROM heroes WHERE CHARINDEX(@Name, NAME) > 0";

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
            }
        }
    }
}


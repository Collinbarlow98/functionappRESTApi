using System.Collections.Generic;
using System.Data.SqlClient;
using FunctionApp2.QueryInterfaces;
using FunctionApp2.Models;
using Microsoft.Extensions.Options;
using FunctionApp2.Options;

namespace FunctionApp2.Heroes
{
    public class HeroSqlQueries : IHeroSqlQueries
    {
        private readonly MyOptions _settings;

        public HeroSqlQueries(IOptions<MyOptions> options)
        {
            _settings = options.Value;
        }

        public TohHero addHero(TohHero hero)
        {
            var text = $"INSERT INTO Heroes (Name, Likes) VALUES (@Name, @Likes)";

            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@Name", System.Data.SqlDbType.VarChar) { Value = hero.Name },
                    new SqlParameter("@Likes", System.Data.SqlDbType.Int) { Value = hero.Likes }
                };
                cmd.Parameters.AddRange(param.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return hero;
                }
            }
        }

        public List<TohHero> getHeroes()
        {
            var text = "SELECT * FROM Heroes";

            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
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
                            HeroId = (int)reader[0],
                            Name = (string)reader[1],
                            Likes = (int)reader[2]
                        };
                        Heroes.Add(hero);
                    }
                    return Heroes;
                }
            }
        }

        public TohHero getHero(int id)
        {
            var text = "Select * FROM heroes WHERE HeroId = @HeroId";

            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@HeroId";
                param.Value = id;
                cmd.Parameters.Add(param);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var hero = new TohHero();
                    while (reader.Read())
                    {
                        hero.HeroId = (int)reader[0];
                        hero.Name = (string)reader[1];
                        hero.Likes = (int)reader[2];
                    }
                    return hero;
                }
            }
        }

        public TohHero updateHero(int id, TohHero updated)
        {
            var text = $"UPDATE Heroes SET Name = @Name WHERE HeroId = @HeroId";
            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {

                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@HeroId", System.Data.SqlDbType.Int) { Value = id },
                    new SqlParameter("@Name", System.Data.SqlDbType.VarChar) { Value = updated.Name }
                };
                cmd.Parameters.AddRange(param.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var hero = new TohHero
                    {
                        HeroId = id,
                        Name = updated.Name,
                        Likes = updated.Likes
                    };
                    return hero;
                }
            }
        }

        public void deleteHero(int id)
        {
            var text = $"DELETE FROM Heroes WHERE HeroId = @HeroId";
            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {

                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@HeroId";
                param.Value = id;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

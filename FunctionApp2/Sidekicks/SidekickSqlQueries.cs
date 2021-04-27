using FunctionApp2.Options;
using FunctionApp2.Models;
using FunctionApp2.QueryInterfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FunctionApp2.Sidekicks
{
    public class SidekickSqlQueries : ISidekickSqlQueries
    {
        private readonly MyOptions _settings;

        public SidekickSqlQueries(IOptions<MyOptions> options)
        {
            _settings = options.Value;
        }

        public TohSidekick addSidekick(TohSidekick sidekick)
        {
            var text = $"INSERT INTO Sidekicks (Name, Likes, HeroId) VALUES (@Name, @Likes, @HeroId)";

            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@Name", System.Data.SqlDbType.VarChar) { Value = sidekick.Name },
                    new SqlParameter("@Likes", System.Data.SqlDbType.Int) { Value = sidekick.Likes },
                    new SqlParameter("@HeroId", System.Data.SqlDbType.Int) { Value = sidekick.HeroId }
                };
                cmd.Parameters.AddRange(param.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return sidekick;
                }
            }
        }

        public List<TohSidekick> getSidekicks()
        {
            var text = "SELECT * FROM Sidekicks";

            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
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
                            SidekickId = (int)reader[0],
                            Name = (string)reader[1],
                            Likes = (int)reader[2],
                            HeroId = (int)reader[3]
                        };
                        Sidekicks.Add(sidekick);
                    }
                    return Sidekicks;
                }
            }
        }

        public List<TohSidekick> getHeroSidekicks(int id)
        {
            var text = "SELECT * FROM Sidekicks WHERE HeroId = @HeroId";

            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@HeroId";
                param.Value = id;
                cmd.Parameters.Add(param);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var Sidekicks = new List<TohSidekick>();
                    while (reader.Read())
                    {
                        var sidekick = new TohSidekick
                        {
                            SidekickId = (int)reader[0],
                            Name = (string)reader[1],
                            Likes = (int)reader[2],
                            HeroId = (int)reader[3]
                        };
                        Sidekicks.Add(sidekick);
                    }
                    return Sidekicks;
                }
            }
        }

        public TohSidekick getSidekick(int id)
        {
            var text = "Select * FROM Sidekicks WHERE SidekickId = @SidekickId";

            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {
                SqlCommand cmd = new SqlCommand(text, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@SidekickId";
                param.Value = id;
                cmd.Parameters.Add(param);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var sidekick = new TohSidekick();
                    while (reader.Read())
                    {
                        sidekick.SidekickId = (int)reader[0];
                        sidekick.Name = (string)reader[1];
                        sidekick.Likes = (int)reader[2];
                        sidekick.HeroId = (int)reader[3];
                    }
                    return sidekick;
                }
            }
        }

        public void deleteSidekick(int id)
        {
            var text = $"DELETE FROM Sidekicks WHERE SidekickId = @SidekickId";
            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {

                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@SidekickId";
                param.Value = id;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
        }

        public TohSidekick updateSidekick(int id, TohSidekick updated)
        {
            var text = $"UPDATE Sidekicks SET Name = @Name WHERE SidekickId = @SidekickId";
            using (SqlConnection conn = new SqlConnection(_settings.sqlConnection))
            {

                SqlCommand cmd = new SqlCommand(text, conn);
                conn.Open();
                List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter("@SidekickId", System.Data.SqlDbType.Int) { Value = id },
                    new SqlParameter("@Name", System.Data.SqlDbType.VarChar) { Value = updated.Name }
                };
                cmd.Parameters.AddRange(param.ToArray());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var sidekick = new TohSidekick
                    {
                        SidekickId = id,
                        Name = updated.Name,
                        Likes = updated.Likes,
                        HeroId = updated.HeroId
                    };
                    return sidekick;
                }
            }
        }
    }
}

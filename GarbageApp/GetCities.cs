using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using GarbageApp.Model;

namespace GarbageApp
{
    public static class GetCities
    {
        [FunctionName("GetCities")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getcities")] HttpRequest req,
            ILogger log)
        {
            List<City> cities = new List<City>();
            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "SELECT DISTINCT CityId, Name FROM City";
                        command.CommandText = sql;

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            City city = new City()
                            {
                                CityId = Guid.Parse(reader["CityId"].ToString()),
                                Name = reader["Name"].ToString()
                            };
                            cities.Add(city);
                        }
                    }
                }

                return new OkObjectResult(cities);
            }

            catch (Exception ex)
            {
                return new OkObjectResult(ex);
            }

        }
    }
}

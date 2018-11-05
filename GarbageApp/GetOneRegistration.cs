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
using GarbageApp.Model;
using System.Data.SqlClient;

namespace GarbageApp
{
    public static class GetOneRegistration
    {
        [FunctionName("GetOneRegistration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getoneregistration/{GarbageId}")] HttpRequest req, string GarbageId,
            ILogger log)
        {
            List<Registration> registrations = new List<Registration>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "SELECT * FROM Garbage WHERE GarbageId = @GarbageId";
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@GarbageId", GarbageId);

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Registration registration = new Registration()
                            {
                                GarbageId = Guid.Parse(reader["GarbageId"].ToString()),
                                Description = reader["Description"].ToString(),
                                CategoryId = Guid.Parse(reader["CategoryId"].ToString()),
                                Street = reader["Street"].ToString(),
                                CityId = Guid.Parse(reader["CityId"].ToString()),
                                Amount = int.Parse(reader["Amount"].ToString()),
                                Latitude = float.Parse(reader["Latitude"].ToString()),
                                Longitude = float.Parse(reader["Longitude"].ToString()),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Date = DateTime.Parse(reader["Date"].ToString())
                            };
                            registrations.Add(registration);
                        }
                    }
                }
                return new OkObjectResult(registrations);
            }

            catch (Exception ex)
            {
                return new OkObjectResult(ex);
            }
        }
    }
}

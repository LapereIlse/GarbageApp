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
    public static class GetCategories
    {
        [FunctionName("GetCategories")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getcategories")] HttpRequest req,
            ILogger log)
        {

            List<Category> categories = new List<Category>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = "SELECT DISTINCT CategoryId, Name FROM Category";
                        command.CommandText = sql;

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Category category = new Category()
                            {
                                CategoryId = Guid.Parse(reader["CategoryId"].ToString()),
                                Name = reader["Name"].ToString()
                            };
                            categories.Add(category);
                        }
                    }
                }
                return new OkObjectResult(categories);
            }

            catch (Exception ex)
            {
                return new OkObjectResult(ex);
            }
            
        }
    }
}

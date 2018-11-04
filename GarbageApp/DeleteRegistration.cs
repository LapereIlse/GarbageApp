using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace GarbageApp
{
    public static class DeleteRegistration
    {
        [FunctionName("DeleteRegistration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "Delete", Route = "deleteregistration/{GarbageId}")] HttpRequest req, string GarbageId,
            ILogger log)
        {
            try
            {
                // Connectie maken met de database
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        string sql = "DELETE FROM Garbage WHERE GarbageId = @GarbageId;";
                        command.Parameters.AddWithValue("@GarbageId", GarbageId);
                        command.CommandText = sql;

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new OkObjectResult("De gevraagde registration is verwijderd.");

            }

            catch (Exception ex)
            {
                return new OkObjectResult(ex);
            }
        }
    }
}

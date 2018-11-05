using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GarbageApp.Model;
using System.Data.SqlClient;

namespace GarbageApp
{
    public static class PutRegistration
    {
        [FunctionName("PutRegistration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "putregistration/{GarbageId}")] HttpRequest req, string GarbageId,
            ILogger log)
        {
            try
            {
                // Gegevens toekennen aan extra variabelen
                DateTime date = DateTime.Now;

                // Connectie maken met de body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                // JSON in body opvragen
                Registration registration = JsonConvert.DeserializeObject<Registration>(requestBody);

                // Extra variabelen toekennen aan JSON
                registration.Date = date;

                // Connectie maken met de database
                using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString")))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        string sql = "UPDATE Garbage SET Description = @Description, CategoryId = @CategoryId, Street = @Street, CityId = @CityId, Amount = @Amount, Latitude = @Latitude, Longitude = @Longitude, Name = @Name, Email = @Email, Date = @Date;";
                        command.CommandText = sql;

                        command.Parameters.AddWithValue("@GarbageId", registration.GarbageId);
                        command.Parameters.AddWithValue("@Description", registration.Description);
                        command.Parameters.AddWithValue("@CategoryId", registration.CategoryId);
                        command.Parameters.AddWithValue("@Street", registration.Street);
                        command.Parameters.AddWithValue("@CityId", registration.CityId);
                        command.Parameters.AddWithValue("@Amount", registration.Amount);
                        command.Parameters.AddWithValue("@Latitude", registration.Latitude);
                        command.Parameters.AddWithValue("@Longitude", registration.Longitude);
                        command.Parameters.AddWithValue("@Name", registration.Name);
                        command.Parameters.AddWithValue("@Email", registration.Email);
                        command.Parameters.AddWithValue("@Date", registration.Date);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new OkObjectResult(registration);
            }

            catch (Exception ex)
            {
                return new OkObjectResult(ex);
            }
        }
    }
}

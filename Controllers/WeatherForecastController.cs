using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace csharp_sqlinjection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(string password)
        {
            string jobName = "";
            SqlConnection connection = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=MyDb;Integrated Security=True");
            string queryString = $@"SELECT * FROM [dbo].[GET_RESULTS] WHERE JOB_NAME='" + password + "'";
                        SqlCommand command = new SqlCommand(queryString, connection);            
            
            
            var adpt = new SqlDataAdapter(command);
            var dt = new DataTable();
            adpt.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
               Response.Redirect("index.aspx");
            }
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

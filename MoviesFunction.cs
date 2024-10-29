using AspNetCoreFunction.RestApiClient;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace AspNetCoreFunction
{
    public class MoviesFunction
    {
        private readonly ILogger<MoviesFunction> _logger;

        public MoviesFunction(ILogger<MoviesFunction> logger)
        {
            _logger = logger;
        }

        [Function("MoviesFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            /*var client = new MoviesClient("https://app-test-webapi-westeurope-dev-001-e4grazbyh8gae3ff.westeurope-01.azurewebsites.net/", new HttpClient());
            var movies = await client.MoviesAllAsync();
            await client.MoviesPOSTAsync(new Movie())
            return new OkObjectResult(movies);*/

            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            const string queueName = "TEST";
            var message = DateTime.Now.ToString();

            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            channel.BasicPublish(exchange: String.Empty, routingKey: queueName, basicProperties: null, body: Encoding.UTF8.GetBytes(message));

            return new OkObjectResult($"Sent message {message} to queue {queueName}");
        }

        /*public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var client = new MoviesClient("https://app-test-webapi-westeurope-dev-001-e4grazbyh8gae3ff.westeurope-01.azurewebsites.net/", new HttpClient());
            var movies = await client.MoviesAllAsync();
            await client.MoviesPOSTAsync(new Movie())
            return new OkObjectResult(movies);
        }*/
    }
}

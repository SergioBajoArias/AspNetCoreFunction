using AspNetCoreFunction.RestApiClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

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
            var client = new MoviesClient("https://app-test-webapi-westeurope-dev-001-e4grazbyh8gae3ff.westeurope-01.azurewebsites.net/", new HttpClient());
            var movies = await client.MoviesAllAsync();
            return new OkObjectResult(movies);
        }
    }
}

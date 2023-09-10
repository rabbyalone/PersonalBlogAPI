using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Personal.Blog.Application.Services
{
    public class ApiCallerHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<ApiCallerHostedService> _logger;
        private Timer _timer;

        public ApiCallerHostedService(ILogger<ApiCallerHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ApiCallerHostedService is starting.");

            // Set up the timer to run the API call every 18 minutes
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(18));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Calling external API...");

            // Replace the URL with the actual API endpoint you want to call
            string apiUrl = "https://rabbyhasanblog.azurewebsites.net/api/posts/check";

            // Create an HttpClient instance to make the API request
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("API call successful.");
                }
                else
                {
                    _logger.LogError($"API call failed with status code {response.StatusCode}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ApiCallerHostedService is stopping.");

            // Stop the timer when the application is shutting down
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

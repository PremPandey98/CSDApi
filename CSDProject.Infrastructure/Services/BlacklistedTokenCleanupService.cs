using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSDProject.Infrastructure.Services
{
    public class BlacklistedTokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BlacklistedTokenCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
                    await authService.DeleteExpiredBlacklistedTokensAsync();
                }
                // Wait for 24 hours
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}

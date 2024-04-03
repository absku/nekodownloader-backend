using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NekoDownloader.Services.Schedules;

namespace NekoDownloader.Services.PeriodicHosted;

public class PeriodicHostedService(
    ILogger<PeriodicHostedService> logger,
    IServiceScopeFactory factory)
    : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                // Create scope, so we get request services
                await using AsyncServiceScope asyncScope = factory.CreateAsyncScope();

                // Get services from scope
                var syncComicSchedule =
                    asyncScope.ServiceProvider.GetRequiredService<SyncComicSchedule>();
                var syncChapterSchedule =
                    asyncScope.ServiceProvider.GetRequiredService<SyncChapterSchedule>();
                var syncPageSchedule =
                    asyncScope.ServiceProvider.GetRequiredService<SyncPageSchedule>();
                
                await syncComicSchedule.SyncComics();
                await syncChapterSchedule.SyncChapters();
                await syncPageSchedule.SyncPages();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                logger.LogInformation(
                    $"Failed to execute PeriodicHostedService with exception message {ex.Message}.");
            }
        }
    }
}
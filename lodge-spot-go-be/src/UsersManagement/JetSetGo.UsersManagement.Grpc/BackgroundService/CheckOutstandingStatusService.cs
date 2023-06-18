namespace JetSetGo.UsersManagement.Grpc.BackgroundService;

public class CheckOutstandingStatusService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ILogger<CheckOutstandingStatusService> _logger;

    public CheckOutstandingStatusService(ILogger<CheckOutstandingStatusService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Put your condition-checking logic here
            bool conditionMet = true;

            if (conditionMet)
            {
                _logger.LogInformation("Condition is met!");
            }
            else
            {
                _logger.LogInformation("Condition is not met!");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
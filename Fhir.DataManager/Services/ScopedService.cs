namespace Fhir.DataManager.Services;
public interface IScopedService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}

public class ScopedService : IScopedService
{
    private readonly ILogger<ScopedService> _logger;
    private readonly DateTime _startDateTime;
    public ScopedService(ILogger<ScopedService> logger)
    {
        _logger = logger;
        _startDateTime = DateTime.UtcNow;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        
        _logger.LogInformation($"{nameof(ScopedService)}  executed {DateTime.Now.ToString("T")}");
    }
}

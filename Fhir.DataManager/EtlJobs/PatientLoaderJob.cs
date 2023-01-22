using Fhir.DataManager.Services;
using Newtonsoft.Json;

namespace Fhir.DataManager.EtlJobs;

public class PatientLoaderJob : CronJobService
{
    private readonly ILogger<PatientLoaderJob> _logger;
    private readonly IFhirDataService _fhirDataService;
    private readonly IRepository _repository;

    public PatientLoaderJob(IScheduleConfig<PatientLoaderJob> configuration, ILogger<PatientLoaderJob> logger, IFhirDataService fhirDataService, IRepository repository) : base(configuration.CronExpression, configuration.TimeZoneInfo)
    {
        _logger = logger;
        _fhirDataService = fhirDataService;
        _repository = repository;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(PatientLoaderJob)} started {DateTime.UtcNow.ToShortDateString()}.");
        return base.StartAsync(cancellationToken);
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{DateTime.Now:hh:mm:ss} {nameof(PatientLoaderJob)} executing new data load process.");
        await LoadSourceDataAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(PatientLoaderJob)} is stopping.");
        return base.StopAsync(cancellationToken);
    }

    public async Task LoadSourceDataAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Data Loading Process");

        _logger.LogInformation($"---Loading data from source ---");

        var query = "select * from RestaurantMenus";

        var sourceData = await _repository.Get(query, DataSources.SqlDataSource.ToString());

        _logger.LogInformation($"---Transforming data from source---");

        await PrintOutput(sourceData);

        _logger.LogInformation($"---Loading transformed data  from source  into FHIR server---");

        await Task.CompletedTask;

    }

    private async Task PrintOutput(IEnumerable<dynamic> data)
    {
        foreach (var item in data)
        {
            var displaydata = new
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
            };

            Console.WriteLine(JsonConvert.SerializeObject(displaydata, Formatting.Indented));

        }
    }
}

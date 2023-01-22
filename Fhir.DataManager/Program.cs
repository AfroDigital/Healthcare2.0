using Fhir.DataManager.Services;
using Cronos;
using Fhir.DataManager.EtlJobs;

namespace Fhir.DataManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddScoped<IScopedService, ScopedService>();
                    services.AddSingleton<DataContext>();
                    services.AddSingleton<IFhirDataService, FhirDataService>();
                    services.AddSingleton<IRepository, Repository>();
                    services.AddCronJob<PatientLoaderJob>(c =>
                    {
                        c.TimeZoneInfo = TimeZoneInfo.Local;
                        c.CronExpression = @"*/1 * * * *";
                    });
                })
                .Build();

            host.Run();
        }
    }
}
using Fhir.ClinicalDecisionSupportService.Models;

namespace Fhir.ClinicalDecisionSupportService.Services;



public interface ICardService
{
    IList<Card> GetCards(string serviceName);
    IList<Service> GetCdsServices();
}

public class CardService : ICardService
{
    private readonly ILogger<CardService> _logger;
    private readonly IFhirDataService _fhirDataService;

    public CardService(ILogger<CardService> logger, IFhirDataService fhirDataService)
    {
        _logger = logger;
        _fhirDataService = fhirDataService;
    }


    public IList<Card> GetCards(string serviceName)
    {
        return GetMockCards();
    }

    private IList<Card> GetMockCards()
    {
        var source = new Link("Static CDS Service");

        return new List<Card> {

            Create("Info card", Indicator.Info, source),
            Create("Warning card", Indicator.Warning, source),
            Create("Hard stop card", Indicator.HardStop, source)

        };
    }


    private Card Create(string summary, Indicator indicator, Link source)
    {
        return new Card(summary, indicator, source);
    }

    public IList<Service> GetCdsServices()
    {
        Service service = new Service("3B06986C-AA6B-4E00-8374-DFA5F58A8362", "patient-view", "Static CDS Service in CSharp","An example static CDS service in CSharp");

        service.Prefetch.Add("patient", "Patient/{{Patient.id}}");

        return new List<Service> {
                service
        };
    }
}

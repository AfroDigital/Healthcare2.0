using Fhir.ClinicalDecisionSupportService.Models;
using Fhir.ClinicalDecisionSupportService.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Fhir.ClinicalDecisionSupportService.Controllers
{
    [ApiController]
    [EnableCors("WithCredentialsAnyOrigin")]
    public class ServiceController : ControllerBase
    {
   

        private readonly ILogger<ServiceController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICardService _cardService;



        public ServiceController(ILogger<ServiceController> logger, IConfiguration configuration, ICardService cardService)
        {
            _logger = logger;
            _configuration = configuration; 
            _cardService = cardService;
        }

        [HttpGet()]
        [Route("")]
        [ApiExplorerSettings(IgnoreApi =true)]
        public async Task<IActionResult> DisplayServiceProperties()
        {
            await Task.CompletedTask;
            return Ok(new { ServiceName = _configuration["Application:ServiceName"], Version = _configuration["Application:Version"], LastCheck = DateTime.Now.ToShortDateString() });
        }


        [HttpGet()]
        [Route("/cds-services")]
        public IActionResult Discovery()
        {
            var services = new Dictionary<string, IList<Service>>
            {
                { "services", _cardService.GetCdsServices().ToList() }
            };
            return Ok(services);
        }

        [HttpPost("/cds-services/{id}")]
        public IActionResult Static(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            
            var cards = new Dictionary<string, IList<Card>>
            {
                { "cards", _cardService.GetCards(id) }
            };

            return Ok(cards);
        }
    }
}
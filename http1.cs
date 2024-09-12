using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CertificateFunc
{
    public class http1
    {
        private readonly ILogger<http1> _logger;
        private readonly CertService _certService;

        public http1(ILogger<http1> logger, CertService certService)
        {
            _logger = logger;
            _certService = certService;
        }

        [Function("http1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            _logger.LogInformation(_certService.ShowCertProps());

            return new OkObjectResult(_certService.ShowCertProps());
        }
    }
}

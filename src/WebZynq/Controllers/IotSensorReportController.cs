using Microsoft.AspNetCore.Mvc;


namespace WebZynq.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IotSensorReportController : ControllerBase
    {
        private readonly ILogger<IotSensorReportController> _logger;

        public IotSensorReportController(ILogger<IotSensorReportController> logger)
        {
            _logger = logger;
        }

        [HttpPost("{id}")]
        public async Task NewReport(Guid id)
        {
            return;
        }
    }
}

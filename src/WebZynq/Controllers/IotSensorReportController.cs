using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using WebZynq.Database.Models;

namespace WebZynq.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IotSensorReportController : ControllerBase
    {
        private readonly ILogger<IotSensorReportController> _logger;
        private readonly DbContext? _dbContext;

        public IotSensorReportController(ILogger<IotSensorReportController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _dbContext = provider.GetService<DbContext>();
        }

        [HttpPost]
        public async Task<IActionResult> PostReport(IotSensorReport report, HttpContext context)
        {
            try{
                await _dbContext!.AddAsync(report);
                return this.Created("/", report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Creation of sensor report failed: {report}", report);
                return this.UnprocessableEntity();
            }
        }
    }
}

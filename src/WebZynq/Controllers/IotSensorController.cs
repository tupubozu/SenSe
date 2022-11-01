using Microsoft.AspNetCore.Mvc;
using WebZynq.Database.Models;

namespace WebZynq.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IotSensorController : ControllerBase
    {
        private readonly ILogger<IotSensorController> _logger;

        public IotSensorController(ILogger<IotSensorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("NewSensorId")]
        public IotSensor NewSensor()
        {
            return new IotSensor() { Id = Guid.NewGuid() };
        }
    }
}

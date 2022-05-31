using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebZynq.Database.Contexts
{
    public class IotDataContext : DbContext
    {
        public DbSet<IotSensor> Sensors { get; set; }
        public DbSet<IotSensorReport> Reports { get; set; }
    }
}

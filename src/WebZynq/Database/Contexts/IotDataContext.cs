﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebZynq.Database.Models;

namespace WebZynq.Database.Contexts
{
    public class IotDataContext : DbContext
    {
        public DbSet<IotSensor> Sensors { get; set; }
        public DbSet<IotSensorReport> Reports { get; set; }
    }
}
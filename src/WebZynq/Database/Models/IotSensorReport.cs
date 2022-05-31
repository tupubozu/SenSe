namespace WebZynq.Database.Models
{
    public class IotSensorReport
    {
        public IotSensor Sensor { get; init; }
        public DateTime TimeStamp { get; init; }
        public double Value { get; init; }
    }
}

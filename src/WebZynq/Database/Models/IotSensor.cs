namespace WebZynq
{
    public class IotSensor
    {
        public Guid Id { get; init; }
        public List<IotSensorReport> Reports { get; init; }
    }
}

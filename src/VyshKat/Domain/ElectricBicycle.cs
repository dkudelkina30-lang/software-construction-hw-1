namespace VyshKat.Domain;

public class ElectricBicycle : Vehicle, IEnergyConsumer
{
    public ElectricBicycle(string name, int simplicity, int condition, double dailyEnergyKwh)
        : base(name, simplicity, condition)
    {
        DailyEnergyKwh = dailyEnergyKwh;
    }

    public double DailyEnergyKwh { get; }
    public override string Kind => "Электровелосипед";
}

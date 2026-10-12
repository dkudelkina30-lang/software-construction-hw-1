namespace VyshKat.Domain;

public class ChargingCabinet : Thing, IEnergyConsumer
{
    public ChargingCabinet(string name, double dailyEnergyKwh) : base(name)
    {
        DailyEnergyKwh = dailyEnergyKwh;
    }

    public double DailyEnergyKwh { get; }
    public override string Kind => "Зарядный шкаф";
}

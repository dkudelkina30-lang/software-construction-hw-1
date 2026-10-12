namespace VyshKat.Domain;

public class DockStation : Thing, IEnergyConsumer
{
    public DockStation(string name, double dailyEnergyKwh) : base(name)
    {
        DailyEnergyKwh = dailyEnergyKwh;
    }

    public double DailyEnergyKwh { get; }
    public override string Kind => "Док-станция";
}

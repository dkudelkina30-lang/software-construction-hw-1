namespace VyshKat.Domain;

public class Scooter : Vehicle, IEnergyConsumer
{
    public Scooter(string name, int simplicity, int condition, double dailyEnergyKwh)
        : base(name, simplicity, condition)
    {
        DailyEnergyKwh = dailyEnergyKwh;
    }

    public double DailyEnergyKwh { get; }
    public override string Kind => "Самокат";
}

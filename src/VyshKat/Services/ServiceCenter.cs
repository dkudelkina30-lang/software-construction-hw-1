using VyshKat.Domain;

namespace VyshKat.Services;

// Штатный техосмотр: состояние не ниже 60%, расход электротранспорта не больше 3 кВт·ч
public class ServiceCenter : IServiceCenter
{
    private const int MinCondition = 60;
    private const double MaxDailyEnergy = 3.0;

    public InspectionResult Inspect(Vehicle vehicle)
    {
        if (vehicle.Condition < MinCondition)
        {
            return new InspectionResult(false, $"сильный износ ({vehicle.Condition}%, нужно от {MinCondition}%)");
        }

        if (vehicle is IEnergyConsumer consumer && consumer.DailyEnergyKwh > MaxDailyEnergy)
        {
            return new InspectionResult(false, $"слишком большой расход энергии (больше {MaxDailyEnergy} кВт·ч)");
        }

        return new InspectionResult(true, "техосмотр пройден");
    }
}

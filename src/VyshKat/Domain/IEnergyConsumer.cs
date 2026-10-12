namespace VyshKat.Domain;

// Устройство, которое потребляет электроэнергию
public interface IEnergyConsumer
{
    double DailyEnergyKwh { get; } // кВт·ч в сутки
}

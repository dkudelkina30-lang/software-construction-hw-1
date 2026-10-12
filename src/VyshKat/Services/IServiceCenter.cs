using VyshKat.Domain;

namespace VyshKat.Services;

// Техосмотр: решает, можно ли принять транспорт в парк
public interface IServiceCenter
{
    InspectionResult Inspect(Vehicle vehicle);
}

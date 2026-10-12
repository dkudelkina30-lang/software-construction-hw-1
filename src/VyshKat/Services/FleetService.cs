using VyshKat.Domain;

namespace VyshKat.Services;

// Учёт парка: приём транспорта, вещи на балансе и отчёты
public class FleetService
{
    private const int BeginnerMinSimplicity = 6;

    private readonly IServiceCenter _serviceCenter;
    private readonly List<Vehicle> _vehicles = new();
    private readonly List<Thing> _things = new();
    private int _lastInventoryNumber;

    public FleetService(IServiceCenter serviceCenter)
    {
        _serviceCenter = serviceCenter;
    }

    public IReadOnlyList<Vehicle> Vehicles => _vehicles;
    public IReadOnlyList<Thing> Things => _things;

    // Транспорт попадает в парк и получает номер только после успешного техосмотра
    public InspectionResult AcceptVehicle(Vehicle vehicle)
    {
        InspectionResult result = _serviceCenter.Inspect(vehicle);

        if (result.Accepted)
        {
            vehicle.InventoryNumber = ++_lastInventoryNumber;
            _vehicles.Add(vehicle);
        }

        return result;
    }

    // Вещи ставятся на баланс без техосмотра
    public void RegisterThing(Thing thing)
    {
        thing.InventoryNumber = ++_lastInventoryNumber;
        _things.Add(thing);
    }

    // Суммарный суточный расход всех потребителей энергии: и транспорта, и вещей
    public double GetTotalDailyEnergy()
    {
        return GetAllItems().OfType<IEnergyConsumer>().Sum(c => c.DailyEnergyKwh);
    }

    public List<Vehicle> GetBeginnerVehicles()
    {
        return _vehicles.Where(v => v.Simplicity >= BeginnerMinSimplicity).ToList();
    }

    // Вся техника и вещи по возрастанию инвентарного номера
    public List<IInventoryItem> GetAllItems()
    {
        return _vehicles.Cast<IInventoryItem>()
            .Concat(_things)
            .OrderBy(item => item.InventoryNumber)
            .ToList();
    }
}

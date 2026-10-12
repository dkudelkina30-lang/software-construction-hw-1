using System.Globalization;
using VyshKat.Domain;
using VyshKat.Services;

namespace VyshKat.App;

// Консольное меню учёта парка
public class ConsoleApp
{
    private readonly FleetService _fleet;
    private readonly ConsoleIO _io;

    public ConsoleApp(FleetService fleet, ConsoleIO io)
    {
        _fleet = fleet;
        _io = io;
    }

    public void Run()
    {
        _io.Print("=== ВышКат: учёт парка ===");
        try
        {
            while (true)
            {
                _io.Print();
                _io.Print("1. Принять транспорт (с техосмотром)");
                _io.Print("2. Поставить вещь на баланс");
                _io.Print("3. Отчёт: количество и энергия");
                _io.Print("4. Транспорт для новичков");
                _io.Print("5. Инвентарная ведомость");
                _io.Print("6. Демонстрационный сценарий");
                _io.Print("0. Выход");

                int choice = _io.ReadInt("Выберите пункт: ", 0, 6);

                if (choice == 0)
                {
                    break;
                }

                switch (choice)
                {
                    case 1: AddVehicle(); break;
                    case 2: AddThing(); break;
                    case 3: PrintReport(); break;
                    case 4: PrintBeginnerVehicles(); break;
                    case 5: PrintInventory(); break;
                    case 6: RunDemo(); break;
                }
            }
        }

        catch (EndOfInputException)
        {
            _io.Print();
        }
        _io.Print("До свидания!");
    }

    private void AddVehicle()
    {
        int kind = _io.ReadInt("Вид (1 - самокат, 2 - велосипед, 3 - электровелосипед): ", 1, 3);
        string name = _io.ReadText("Модель: ");
        int simplicity = _io.ReadInt("Простота для новичка (1-10): ", 1, 10);
        int condition = _io.ReadInt("Техническое состояние, % (0-100): ", 0, 100);

        // Новый вид транспорта добавляется сюда отдельной веткой
        Vehicle vehicle = kind switch
        {
            1 => new Scooter(name, simplicity, condition, _io.ReadEnergy("Расход энергии, кВт·ч/сутки: ")),
            2 => new Bicycle(name, simplicity, condition),
            _ => new ElectricBicycle(name, simplicity, condition, _io.ReadEnergy("Расход энергии, кВт·ч/сутки: ")),
        };

        Accept(vehicle);
    }

    private void AddThing()
    {
        int kind = _io.ReadInt("Вид (1 - шлем, 2 - док-станция, 3 - зарядный шкаф): ", 1, 3);
        string name = _io.ReadText("Наименование: ");

        Thing thing = kind switch
        {
            1 => new Helmet(name),
            2 => new DockStation(name, _io.ReadEnergy("Расход энергии, кВт·ч/сутки: ")),
            _ => new ChargingCabinet(name, _io.ReadEnergy("Расход энергии, кВт·ч/сутки: ")),
        };

        Register(thing);
    }

    private void Accept(Vehicle vehicle)
    {
        InspectionResult result = _fleet.AcceptVehicle(vehicle);

        if (result.Accepted)
        {
            _io.Print($"[ПРИНЯТ] {Describe(vehicle)}");
        }
        else
        {
            _io.Print($"[ОТКЛОНЁН] {vehicle.Kind} «{vehicle.Name}»: {result.Reason}");
        }
    }

    private void Register(Thing thing)
    {
        _fleet.RegisterThing(thing);
        _io.Print($"[НА БАЛАНСЕ] {Describe(thing)}");
    }

    private void PrintReport()
    {
        _io.Print("--- Отчёт ---");
        _io.Print($"Транспорт в парке: {_fleet.Vehicles.Count}");
        _io.Print($"Вещей на балансе: {_fleet.Things.Count}");
        _io.Print($"Всего единиц: {_fleet.Vehicles.Count + _fleet.Things.Count}");
        _io.Print($"Суммарный расход энергии: {Format(_fleet.GetTotalDailyEnergy())} кВт·ч/сутки");
    }

    private void PrintBeginnerVehicles()
    {
        _io.Print("--- Для новичков (простота от 6) ---");
        List<Vehicle> vehicles = _fleet.GetBeginnerVehicles();

        if (vehicles.Count == 0)
        {
            _io.Print("нет подходящего транспорта");
        }

        foreach (Vehicle vehicle in vehicles)
            _io.Print($"{Describe(vehicle)}, простота {vehicle.Simplicity}");
    }

    private void PrintInventory()
    {
        _io.Print("--- Инвентарная ведомость ---");
        List<IInventoryItem> items = _fleet.GetAllItems();

        if (items.Count == 0)
        {
            _io.Print("на балансе ничего нет");
        }

        foreach (IInventoryItem item in items)
            _io.Print(Describe(item));
    }

    // Все сценарии подряд: приём, два отказа техосмотра, вещи и все отчёты
    public void RunDemo()
    {
        _io.Print(">>> Демонстрация");
        Accept(new Scooter("Ninebot Max G30", 8, 95, 0.6));
        Accept(new ElectricBicycle("Eltreco XT 600", 5, 85, 1.2));
        Accept(new Bicycle("Stels Navigator 300", 9, 70));
        Accept(new Scooter("Kugoo M4 Pro", 4, 40, 0.9));
        Accept(new ElectricBicycle("Прототип с хакатона ФКН", 7, 90, 4.5));
        Register(new Helmet("Шлем, размер M"));
        Register(new DockStation("Док-станция на Покровке", 2.4));
        Register(new ChargingCabinet("Зарядный шкаф у корпуса R", 5.0));
        PrintReport();
        PrintBeginnerVehicles();
        PrintInventory();
        _io.Print("<<< Конец демонстрации");
    }

    private static string Describe(IInventoryItem item)
    {
        string text = $"VK-{item.InventoryNumber:D4} {item.Kind} «{item.Name}»";

        if (item is IEnergyConsumer consumer)
        {
            text += $", {Format(consumer.DailyEnergyKwh)} кВт·ч/сутки";
        }
        
        return text;
    }

    private static string Format(double value)
    {
        return value.ToString("0.00", CultureInfo.InvariantCulture);
    }
}

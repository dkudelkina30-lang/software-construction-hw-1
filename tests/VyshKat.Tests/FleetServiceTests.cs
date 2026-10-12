using Microsoft.Extensions.DependencyInjection;
using VyshKat.App;
using VyshKat.Domain;
using VyshKat.Services;
using Xunit;

namespace VyshKat.Tests;

public class FleetServiceTests
{
    // Собираем сервис тем же DI, что и в программе, но подменяем техосмотр на fake:
    private static FleetService CreateService(IServiceCenter serviceCenter)
    {
        var io = new ConsoleIO(new StringReader(""), TextWriter.Null);

        ServiceProvider provider = new ServiceCollection()
            .AddVyshKat(io)
            .AddSingleton<IServiceCenter>(serviceCenter)
            .BuildServiceProvider();

        return provider.GetRequiredService<FleetService>();
    }

    [Fact]
    public void RejectedVehicleIsNotAdded()
    {
        var fake = new FakeServiceCenter(accept: false);
        FleetService fleet = CreateService(fake);
        var scooter = new Scooter("Ninebot", 8, 95, 0.6);

        fleet.AcceptVehicle(scooter);

        Assert.Equal(1, fake.Calls);
        Assert.Empty(fleet.Vehicles);
        Assert.Equal(0, scooter.InventoryNumber);
    }

    [Fact]
    public void FleetTrustsServiceCenterDecision()
    {
        // Штатный техосмотр отклонил бы изношенный самокат, но fake его принимает
        FleetService fleet = CreateService(new FakeServiceCenter(accept: true));

        fleet.AcceptVehicle(new Scooter("Изношенный", 3, 5, 0.9));

        Assert.Single(fleet.Vehicles);
        Assert.Equal(1, fleet.Vehicles[0].InventoryNumber);
    }

    [Fact]
    public void ThingIsRegisteredWithoutInspection()
    {
        var fake = new FakeServiceCenter(accept: false);
        FleetService fleet = CreateService(fake);

        fleet.RegisterThing(new Helmet("Шлем"));

        Assert.Equal(0, fake.Calls);
        Assert.Single(fleet.Things);
    }

    [Fact]
    public void TotalEnergyCountsVehiclesAndThings()
    {
        FleetService fleet = CreateService(new FakeServiceCenter(accept: true));
        fleet.AcceptVehicle(new Scooter("Ninebot", 8, 95, 0.6));
        fleet.AcceptVehicle(new Bicycle("Stels", 9, 70));
        fleet.RegisterThing(new Helmet("Шлем"));
        fleet.RegisterThing(new DockStation("Док", 2.4));

        Assert.Equal(3.0, fleet.GetTotalDailyEnergy(), 6);
    }

    [Theory]
    [InlineData(5, false)]
    [InlineData(6, true)]
    [InlineData(10, true)]
    
    public void BeginnerRuleIsSimplicityAtLeastSix(int simplicity, bool expected)
    {
        FleetService fleet = CreateService(new FakeServiceCenter(accept: true));
        fleet.AcceptVehicle(new Bicycle("Stels", simplicity, 90));

        Assert.Equal(expected, fleet.GetBeginnerVehicles().Count == 1);
    }

    [Fact]
    public void InventoryNumbersAreSharedByVehiclesAndThings()
    {
        FleetService fleet = CreateService(new FakeServiceCenter(accept: true));
        fleet.AcceptVehicle(new Bicycle("Stels", 9, 70));
        fleet.RegisterThing(new Helmet("Шлем"));
        fleet.AcceptVehicle(new Scooter("Ninebot", 8, 95, 0.6));

        List<IInventoryItem> items = fleet.GetAllItems();

        Assert.Equal(new[] { 1, 2, 3 }, items.Select(i => i.InventoryNumber));
        Assert.Equal("Шлем", items[1].Kind);
    }

    [Fact]
    public void VehicleWithWrongSimplicityCannotBeCreated()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bicycle("Stels", 11, 70));
    }
}

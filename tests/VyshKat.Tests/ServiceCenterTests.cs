using VyshKat.Domain;
using VyshKat.Services;
using Xunit;

namespace VyshKat.Tests;

public class ServiceCenterTests
{
    private readonly ServiceCenter _serviceCenter = new();

    [Fact]
    public void AcceptsVehicleInGoodCondition()
    {
        Assert.True(_serviceCenter.Inspect(new Scooter("Ninebot", 8, 95, 0.6)).Accepted);
    }

    [Fact]
    public void AcceptsConditionExactlySixty()
    {
        Assert.True(_serviceCenter.Inspect(new Bicycle("Stels", 9, 60)).Accepted);
    }

    [Fact]
    public void RejectsWornVehicle()
    {
        Assert.False(_serviceCenter.Inspect(new Bicycle("Stels", 9, 59)).Accepted);
    }

    [Fact]
    public void RejectsTooHighEnergyConsumption()
    {
        Assert.False(_serviceCenter.Inspect(new ElectricBicycle("Прототип", 7, 90, 4.5)).Accepted);
    }
}

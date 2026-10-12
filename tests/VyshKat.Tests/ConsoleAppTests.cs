using Microsoft.Extensions.DependencyInjection;
using VyshKat.App;
using Xunit;

namespace VyshKat.Tests;

public class ConsoleAppTests
{
    private static string Run(params string[] lines)
    {
        var output = new StringWriter();
        var io = new ConsoleIO(new StringReader(string.Join("\n", lines)), output);
        ServiceProvider provider = new ServiceCollection().AddVyshKat(io).BuildServiceProvider();

        provider.GetRequiredService<ConsoleApp>().Run();
        return output.ToString();
    }

    [Fact]
    public void AcceptsVehicleFromInputAndShowsReport()
    {
        string output = Run("1", "1", "Ninebot", "8", "95", "0,6", "3", "0");

        Assert.Contains("[ПРИНЯТ] VK-0001 Самокат «Ninebot»", output);
        Assert.Contains("Суммарный расход энергии: 0.60", output);
    }

    [Fact]
    public void WrongInputIsAskedAgain()
    {
        string output = Run("abc", "1", "2", "", "Stels", "15", "9", "30", "0");

        Assert.Contains("Введите целое число от 0 до 6", output);
        Assert.Contains("Строка не должна быть пустой", output);
        Assert.Contains("[ОТКЛОНЁН] Велосипед «Stels»", output);
    }

    [Fact]
    public void DemoShowsAllScenarios()
    {
        string output = Run("6", "0");

        Assert.Contains("[ОТКЛОНЁН] Самокат «Kugoo M4 Pro»", output);
        Assert.Contains("Всего единиц: 6", output);
        Assert.Contains("Суммарный расход энергии: 9.20", output);
    }

    [Fact]
    public void EndOfInputStopsProgram()
    {
        string output = Run("1", "1");

        Assert.Contains("До свидания!", output);
    }
}

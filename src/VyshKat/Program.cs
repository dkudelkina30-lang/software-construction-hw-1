using System.Text;
using Microsoft.Extensions.DependencyInjection;
using VyshKat.App;

Console.OutputEncoding = Encoding.UTF8;

string inputFile = args.Length > 0 ? args[0] : "scenario.txt";

if (!File.Exists(inputFile))
{
    Console.WriteLine($"Файл не найден: {inputFile}");
    return;
}

using var input = new StreamReader(inputFile, Encoding.UTF8);
var io = new ConsoleIO(input, Console.Out);

using ServiceProvider provider = new ServiceCollection()
    .AddVyshKat(io)
    .BuildServiceProvider();

provider.GetRequiredService<ConsoleApp>().Run();

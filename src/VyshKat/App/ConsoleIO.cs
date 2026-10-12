using System.Globalization;

namespace VyshKat.App;

// Входной файл закончился
public class EndOfInputException : Exception
{
}

// Чтение значений из входного файла с проверкой.
// Каждая строка файла - ответ на очередной вопрос программы.
// Прочитанное значение печатается после вопроса, чтобы в выводе было видно, что ввели.
// При ошибке выводится подсказка и читается следующая строка.
public class ConsoleIO
{
    private readonly TextReader _input;
    private readonly TextWriter _output;

    public ConsoleIO(TextReader input, TextWriter output)
    {
        _input = input;
        _output = output;
    }

    public void Print(string text = "")
    {
        _output.WriteLine(text);
    }

    public string ReadLine(string prompt)
    {
        _output.Write(prompt);
        string? line = _input.ReadLine();

        if (line == null)
        {
            throw new EndOfInputException();
        }

        _output.WriteLine(line);
        return line.Trim();
    }

    public string ReadText(string prompt)
    {
        while (true)
        {
            string text = ReadLine(prompt);

            if (text.Length > 0)
            {
                return text;
            }

            Print("  Строка не должна быть пустой.");
        }
    }

    public int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            if (int.TryParse(ReadLine(prompt), out int value) && value >= min && value <= max)
            {
                return value;
            }

            Print($"  Введите целое число от {min} до {max}.");
        }
    }

    public double ReadEnergy(string prompt)
    {
        while (true)
        {
            string text = ReadLine(prompt).Replace(',', '.');

            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value) && value >= 0)
            {
                return value;
            }
            
            Print("  Введите неотрицательное число, например 0.8");
        }
    }
}

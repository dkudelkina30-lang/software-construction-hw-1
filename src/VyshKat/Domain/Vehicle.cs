namespace VyshKat.Domain;

// Базовый класс транспорта. Новый вид транспорта = новый наследник
public abstract class Vehicle : IInventoryItem
{
    protected Vehicle(string name, int simplicity, int condition)
    {
        if (simplicity < 1 || simplicity > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(simplicity), "Простота должна быть от 1 до 10");
        }

        if (condition < 0 || condition > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(condition), "Состояние должно быть от 0 до 100%");
        }

        Name = name;
        Simplicity = simplicity;
        Condition = condition;
    }

    public string Name { get; }
    public int InventoryNumber { get; set; }

    public int Simplicity { get; } // простота для новичка, 1-10
    public int Condition { get; }  // техническое состояние, %

    public abstract string Kind { get; }
}

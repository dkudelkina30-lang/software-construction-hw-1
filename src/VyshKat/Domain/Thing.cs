namespace VyshKat.Domain;

// Вещь на балансе (не транспорт): ставится на учёт без техосмотра
public abstract class Thing : IInventoryItem
{
    protected Thing(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public int InventoryNumber { get; set; }

    public abstract string Kind { get; }
}

namespace VyshKat.Domain;

// Все, что стоит на балансе и имеет инвентарный номер (транспорт и вещи).
public interface IInventoryItem
{
    string Name { get; }
    string Kind { get; }
    int InventoryNumber { get; set; }
}

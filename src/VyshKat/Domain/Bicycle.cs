namespace VyshKat.Domain;

// Обычный велосипед энергию не потребляет
public class Bicycle : Vehicle
{
    public Bicycle(string name, int simplicity, int condition)
        : base(name, simplicity, condition)
    {
    }

    public override string Kind => "Велосипед";
}

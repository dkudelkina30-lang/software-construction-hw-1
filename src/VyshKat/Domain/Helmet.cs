namespace VyshKat.Domain;

public class Helmet : Thing
{
    public Helmet(string name) : base(name)
    {
    }

    public override string Kind => "Шлем";
}

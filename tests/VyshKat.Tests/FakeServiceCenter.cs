using VyshKat.Domain;
using VyshKat.Services;

namespace VyshKat.Tests;

// Fake-техосмотр: всегда даёт заданный ответ и запоминает, сколько раз его вызвали
public class FakeServiceCenter : IServiceCenter
{
    private readonly bool _accept;

    public FakeServiceCenter(bool accept)
    {
        _accept = accept;
    }

    public int Calls { get; private set; }

    public InspectionResult Inspect(Vehicle vehicle)
    {
        Calls++;
        return new InspectionResult(_accept, _accept ? "ok" : "отказ");
    }
}

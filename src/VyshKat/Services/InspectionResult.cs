namespace VyshKat.Services;

// Решение техосмотра: принять или нет и почему
public record InspectionResult(bool Accepted, string Reason);

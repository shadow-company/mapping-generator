using MappingGenerator;

namespace Test;

public record Class1
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}

[GenerateMapperFrom(typeof(Class1))]
public partial record Class2
{
    public Guid ID { get; set; } = Guid.NewGuid();
    [MapFromProperty(nameof(Class1.Name))] public string Description { get; set; } = string.Empty;
}

public class Program
{
    public void Main()
    {
        Class1 a = new();
        Class2 b = Class2.MapFrom(a);
    }
}

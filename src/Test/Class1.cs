using MappingGenerator;

namespace Test;

public record Class1
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; } = 1;
}

[GenerateMapperFrom(typeof(Class1))]
public partial record Class2
{
    public Guid ID { get; set; } = Guid.NewGuid();
    [MapRawValue("source.ID.ToString()")] public required string UserID { get; set; }
    [MapFromProperty(nameof(Class1.Name))] public string Username { get; set; } = string.Empty;
    [MapConstantValue(5)] public required int Number { get; set; }
    [MapDefaultValue] public required int Number2 { get; set; } = 0;
}

public class Program
{
    public void Main()
    {
        Class1 a = new();
        Class2 b = Class2.MapFrom(a);

        List<Class1> aList = [new(), new()];
        List<Class2> bList = Class2.MapFrom(aList);

        Class1[] aArray = [new(), new()];
        Class2[] bArray = Class2.MapFrom(aArray);

        IEnumerable<Class2> bEnumerable = Class2.MapFrom(aList.Where(a => a.ID != Guid.Empty));

        Dictionary<Guid, Class1> aDictionary = [];
        aDictionary.Add(aList[0].ID, aList[0]);
        aDictionary.Add(aList[1].ID, aList[1]);

        Dictionary<Guid, Class2> bDictionary = Class2.MapFrom(aDictionary);
    }
}

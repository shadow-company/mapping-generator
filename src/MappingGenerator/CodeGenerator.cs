using Microsoft.CodeAnalysis;

namespace MappingGenerator;

[Generator]
public class CodeGenerator : IIncrementalGenerator
{
    internal const string Namespace = "MappingGenerator";
    internal const string InterfaceName = "IMappable";
    internal const string MapperAttributeName = "GenerateMapperFrom";
    internal const string PropertyAttributeName = "MapFromProperty";
    internal const string ValueProviderAttributeName = "MapConstantValue";
    internal const string RawValueProviderAttributeName = "MapRawValue";

    internal static readonly DiagnosticDescriptor UnmappedRequiredPropertyWarning = new(
        id: "MAPGEN001",
        title: "Unmapped required property",
        messageFormat: "Unresolved reference in source. This property will be mapped to 'default'. Use '[{0}]' to manually map to a different field, '[{1}]' to provide a constant value or '[{2}]' to insert C# code.",
        category: "Unresolved Reference",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    internal static readonly DiagnosticDescriptor PropertyTypeMismatch = new(
        id: "MAPGEN002",
        title: "Unmapped required property",
        messageFormat: "Type mismatch between '{0} {1}.{2}' and '{3} {4}.{5}'. This property will be mapped to 'default' instead.",
        category: "Type Mismatch",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public void Initialize(IncrementalGeneratorInitializationContext incrementalGeneratorInitializationContext)
    {
        AddInterfaceSourceCode.Run(incrementalGeneratorInitializationContext);
        AddAttributesSourceCode.Run(incrementalGeneratorInitializationContext);
        IncrementalValuesProvider<List<ClassModel>> incrementalValuesProvider = CheckForGeneratorAttributeUsages.Run(incrementalGeneratorInitializationContext);
        incrementalGeneratorInitializationContext.RegisterSourceOutput(incrementalValuesProvider, GeneratePartialClassSourceCode.Run);
    }
}

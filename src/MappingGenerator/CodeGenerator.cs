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
    internal const string DefaultValueProviderAttributeName = "MapDefaultValue";
    internal const string RawValueProviderAttributeName = "MapRawValue";

    internal static readonly DiagnosticDescriptor UnmappedProperty = new(
        id: "MAPGEN001",
        title: "Unresolved Reference",
        messageFormat: "Unresolved reference in source. Use '[{0}]' to manually map to a different field, '[{1}]' to provide a constant value or '[{2}]' to insert C# code.",
        category: "Unresolved Reference",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    internal static readonly DiagnosticDescriptor UnmappedRequiredProperty = new(
        id: "MAPGEN002",
        title: "Unresolved Reference",
        messageFormat: "Unresolved required reference in source. Use '[{0}]' to manually map to a different field, '[{1}]' to provide a constant value or '[{2}]' to insert C# code.",
        category: "Unresolved Reference",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    internal static readonly DiagnosticDescriptor TypeMismatch = new(
        id: "MAPGEN003",
        title: "Type Mismatch",
        messageFormat: "Type mismatch between '{0} {1}.{2}' and '{3} {4}.{5}'",
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

using Microsoft.CodeAnalysis;

namespace MappingGenerator;

internal record ClassModel
{
    public readonly ClassDeclarationTypes ClassDeclarationType;
    public readonly string SourceNamespaceString;
    public readonly string SourceName;
    public readonly string TargetNamespaceString;
    public readonly string TargetName;
    public List<IPropertySymbol> SourceProperties;
    public List<IPropertySymbol> TargetProperties;

    public ClassModel(ClassDeclarationTypes classDeclarationType, string sourceNamespaceString, string sourceName, string targetNamespaceString, string targetName)
    {
        ClassDeclarationType = classDeclarationType;
        SourceNamespaceString = sourceNamespaceString;
        SourceName = sourceName;
        TargetNamespaceString = targetNamespaceString;
        TargetName = targetName;
        SourceProperties = [];
        TargetProperties = [];
    }
}

using Microsoft.CodeAnalysis;

namespace MappingGenerator;

internal static class GetSerializableProperties
{
    internal static void Run(INamedTypeSymbol sourceTypeSymbol, INamedTypeSymbol targetTypeSymbol, ref ClassModel classModel)
    {
        List<IPropertySymbol> sourcePropertySymbols = [];
        List<IPropertySymbol> targetPropertySymbols = [];
        INamedTypeSymbol? currentSourceSymbol = sourceTypeSymbol;
        INamedTypeSymbol? currentTargetSymbol = targetTypeSymbol;

        while (currentTargetSymbol is { SpecialType: not SpecialType.System_Object })
        {
            List<IPropertySymbol> currentTargetProperties = [.. currentTargetSymbol.GetMembers().OfType<IPropertySymbol>().Where(propertySymbol => propertySymbol is {
                DeclaredAccessibility: Accessibility.Public,
                IsReadOnly: false,
                IsAbstract: false,
                IsStatic: false,
                IsExtern: false,
                GetMethod: not null,
                SetMethod: not null
            })];

            targetPropertySymbols.InsertRange(0, currentTargetProperties);
            currentTargetSymbol = currentTargetSymbol.BaseType;
        }

        while (currentSourceSymbol is { SpecialType: not SpecialType.System_Object })
        {
            List<IPropertySymbol> currentSourceProperties = [.. currentSourceSymbol.GetMembers().OfType<IPropertySymbol>().Where(propertySymbol => propertySymbol is {
                DeclaredAccessibility: Accessibility.Public,
                IsReadOnly: false,
                IsAbstract: false,
                IsStatic: false,
                IsExtern: false,
                GetMethod: not null,
                SetMethod: not null
            })];

            sourcePropertySymbols.InsertRange(0, currentSourceProperties);
            currentSourceSymbol = currentSourceSymbol.BaseType;
        }

        classModel.SourceProperties = sourcePropertySymbols;
        classModel.TargetProperties = targetPropertySymbols;
    }
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MappingGenerator;

internal static class GetDefaultPropertyValue
{
    internal static string Run(IPropertySymbol propertySymbol)
    {
        if (propertySymbol.DeclaringSyntaxReferences.Length == 0)
        {
            return "default";
        }

        if (propertySymbol.DeclaringSyntaxReferences[0].GetSyntax() is PropertyDeclarationSyntax propertySyntax)
        {
            if (propertySyntax.Initializer is EqualsValueClauseSyntax initializer)
            {
                string defaultValueCode = initializer.Value.ToString();
                return defaultValueCode;
            }
        }

        return "default";
    }
}
